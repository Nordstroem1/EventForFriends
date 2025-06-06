using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.Update
{
    public class UpdateUserCommandHandler(IImageHandler imageHandler, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UpdateUserCommandHandler> logger, IMapper mapper) : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        public async Task<OperationResult<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdatedUser == null)
                {
                    logger.LogError("User is null");
                   
                    return OperationResult<User>.Fail("User is null", "Application");
                }

                var loggedInUser = await userManager.FindByIdAsync(request.LoggedInUser);
                var foundUser = await userManager.FindByIdAsync(request.UserId.ToString());

                if (foundUser == null)
                {
                    logger.LogError("User not found");
                    return OperationResult<User>.Fail("User not found", "Application");
                }
                if(loggedInUser == null)
                {
                    logger.LogError("Logged in user not found");
                    return OperationResult<User>.Fail("Logged in user not found", "Application");
                }

                if (!string.IsNullOrWhiteSpace(request.UpdatedUser.UserName))
                    foundUser.UserName = request.UpdatedUser.UserName;

                if (request.UpdatedUser.ProfilePicture != null)
                {
                   var deleteResult =   await imageHandler.DeleteImageAsync(foundUser.ProfilePicture);
                   var uploadResult = await imageHandler.UploadImageAsync(request.UpdatedUser.ProfilePicture, "User");

                    if (!deleteResult.Succeeded || uploadResult.ErrorMessage.Length > 0 || uploadResult == null)
                    {
                        logger.LogError("Image upload failed.");
                        return OperationResult<User>.Fail("Image upload failed", "UpdateUserCommandHandler");
                    }
                    foundUser.ProfilePicture = uploadResult.Data;
                }

                if (loggedInUser.Id != request.UserId
                && loggedInUser.Role.ToLower() != "admin"
                && loggedInUser.Role.ToLower() != "superadmin")
                {
                    logger.LogError("User does not have permission to delete user");
                    return OperationResult<User>.Fail("User does not have permission to delete user", "Application");
                }

                var result = await userManager.UpdateAsync(foundUser);

                if (!result.Succeeded)
                {
                    logger.LogError("Failed to update user");
                    return OperationResult<User>.Fail("Failed to update user", "Application");
                }

                logger.LogInformation("User updated successfully");
                return OperationResult<User>.Success(foundUser);
            }
            catch
            {
                return OperationResult<User>.Fail("Unexpected error", "Application");
            }
        }
    }
}
