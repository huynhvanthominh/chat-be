using chat_be.Models;
using chat_be.Models.Responses;

namespace chat_be.Extensions
{
    public static class MakeFriendModelExtensions
    {
        public static FriendResponse ToFriendResponse(this MakeFriendModel makeFriend, UserModel current)
        {
            return new FriendResponse()
            {
                UserId = makeFriend.UserId == current.Id ? makeFriend.FriendId : makeFriend.UserId,
                Username = makeFriend.UserId == current.Id ? makeFriend.Friend.Username : makeFriend.User.Username,
                DisplayName = makeFriend.UserId == current.Id ? makeFriend.Friend.DisplayName : makeFriend.User.DisplayName,
                MessageGroupId = makeFriend.MessageGroupId
            };
        }

         public static List<FriendResponse> ToFriendResponse(this List<MakeFriendModel> makeFriend, UserModel current)
        {
            return makeFriend.Select(mf => mf.ToFriendResponse(current)).ToList();
        }

        public static PaginatedResponse<FriendResponse> ToFriendResponse(this PaginatedResponse<MakeFriendModel> makeFriends, UserModel current)
        {
            return new PaginatedResponse<FriendResponse>(
                makeFriends.TotalPage,
                makeFriends.CurrentPage,
                makeFriends.CountPerPage,
                makeFriends.TotalCount,
                makeFriends.Data.ToFriendResponse(current)
            );
        }
    }
}