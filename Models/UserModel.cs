using System.ComponentModel.DataAnnotations;
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

        public virtual ICollection<MakeFriendModel> MakeFriendRequests { get; set; }
        public virtual ICollection<MakeFriendModel> ReceivedFriendRequests { get; set; }

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
            this.MakeFriendRequests = [];
            this.ReceivedFriendRequests = [];
        }
    }

    public static class UserModelExtensions
    {
        public static UserResponse ToResponse(this UserModel user)
        {
            return new UserResponse(
                user.Username,
                user.DisplayName
            );
        }
        public static List<UserResponse> ToResponse(this List<UserModel> users)
        {
            return users.Select(x => x.ToResponse()).ToList();
        }
    }
}