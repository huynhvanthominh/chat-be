namespace chat_be.Models.Responses
{
    public class RegisterResponse
    {
        public string Username { get; set; }

        public RegisterResponse(string username)
        {
            Username = username;
        }
    }
}