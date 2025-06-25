using Microsoft.AspNetCore.Http;

namespace Application.Dtos.User
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string? OldImageUrl { get; set; } = string.Empty;
        public IFormFile? NewProfilePicture { get; set; }
    }
}
