using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;

namespace chat_be.Services.Abstracts{

    public interface IUserService{
        Task<UserModel?> GetUser(string username);
        Task<UserModel?> GetUser(string username, string password);
        Task AddFriend(AddFriendRequest request);
        Task ConfirmFriend(AddFriendRequest request);

        Task<List<UserResponse>> GetFriends();

        Task<UserModel> CreateUser(UserModel user);

        Task<UserModel> UpdateUser(UserModel user);
    }
}