using System.ComponentModel.DataAnnotations;

namespace chat_be.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(6)]
        public required string Username { get; set; }
        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
        [Required]
        [MinLength(6)]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }

        public string? DisplayName { get; set; }
        public RegisterRequest(){
            DisplayName = DisplayName ?? Username;
        }
    }
}