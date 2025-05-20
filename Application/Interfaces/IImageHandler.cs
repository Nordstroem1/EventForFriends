using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IImageHandler
    {
        Task<OperationResult<string>> UploadImageAsync(IFormFile file, string folderName);
        Task<OperationResult<bool>> DeleteImageAsync(string imageUrl);
    }
}

