namespace chat_be.Models.Requests
{
    public class UpdateProfileRequest
    {
        public string DisplayName { get; set; }
        public IFormFile AvatarFile { get; set; }
    }
}