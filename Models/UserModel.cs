using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
namespace chat_be.Models
{

    public enum UserRole
    {
        admin,
        user
    }

    [Index(nameof(Username), nameof(DisplayName), IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(5)]
        public UserRole Role { get; set; }
        [AllowNull]
        public string? DisplayName { get; set; }
        [AllowNull]
        public string? Avatar { get; set; }

        [NotMapped]
        public ICollection<MakeFriendModel> Friends { get; set; }

        public UserModel()
        {
            Avatar = "";
            Username = "";
            Password = "";
        }

        public UserModel(
            string Username,
             string Password,
              UserRole Role,
              string? DisplayName)
        {
            this.Username = Username;
            this.Password = Password;
            this.Role = Role;
            this.DisplayName = DisplayName == "" ? Username : DisplayName;
        }
    }
}