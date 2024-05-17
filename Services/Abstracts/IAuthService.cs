using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;

namespace chat_be.Services.Abstracts
{
    public interface IAuthService
    {
        Task<UserModel> Register(RegisterRequest user);
        Task<LoginResponse> Login(LoginRequest user);

        Task<UserModel> CurrentUser();
    }
}