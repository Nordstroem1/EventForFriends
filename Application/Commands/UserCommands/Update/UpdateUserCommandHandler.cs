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
                   
                    return OperationResult<User>.Fail("User is null", "UpdateUserCommandHandler");
                }

                var loggedInUser = await userManager.FindByIdAsync(request.LoggedInUser);
                var foundUser = await userManager.FindByIdAsync(request.UserId.ToString());

                if (foundUser == null)
                {
                    logger.LogError("User not found");
                    return OperationResult<User>.Fail("User not found", "UpdateUserCommandHandler");
                }
                if(loggedInUser == null)
                {
                    logger.LogError("Logged in user not found");
                    return OperationResult<User>.Fail("Logged in user not found", "UpdateUserCommandHandler");
                }
                
                if (!string.IsNullOrWhiteSpace(request.UpdatedUser.UserName))
                {
                    if (!string.Equals(foundUser.UserName, request.UpdatedUser.UserName, StringComparison.OrdinalIgnoreCase))
                    {
                        var existingUser = await userManager.FindByNameAsync(request.UpdatedUser.UserName);
                        if (existingUser != null && existingUser.Id != foundUser.Id)
                        {
                            logger.LogError("Username is already taken or invalid");
                            return OperationResult<User>.Fail("Username is already taken or invalid", "UpdateUserCommandHandler");
                        }
                        foundUser.UserName = request.UpdatedUser.UserName;
                    }
                }

                if (request.UpdatedUser.NewProfilePicture is not null)
                {
                    var deleteResult = await imageHandler.DeleteImageAsync(foundUser.ProfilePicture);
                    var uploadResult = await imageHandler.UploadImageAsync(request.UpdatedUser.NewProfilePicture, "User");

                    if (!deleteResult.Succeeded || uploadResult.ErrorMessage.Length > 0 || uploadResult == null)
                    {
                        logger.LogError("Image upload failed.");
                        return OperationResult<User>.Fail("Image upload failed", "UpdateUserCommandHandler");
                    }
                    foundUser.ProfilePicture = uploadResult.Data;
                }
                else
                {
                    foundUser.ProfilePicture = request.UpdatedUser.OldImageUrl!;
                }

                var result = await userManager.UpdateAsync(foundUser);

                if (!result.Succeeded)
                {
                    logger.LogError("Failed to update user");
                    return OperationResult<User>.Fail("Failed to update user", "UpdateUserCommandHandler");
                }

                logger.LogInformation("User updated successfully");
                return OperationResult<User>.Success(foundUser);
            }
            catch
            {
                return OperationResult<User>.Fail("Unexpected error", "UpdateUserCommandHandler");
            }
        }
    }
}
