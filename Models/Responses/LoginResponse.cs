using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace chat_be.Models.Responses
{
    public class LoginResponse
    {
        [DisplayName("token")]
        [Required]
        public string Token { get; set; }
        [DisplayName("refreshToken")]
        public string? RefreshToken { get; set; }
        [DisplayName("expires")]
        [Required]
        public DateTime Expires { get; set; }

        public LoginResponse(string token, string? refreshToken, DateTime expires)
        {
            Token = token;
            RefreshToken = refreshToken;
            Expires = expires;
        }
    }
}