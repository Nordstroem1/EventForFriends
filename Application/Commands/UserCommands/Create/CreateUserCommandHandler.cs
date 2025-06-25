using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Transactions;

namespace Application.Commands.UserCommands.Create
{
    public class CreateUserCommandHandler(IImageHandler imageHandler, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<CreateUserCommandHandler> logger, IMapper mapper) : IRequestHandler<CreateUserCommand, OperationResult<User>>
    {
        public async Task<OperationResult<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ParseCoordinates(request.UserDto);

                var createdUser = mapper.Map<User>(request.UserDto);

                createdUser.Role = "user";

                if(!await roleManager.RoleExistsAsync(createdUser.Role))
                {
                    var createRoleResult = await roleManager.CreateAsync(new IdentityRole("user"));

                    if (!createRoleResult.Succeeded)
                    {
                        logger.LogError($"Error when creating a user role: ");

                        return OperationResult<User>.Fail($"Failed to create user role: ", "Application");
                    }
                }

                var uploadImageResult = await imageHandler.UploadImageAsync(request.ProfilePicture, "User");

                if (uploadImageResult.ErrorMessage.Length > 0 || uploadImageResult == null)
                {
                    logger.LogError("Image upload failed.");
                    return OperationResult<User>.Fail("Image upload failed", "CreateUserCommandHandler");
                }
                createdUser.ProfilePicture = uploadImageResult.Data;
                
                var userResult = await userManager.CreateAsync(createdUser, request.UserDto.Password);
               
                await userManager.UpdateAsync(createdUser); 

                if (!userResult.Succeeded)
                {
                    var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                    logger.LogError($"Error when creating a user: {errors}");
                    await imageHandler.DeleteImageAsync(createdUser.ProfilePicture);

                    return OperationResult<User>.Fail(errors, "Application");
                }

                var roleResult = await userManager.AddToRoleAsync(createdUser, createdUser.Role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    logger.LogError($"Error when adding user to role: {errors}");
                    return OperationResult<User>.Fail($"Failed to add user to role: {errors}", "Application");
                }

                return OperationResult<User>.Success(createdUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error." + ex.Message);
            }
        }

        private void ParseCoordinates(CreateUserDto userDto)
        {
            string latNormalized = userDto.Latitude?.Replace(',', '.');
            string lonNormalized = userDto.Longitude?.Replace(',', '.');

            if (!double.TryParse(latNormalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude))
                throw new ArgumentException("Could not change latitude format.");

            if (!double.TryParse(lonNormalized, NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude))
                throw new ArgumentException("Could not change longitude format.");
        }
    }
}
