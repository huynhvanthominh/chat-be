using System.ComponentModel.DataAnnotations.Schema;

namespace chat_be.Models
{
    public class MessageGroupModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public ICollection<MessageGroupUserModel> MessageGroupUsers { get; set; }
        public MessageGroupModel(){
            Name = "";
            MessageGroupUsers = new List<MessageGroupUserModel>();
        }

    }

    public class MessageGroupUserModel
    {
        public int Id { get; set; }
        public int MessageGroupId { get; set; }
        public int UserId { get; set; }
        [NotMapped]
        public required UserModel User { get; set; }
        [NotMapped]
        public required MessageGroupModel MessageGroup { get; set; }
    }
}