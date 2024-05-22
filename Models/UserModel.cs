using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using chat_be.Models.Responses;
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
        [StringLength(5)]
        public UserRole Role { get; set; }
        public string DisplayName { get; set; }
        [AllowNull]
        public string? Avatar { get; set; }

        public UserModel()
        {
            Avatar = "";
        }

        public UserModel(
            string Username,
             string Password,
              UserRole Role,
              string DisplayName
               )
        {
            this.Username = Username;
            this.Password = Password;
            this.Role = Role;
            this.DisplayName = DisplayName == "" ? Username : DisplayName;
        }
    }

    public static class UserModelExtensions
    {
        public static UserResponse ToResponse(this UserModel user)
        {
            return new UserResponse(
                user.Id,
                user.Username,
                user.DisplayName,
                user.Avatar
            );
        }
        public static List<UserResponse> ToResponse(this List<UserModel> users)
        {
            return users.Select(x => x.ToResponse()).ToList();
        }
    }
}