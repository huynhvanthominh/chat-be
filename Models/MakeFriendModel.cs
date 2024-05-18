using System.ComponentModel.DataAnnotations;

namespace chat_be.Models
{
    public class MakeFriendModel
    {
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public int FriendId { get; set; }
        public UserModel Friend { get; set; }
        public bool IsAccepted { get; set; }

        public MakeFriendModel()
        {
        }
        public MakeFriendModel(int userId, int friendId)
        {
            this.IsAccepted = false;
            this.UserId = userId;
            this.FriendId = friendId;
        }
    }
}