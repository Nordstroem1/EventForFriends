using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
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

                var publicId = ExtractPublicIdFromCloudinaryUrl(imageUrl);

                if (string.IsNullOrWhiteSpace(publicId))
                {
                    return OperationResult<bool>.Fail("Could not extract public ID from image URL", "CloudinaryImageService");
                }

                var deletionParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                if (deletionResult.StatusCode != HttpStatusCode.OK || deletionResult.Result.ToLower() == "not found")
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
        private static string ExtractPublicIdFromCloudinaryUrl(string imageUrl)
        {
            try
            {
                var uri = new Uri(imageUrl);
                var path = uri.AbsolutePath;
                var uploadMarker = "/upload/";

                var uploadIndex = path.IndexOf(uploadMarker, StringComparison.OrdinalIgnoreCase);

                if (uploadIndex < 0)
                {
                    return null;
                }

                var publicIdWithExtension = path[(uploadIndex + uploadMarker.Length)..];

                var segments = publicIdWithExtension.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length > 0 && segments[0].StartsWith("v") && long.TryParse(segments[0][1..], out _))
                {
                    publicIdWithExtension = string.Join('/', segments.Skip(1));
                }

                var dotIndex = publicIdWithExtension.LastIndexOf('.');
                return dotIndex > 0 ? publicIdWithExtension[..dotIndex] : publicIdWithExtension;
            }
            catch
            {
                return null;
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
