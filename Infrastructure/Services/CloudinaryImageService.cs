using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Infrastructure.Services
{
    public class CloudinaryImageService : IImageHandler
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryImageService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<OperationResult<bool>> DeleteImageAsync(string imageUrl)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(imageUrl))
                {
                    return OperationResult<bool>.Fail("Image URL is null or empty", "CloudinaryImageService");
                }

                var uri = new Uri(imageUrl);
                var fileName = uri.Segments.Last();
                var publicId = fileName.Split('.').FirstOrDefault();

                var deletionParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                if(deletionResult.StatusCode != HttpStatusCode.OK)
                {
                    return OperationResult<bool>.Fail("Image deletion failed", "CloudinaryImageService");
                }

                return OperationResult<bool>.Success(true);
            }
            catch
            {
                return OperationResult<bool>.Fail("Could not delete image", "CloudinaryImageService");
            }
        }

        public async Task<OperationResult<string>> UploadImageAsync(IFormFile imageFile, string folderName)
        {
            try
            {
                if (imageFile.Length == 0 || imageFile == null)
                {
                    return OperationResult<string>.Fail("No Image selected.", "CloudinaryImageService");
                }

                await using var stream = imageFile.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = folderName,
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return OperationResult<string>.Fail(uploadResult.Error.Message, "CloudinaryImageService");
                }

                return OperationResult<string>.Success(uploadResult.SecureUrl.ToString());
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail(ex.Message, "CloudinaryImageService");
            }
        }
    }
}
