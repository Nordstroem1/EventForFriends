using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IImageUploader
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
