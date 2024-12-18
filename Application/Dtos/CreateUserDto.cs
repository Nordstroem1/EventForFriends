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
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phonenumber is required")]
        public int PhoneNumber { get; set; }
        public CreateUserDto(string userName, string email, string password, int phoneNumber)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }
    }
}
