using System.ComponentModel.DataAnnotations;

namespace chat_be.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}