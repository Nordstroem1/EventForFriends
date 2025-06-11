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
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public CreateUserDto(string userName, string email, string password, string longitude, string latitude)
        {
            UserName = userName;
            Email = email;
            Password = password;
            Longitude = longitude;
            Latitude = latitude;
        }
        public CreateUserDto() { }
    }
}
