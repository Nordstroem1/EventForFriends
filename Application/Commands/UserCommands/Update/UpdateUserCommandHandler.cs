using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        public UpdateUserCommandHandler(UserManager<User> userManager, ILogger<UpdateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<OperationResult<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdatedUser == null)
                {
                    _logger.LogError("User is null");
                    return OperationResult<User>.Fail("User is null", "Application");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserId.ToString());

                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<User>.Fail("User not found", "Application");
                }

                foundUser.UserName = request.UpdatedUser.UserName;
                foundUser.Email = request.UpdatedUser.Email;
                foundUser.PhoneNumber = request.UpdatedUser.PhoneNumber.ToString();
                foundUser.Role = request.UpdatedUser.Role;

                if (!string.IsNullOrEmpty(request.UpdatedUser.Password))
                {
                    foundUser.PasswordHash = _userManager.PasswordHasher.HashPassword(foundUser, request.UpdatedUser.Password);
                }

                var result = await _userManager.UpdateAsync(foundUser);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to update user");
                    return OperationResult<User>.Fail("Failed to update user", "Application");
                }
                _logger.LogInformation("User updated successfully");

                return OperationResult<User>.Success(foundUser);
            }
            catch
            {
                return OperationResult<User>.Fail("Unexpected error", "Application");
            }
        }
    }
}
