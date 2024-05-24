namespace chat_be.Models.Responses{
    
    public class FriendResponse{
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName  { get; set; }

        public int MessageGroupId { get; set; }
    }
}