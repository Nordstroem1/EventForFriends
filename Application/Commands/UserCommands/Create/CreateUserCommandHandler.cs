using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        public CreateUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<OperationResult<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User CreatedUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserDto.UserName,
                    PhoneNumber = request.UserDto.PhoneNumber.ToString(),
                    Email = request.UserDto.Email,
                    Role = RoleEnums.Roles.user,
                    CreatedAt = DateTime.UtcNow,
                    Events = new List<Event>(),
                    Messages = new List<Message>()
                };

                var result = await _userManager.CreateAsync(CreatedUser, request.UserDto.Password);

                if (!result.Succeeded)
                {
                    return OperationResult<User>.Fail("Failed to create user", "Application");
                }

                var roleResult = await _userManager.AddToRoleAsync(CreatedUser,CreatedUser.Role.ToString());

                if (!roleResult.Succeeded)
                {
                    return OperationResult<User>.Fail("Failed to add role", "Application");
                }

                return OperationResult<User>.Success(CreatedUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpeted error." + ex.Message);
            }
        }
    }
}
