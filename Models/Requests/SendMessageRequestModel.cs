namespace chat_be.Models.Requests
{
    public class SendMessageRequest 
    {
        public string Message { get; set; }
        public int? MessageGroupId { get; set; }
        
    }
}