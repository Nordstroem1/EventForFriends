using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.ChangeRole
{
    public class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ChangeRoleCommandHandler> _logger;

        public ChangeRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<ChangeRoleCommandHandler> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<OperationResult<User>> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.ChangeRoleDto.UserId == Guid.Empty)
            {
                _logger.LogError("UserId was null or empty.");
                return OperationResult<User>.Fail("User ID is empty", "Application");
            }

            var foundUser = await _userManager.FindByIdAsync(request.ChangeRoleDto.UserId.ToString());
            if(foundUser == null)
            {
                _logger.LogError("Could not find user.");
                return OperationResult<User>.Fail("User not found", "Application");
            }

            var foundRole = await _roleManager.RoleExistsAsync(request.ChangeRoleDto.Role.ToString());
            if(!foundRole)
            {
                _logger.LogError("Role not found.");
                return OperationResult<User>.Fail("Role not found", "Application");
            }

            var addRole = await _userManager.AddToRoleAsync(foundUser, request.ChangeRoleDto.Role.ToString());
            if (!addRole.Succeeded)
            {
                _logger.LogError("Failed to add role.");
                return OperationResult<User>.Fail("Failed to add role", "Application");
            }
            _logger.LogInformation("Role added successfully.");

            return OperationResult<User>.Success(foundUser);
        }
    }
}
