using System.ComponentModel.DataAnnotations.Schema;

namespace chat_be.Models.Responses
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MessageGroupId { get; set; }
        public List<int> UserIds { get; set; }
        public MessageResponse() { }
    }
}