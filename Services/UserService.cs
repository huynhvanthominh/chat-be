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
        public async Task<List<UserModel>> GetFriends()
        {
            using (var context = _context)
            {
                var currentUser = await _authService.CurrentUser();
                _logger.LogInformation("Current user: {0}", currentUser.Username);
                var friends = await context.MakeFriendModels
                    .Where(x => x.UserId == currentUser.Id)
                    .Select(x => x.Friend)
                    .ToListAsync();
                return await Task.FromResult(friends);
            }
        }

        public async Task<PaginatedResponse<UserModel>> GetMakeFriendRequests(
            PaginateRequest options
        )
        {
            var currentUser = await _authService.CurrentUser();
            var friendRequests = await  _context.MakeFriendModels
                .Where(x => x.UserId == currentUser.Id)
                .Select(x => x.Friend)
                .ToPaginatedListAsync(options.Page, options.CountPerPage);
            return await Task.FromResult(friendRequests);
        }
        

        public Task<List<UserModel>> GetReceivedFriendRequests()
        {
            var currentUser = _authService.CurrentUser();
            var receivedFriendRequests = _context.MakeFriendModels
                .Where(x => x.UserId == currentUser.Id)
                .Select(x => x.Friend)
                .ToListAsync();

            return receivedFriendRequests;
        }

        public Task<UserModel?> GetUser(string username)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task<UserModel?> GetUser(string username, string password)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }

        public async Task<MakeFriendModel> AddFriend(AddFriendRequest request)
        {
            var currentUser = await _authService.CurrentUser();
            var user = await GetUser(request.Username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (currentUser.Id == user.Id)
            {
                throw new Exception("Cannot add yourself as a friend");
            }
            _logger.LogInformation("User: {0}", user.Username);
            _logger.LogInformation("Current user: {0}", currentUser.Username);
            var makeFriend = new MakeFriendModel(currentUser.Id, user.Id);
            _context.MakeFriendModels.Add(makeFriend);
            await _context.SaveChangesAsync();
            return await Task.FromResult(makeFriend);
        }

        public async Task ConfirmFriend(AddFriendRequest request)
        {
            throw new NotImplementedException();
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