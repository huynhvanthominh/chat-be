using System.ComponentModel.DataAnnotations;

namespace chat_be.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [RegularExpression("admin|user")]
        [StringLength(5)]
        public string Role { get; set; }
        public string DisplayName { get; set; }

        public UserModel(string Username, string Password, string Role = "user", string DisplayName = "")
        {
            this.Username = Username;
            this.Password = Password;
            this.Role = Role;
            this.DisplayName = DisplayName == "" ? Username : DisplayName;
        }
    }
}