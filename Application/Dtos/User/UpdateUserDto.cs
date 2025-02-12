using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.User
{
    public class UpdateUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string Role { get; set; }
        public UpdateUserDto(Guid id, string userName, string email, string password, int phoneNumber, string role)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Role = role;
        }
    }
}
