
using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;

namespace chat_be.Services.Abstracts
{
    public interface IAdminUserService
    {
        Task<PaginatedResponse<UserResponse>> GetUsers(PaginateRequest options);
    }
}