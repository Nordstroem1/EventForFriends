using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<string>>
    {
        private readonly UserManager<User> _userManager;
        public DeleteUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<OperationResult<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(request.UserId == Guid.Empty)
            {
                return OperationResult<string>.Fail("User ID is empty", "Application");
            }
            
            User foundUser = await _userManager.FindByIdAsync(request.UserId.ToString());

            if(foundUser == null)
            {
                return OperationResult<string>.Fail("User not found", "Application");
            }

            var result = await _userManager.DeleteAsync(foundUser);
            if (!result.Succeeded)
            {
                return OperationResult<string>.Fail("Failed to delete user", "Application");
            }
            return OperationResult<string>.Success("Successfully deleted user.");
        }
    }
}
