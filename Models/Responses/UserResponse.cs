namespace chat_be.Models.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        
        public UserResponse(string username, string displayName)
        {
            Username = username;
            DisplayName = displayName;
        }
    }
}