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
            try
            {
                if (request == null || request.ChangeRoleDto.UserId == Guid.Empty)
                {
                    _logger.LogError("UserId was null or empty.");
                    return OperationResult<User>.Fail("User ID is empty", "ChangeRoleCommandHandler");
                }

                var loggedInUser = await _userManager.FindByIdAsync(request.LoggedInUserId);

                if (loggedInUser == null)
                {
                    _logger.LogError("Could not find logged in user.");
                    return OperationResult<User>.Fail("Could not find logged in user", "ChangeRoleCommandHandler");
                }

                var foundUser = await _userManager.FindByIdAsync(request.ChangeRoleDto.UserId.ToString());

                if (foundUser == null)
                {
                    _logger.LogError("Could not find user.");
                    return OperationResult<User>.Fail("User not found", "ChangeRoleCommandHandler");
                }

                if (loggedInUser.Role == "admin" && request.ChangeRoleDto.Role == "user")
                {
                    var grantUserRole = await _userManager.AddToRoleAsync(foundUser, request.ChangeRoleDto.Role);
                }

                if (loggedInUser.Role == "superadmin")
                {
                    return await GrantAdminRoleAsSuperAdmin(request, foundUser);
                }
                if (loggedInUser.Role == "admin")
                {
                    return await GrantAdminRoleAsAdmin(request, foundUser);
                }

                var foundRole = await _roleManager.RoleExistsAsync(request.ChangeRoleDto.Role);
                if (!foundRole)
                {
                    _logger.LogError("Role not found.");
                    return OperationResult<User>.Fail("Role not found", "ChangeRoleCommandHandler");
                }

                return OperationResult<User>.Fail("Something went wrong wile changing role. Check permissions. ", "ChangeRoleCommandHandler");
            }
            catch
            {
                _logger.LogError("Something went wrong while changing role.");
                return OperationResult<User>.Fail("Something went wrong while changing role.", "ChangeRoleCommandHandler");
            }
        }

        private async Task<OperationResult<User>> GrantAdminRoleAsSuperAdmin(ChangeRoleCommand request, User? foundUser)
        {
            if (request.ChangeRoleDto.Role.ToLower() == "admin")
            {
                var grantAdminRole = await _userManager.AddToRoleAsync(foundUser, request.ChangeRoleDto.Role);
                return OperationResult<User>.Success(foundUser);

            }

            if (request.ChangeRoleDto.Role.ToLower() == "superadmin")
            {
                var grantSuperAdminRole = await _userManager.AddToRoleAsync(foundUser, request.ChangeRoleDto.Role);
                return OperationResult<User>.Success(foundUser);
            }

            return OperationResult<User>.Fail("Role not found", "ChangeRoleCommandHandler");
        }
        private async Task<OperationResult<User>> GrantAdminRoleAsAdmin(ChangeRoleCommand request, User? foundUser)
        {
            if (request.ChangeRoleDto.Role.ToLower() == "admin" || request.ChangeRoleDto.Role == "superadmin")
            {
                return OperationResult<User>.Fail("You do not have permission to change superadmins role", "ChangeRoleCommandHandler");
            }

            if (request.ChangeRoleDto.Role.ToLower() == "user")
            {
                var grantAdminRole = await _userManager.AddToRoleAsync(foundUser, request.ChangeRoleDto.Role);
                return OperationResult<User>.Success(foundUser);
            }

            return OperationResult<User>.Fail("Role not found", "ChangeRoleCommandHandler");
        }
    }
}
