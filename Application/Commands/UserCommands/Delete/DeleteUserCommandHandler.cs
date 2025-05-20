using Application.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.Delete
{
    public class DeleteUserCommandHandler(IImageHandler imageHandler, IPermissionChecker permissionChecker, UserManager<User> userManager, ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand, OperationResult<string>>
    {
        public async Task<OperationResult<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.UserId))
            {
                logger.LogError("User ID is empty");
                return OperationResult<string>.Fail("User ID is empty", "Application");
            }

            User LoggedInUser = await userManager.FindByIdAsync(request.loggedinUser);
            if (LoggedInUser == null)
            {
                logger.LogError("Could not find logged in user");
                return OperationResult<string>.Fail("Could not find logged in user", "Application");
            }

            if(LoggedInUser.Id != request.UserId 
                && LoggedInUser.Role.ToLower() != "admin" 
                && LoggedInUser.Role.ToLower() != "superadmin")
            {
                logger.LogError("User does not have permission to delete user");
                return OperationResult<string>.Fail("User does not have permission to delete user", "Application");
            }

            User foundUser = await userManager.FindByIdAsync(request.UserId);

            if(foundUser == null)
            {
                logger.LogError("User not found");
                return OperationResult<string>.Fail("User not found", "Application");
            }

            if(!string.IsNullOrWhiteSpace(foundUser.ProfilePicture))
            {
                var imageDeletion = await imageHandler.DeleteImageAsync(foundUser.ProfilePicture);
                if (!imageDeletion.Succeeded)
                {
                    logger.LogError("Could not delete image: " + imageDeletion.ErrorMessage);
                    return OperationResult<string>.Fail("Could not delete image", "Application");
                }
            }

            var result = await userManager.DeleteAsync(foundUser);
            if (!result.Succeeded)
            {
                logger.LogError("Failed to delete user");
                return OperationResult<string>.Fail("Failed to delete user", "Application");
            }
            logger.LogInformation("Successfully deleted user");

            return OperationResult<string>.Success("Successfully deleted user.");
        }
    }
}
