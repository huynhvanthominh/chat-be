using System.ComponentModel.DataAnnotations;

namespace chat_be.Models
{
    public enum MessageType
    {
        Text,
        Image,
        Video,
        Audio,
        File
    }
    public class MessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int MessageGroupId { get; set; }
        public MessageType Type { get; set; }
        public string? Url { get; set; }
        public MessageModel() {
            CreatedAt = DateTime.Now;
        }
    }

}