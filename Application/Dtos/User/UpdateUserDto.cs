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
        public UpdateUserDto(string userName, string email, string password, int phoneNumber)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }
    }
}
