using chat_be.Models;
using chat_be.Models.Responses;

namespace chat_be.Mappers.Abstracts
{
    public interface IUserMapper
    {
        public UserResponse MapUserModelToUserResponse(UserModel user);
        public List<UserResponse> MapUserModelToUserResponse(List<UserModel> users);
    }
}