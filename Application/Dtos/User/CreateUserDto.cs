using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string LivingLocation { get; set; }
        public CreateUserDto(string userName, string email, string password, string livinglocation)
        {
            UserName = userName;
            Email = email;
            Password = password;
            LivingLocation = livinglocation;
        }
        public CreateUserDto() { }
    }
}
