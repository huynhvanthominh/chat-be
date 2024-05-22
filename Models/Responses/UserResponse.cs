namespace chat_be.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
public string Avatar { get; set;}
        
        public UserResponse(int userId, string username, string displayName, string avatar)
        {
            Id = userId;
            Username = username;
            DisplayName = displayName;
            Avatar = avatar;
        }
    }
}