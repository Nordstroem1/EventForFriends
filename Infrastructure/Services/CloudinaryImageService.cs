using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CloudinaryImageService : IImageUploader
    {
        public Task<bool> DeleteImageAsync(string imageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
