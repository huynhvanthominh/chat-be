using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace chat_be.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string DisplayName { get; set; }
    }
}