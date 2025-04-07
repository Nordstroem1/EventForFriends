using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<string>>
    {
        private readonly UserManager<User> _userManager;
        readonly ILogger<DeleteUserCommandHandler> _logger;
        public DeleteUserCommandHandler(UserManager<User> userManager, ILogger<DeleteUserCommandHandler> logger)
        {
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<OperationResult<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.UserId))
            {
                _logger.LogError("User ID is empty");
                return OperationResult<string>.Fail("User ID is empty", "Application");
            }

            User LoggedInUser = await _userManager.FindByIdAsync(request.loggedinUser);
            if (LoggedInUser == null)
            {
                _logger.LogError("Could not find logged in user");
                return OperationResult<string>.Fail("Could not find logged in user", "Application");
            }

            if(LoggedInUser.Id != request.UserId 
                && LoggedInUser.Role.ToLower() != "admin" 
                && LoggedInUser.Role.ToLower() != "superadmin")
            {
                _logger.LogError("User does not have permission to delete user");
                return OperationResult<string>.Fail("User does not have permission to delete user", "Application");
            }

            User foundUser = await _userManager.FindByIdAsync(request.UserId);

            if(foundUser == null)
            {
                _logger.LogError("User not found");
                return OperationResult<string>.Fail("User not found", "Application");
            }

            var result = await _userManager.DeleteAsync(foundUser);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete user");
                return OperationResult<string>.Fail("Failed to delete user", "Application");
            }
            _logger.LogInformation("Successfully deleted user");

            return OperationResult<string>.Success("Successfully deleted user.");
        }
    }
}
