using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace chat_be.Models
{
    public class MakeFriendModel
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public bool IsAccepted { get; set; }
        [AllowNull]
        public int? MessageGroupId { get; set; }

        [NotMapped]
        public UserModel User { get; set; }
        [NotMapped]
        public UserModel Friend { get; set; }
        [NotMapped]
        [AllowNull]
        public MessageGroupModel? MessageGroupModel { get; set; }

        public MakeFriendModel() {
            this.IsAccepted = false;
            this.MessageGroupId = null;
         }
        public MakeFriendModel(int userId, int friendId, int messageGroupModelId)
        {
            this.IsAccepted = false;
            this.UserId = userId;
            this.FriendId = friendId;
            this.MessageGroupId = messageGroupModelId;
        }
    }

    
}