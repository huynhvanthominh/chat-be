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

        public UserService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task AddFriend(AddFriendRequest request)
        {
            return;
        }
        public async Task ConfirmFriend(AddFriendRequest request)
        {
            return;
        }

        public async Task<UserModel> CreateUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }

        public async Task<List<UserResponse>> GetFriends()
        {
            return new List<UserResponse>();
        }

        public Task<UserModel?> GetUser(string username)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task<UserModel?> GetUser(string username, string password)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}