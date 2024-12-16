using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        public UpdateUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<OperationResult<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            if (request.UpdatedUser == null)
            {
                return OperationResult<User>.Fail("User is null", "Application");
            }

            var foundUser = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (foundUser == null)
            {
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
                return OperationResult<User>.Fail("Failed to update user", "Application");
            }

            return OperationResult<User>.Success(foundUser);
        }
    }
}
