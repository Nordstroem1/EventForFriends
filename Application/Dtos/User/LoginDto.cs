using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class LoginDto
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginDto(string? email, string? userName, string password)
        {
            Email = email;
            UserName = userName;
            Password = password;
        }
    }
}
