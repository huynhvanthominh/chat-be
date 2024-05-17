namespace chat_be.Models.Responses
{
    public class UserResponse
    {
        public string Username { get; set; }

        public UserResponse(string username)
        {
            Username = username;
        }
    }
}