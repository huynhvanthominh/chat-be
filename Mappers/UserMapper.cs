using chat_be.Mappers.Abstracts;
using chat_be.Models;
using chat_be.Models.Responses;

namespace chat_be.Mappers
{

    public class UserMapper : IUserMapper
    {
        // public UserResponse MapUserModelToUserResponse(UserModel user)
        // {
        //     return new UserResponse(
        //          user.Username
        //     );
        // }

        // public List<UserResponse> MapUserModelToUserResponse(List<UserModel> users)
        // {
        //     List<UserResponse> userResponses = new List<UserResponse>();
        //     foreach (var user in users)
        //     {
        //         userResponses.Add(
        //             this.MapUserModelToUserResponse(user)
        //         );
        //     }
        //     return userResponses;
        // }
    }
}