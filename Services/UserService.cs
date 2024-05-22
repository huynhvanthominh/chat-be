using chat_be.Data;
using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

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
            PaginateRequest options
        )
        {
            var currentUser = await _authService.CurrentUser();
            var friends = await _context.MakeFriendModels
                .Where(x => (x.UserId == currentUser.Id || x.FriendId == currentUser.Id) && x.IsAccepted)
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
                        UserId = currentUser.Id
                    };
                    var messageGroupUser2 = new MessageGroupUserModel()
                    {
                        MessageGroupId = messageGroup.Id,
                        UserId = x.user.Id
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
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task<UserModel?> GetUser(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<UserModel?> GetUser(string username, string password)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
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
                UserId = currentUser.Id
            };
            var messageGroupUser2 = new MessageGroupUserModel()
            {
                MessageGroupId = messageGroup.Id,
                UserId = user.Id
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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }
    }

    public static class IUserServiceExtensions
    {
        public static PaginatedResponse<UserResponse> ToResponse(this PaginatedResponse<UserModel> paginatedResponse)
        {
            return new PaginatedResponse<UserResponse>(
                paginatedResponse.TotalPage,
                paginatedResponse.CurrentPage,
                paginatedResponse.CountPerPage,
                paginatedResponse.TotalCount,
                paginatedResponse.Data.Select(x => x.ToResponse()).ToList()
            )!;
        }
    }
}