using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;

namespace chat_be.Services.Abstracts{

    public interface IUserService{
        Task<UserModel?> GetUser(string username);
        Task<UserModel?> GetUser(int userId);
        Task<UserModel?> GetUser(string username, string password);
        Task<PaginatedResponse<MakeFriendModel>> GetFriends(
            PaginateRequest options
        );
        Task<PaginatedResponse<UserModel>> GetMakeFriendRequests(
            PaginateRequest options
        );
        Task<PaginatedResponse<UserModel>> GetReceivedFriendRequests(
            PaginateRequest options
        );
        Task<MakeFriendModel> AddFriend(AddFriendRequest request);
        Task<MakeFriendModel> ConfirmFriend(ConfirmFriendRequest request);
        Task<UserModel> CreateUser(UserModel user);
        Task<UserModel> UpdateUser(UserModel user);
    }
}