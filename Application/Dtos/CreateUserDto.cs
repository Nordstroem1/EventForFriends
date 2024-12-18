using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phonenumber is required")]
        public int PhoneNumber { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public RoleEnums.Roles Role { get; set; }
        public CreateUserDto(string userName, string email, string password, int phoneNumber, RoleEnums roleEnums)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Role = RoleEnums.Roles.user;
        }
    }
}
