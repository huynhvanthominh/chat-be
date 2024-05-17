using System.ComponentModel.DataAnnotations;

namespace chat_be.Models
{

    public enum UserRole
    {
        admin,
        user
    }

    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [RegularExpression("admin|user")]
        [StringLength(5)]
        public UserRole Role { get; set; }
        public string DisplayName { get; set; }

        public UserModel(string Username, string Password, UserRole Role, string DisplayName)
        {
            this.Username = Username;
            this.Password = Password;
            this.Role = Role;
            this.DisplayName = DisplayName == "" ? Username : DisplayName;
        }
    }
}