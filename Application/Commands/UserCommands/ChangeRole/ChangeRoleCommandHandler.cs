using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands.ChangeRole
{
    public class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ChangeRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<OperationResult<User>> Handle(ChangeRoleCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.ChangeRoleDto.UserId == Guid.Empty)
            {
                return OperationResult<User>.Fail("User ID is empty", "Application");
            }

            var foundUser = await _userManager.FindByIdAsync(request.ChangeRoleDto.UserId.ToString());
            if(foundUser == null)
            {
                return OperationResult<User>.Fail("User not found", "Application");
            }

            var foundRole = await _roleManager.RoleExistsAsync(request.ChangeRoleDto.Role.ToString());
            if(!foundRole)
            {
                return OperationResult<User>.Fail("Role not found", "Application");
            }

            var addRole = await _userManager.AddToRoleAsync(foundUser, request.ChangeRoleDto.Role.ToString());
            if (!addRole.Succeeded)
            {
                return OperationResult<User>.Fail("Failed to add role", "Application");
            }

            return OperationResult<User>.Success(foundUser);
        }
    }
}
