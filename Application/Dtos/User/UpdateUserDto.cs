using Microsoft.AspNetCore.Http;

namespace Application.Dtos.User
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public IFormFile? ProfilePicture { get; set; }
        public UpdateUserDto(string userName, string location, IFormFile profilePic)
        {
            Location = location;
            UserName = userName;
            ProfilePicture = profilePic;
        }
    }
}
