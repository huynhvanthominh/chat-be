
using chat_be.Models;

namespace chat_be.Services.Abstracts
{
    public interface IAdminUserService
    {
        Task<List<UserModel>> GetUsers();

        void initAdmin();
    }
}