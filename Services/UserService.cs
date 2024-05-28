using chat_be.Data;
using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace chat_be.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            DatabaseContext context,
            IAuthService authService,
            ILogger<UserService> logger
            )
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        public async Task<PaginatedResponse<MakeFriendModel>> GetFriends(
            PaginateRequest options,
            bool isAccepted = true
        )
        {
            var currentUser = await _authService.CurrentUser();
            var query = _context.MakeFriendModels;
            if (options.Search != null)
            {
                query.Where(x => x.User.Username.Contains(options.Search));
            }
            var friends = await query
            .Where(x => (x.UserId == currentUser.Id || x.FriendId == currentUser.Id) && x.IsAccepted == isAccepted)
            .Join(
                    _context.Users,
                    x => x.UserId == currentUser.Id ? x.FriendId : x.UserId,
                    x => x.Id,
                    (makeFriend, user) => new { makeFriend, user }
                )
                .ToPaginatedListAsync(options.Page, options.CountPerPage);
            var isCallback = false;
            friends.Data.ForEach(async x =>
            {
                if (x.makeFriend.MessageGroupId == null || x.makeFriend.MessageGroupId == 0)
                {
                    var messageGroup = new MessageGroupModel();
                    await _context.MessageGroupModels.AddAsync(messageGroup);
                    await _context.SaveChangesAsync();
                    var messageGroupUser1 = new MessageGroupUserModel()
                    {
                        MessageGroupId = messageGroup.Id,
                        UserId = currentUser.Id,
                        MessageGroup = messageGroup,
                        User = currentUser
                    };
                    var messageGroupUser2 = new MessageGroupUserModel()
                    {
                        MessageGroupId = messageGroup.Id,
                        UserId = x.user.Id,
                        MessageGroup = messageGroup,
                        User = x.user
                    };
                    await _context.MessageGroupUserModels.AddAsync(messageGroupUser1);
                    await _context.MessageGroupUserModels.AddAsync(messageGroupUser2);
                    await _context.SaveChangesAsync();
                    isCallback = true;
                }
            });
            if (isCallback)
            {
                return await GetFriends(options);
            }
            return await Task.FromResult(
                new PaginatedResponse<MakeFriendModel>(
                    friends.TotalPage,
                    friends.CurrentPage,
                    friends.CountPerPage,
                    friends.TotalCount,
                    friends.Data.Select(x => x.makeFriend).ToList()
                ));
        }

        public async Task<PaginatedResponse<UserModel>> GetMakeFriendRequests(
            PaginateRequest options
        )
        {
            var currentUser = await _authService.CurrentUser();
            var friendRequests = await _context.MakeFriendModels
                .Where(x => x.UserId == currentUser.Id && !x.IsAccepted)
                .Join(
                    _context.Users,
                    x => x.FriendId,
                    x => x.Id,
                    (makeFriend, user) => user
                )
                .ToPaginatedListAsync(
                    options.Page,
                    options.CountPerPage
                );
            return await Task.FromResult(friendRequests);
        }


        public async Task<PaginatedResponse<UserModel>> GetReceivedFriendRequests(
            PaginateRequest options
        )
        {
            var currentUser = await _authService.CurrentUser();
            var receivedFriendRequests = await _context.MakeFriendModels
                .Where(x => x.FriendId == currentUser.Id && !x.IsAccepted)
                .Join(
                    _context.Users,
                    x => x.UserId,
                    x => x.Id,
                    (makeFriend, user) => user
                )
                .ToPaginatedListAsync(
                    options.Page,
                    options.CountPerPage
                );

            return await Task.FromResult(receivedFriendRequests);
        }

        public Task<UserModel?> GetUser(string username)
        {
            return Task.FromResult(_context.Users.AsEnumerable().FirstOrDefault(x => x.Username.Equals(username, StringComparison.Ordinal)));
        }

        public Task<UserModel?> GetUser(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<UserModel?> GetUser(string username, string password)
        {
            var user = await GetUser(username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new Exception("Invalid username or password");
            }
            return await Task.FromResult(user);
        }

        public async Task<PaginatedResponse<UserResponse>> SearchUsers(PaginateRequest options)
        {
            var query = _context.Users;
            query.Where(x => x.Id != _authService.CurrentUser().Id)
            .Where(x => x.Role == UserRole.user);

            if (options.Search != null)
            {
                query.Where(x => x.Username.Contains(options.Search) || (x.DisplayName != null && x.DisplayName.Contains(options.Search)));
            }
            return await query.Select(x => x.ToResponse()).ToPaginatedListAsync(options.Page, options.CountPerPage);
        }
        public async Task<MakeFriendModel> AddFriend(AddFriendRequest request)
        {
            var currentUser = await _authService.CurrentUser();
            var user = await GetUser(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (currentUser.Id == user.Id)
            {
                throw new Exception("Cannot add yourself as a friend");
            }
            var messageGroup = new MessageGroupModel();
            await _context.MessageGroupModels.AddAsync(messageGroup);
            await _context.SaveChangesAsync();
            var messageGroupUser1 = new MessageGroupUserModel()
            {
                MessageGroupId = messageGroup.Id,
                UserId = currentUser.Id,
                User = currentUser,
                MessageGroup = messageGroup
            };
            var messageGroupUser2 = new MessageGroupUserModel()
            {
                MessageGroupId = messageGroup.Id,
                UserId = user.Id,
                User = user,
                MessageGroup = messageGroup
            };
            await _context.MessageGroupUserModels.AddAsync(messageGroupUser1);
            await _context.MessageGroupUserModels.AddAsync(messageGroupUser2);
            await _context.SaveChangesAsync();
            var makeFriend = new MakeFriendModel(currentUser.Id, user.Id, messageGroup.Id);
            _context.MakeFriendModels.Add(makeFriend);
            await _context.SaveChangesAsync();
            return await Task.FromResult(makeFriend);
        }

        public async Task<MakeFriendModel> ConfirmFriend(ConfirmFriendRequest request)
        {
            var currentUser = await _authService.CurrentUser();
            var makeFriend = await _context.MakeFriendModels
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.FriendId == currentUser.Id && !x.IsAccepted);
            if (makeFriend == null)
            {
                throw new Exception("Friend request not found");
            }
            makeFriend.IsAccepted = true;
            _context.MakeFriendModels.Update(makeFriend);
            await _context.SaveChangesAsync();
            return await Task.FromResult(makeFriend);
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            var userExists = await GetUser(user.Username);
            if (userExists != null)
            {
                throw new Exception("User already exists");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            var userExists = await GetUser(user.Id);
            if (userExists == null)
            {
                throw new Exception("User not found");
            }
            user.Password ??= userExists.Password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }
    }
}