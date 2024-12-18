using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        public CreateUserCommandHandler(UserManager<User> userManager, ILogger<CreateUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<OperationResult<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User CreatedUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserDto.UserName,
                    PhoneNumber = request.UserDto.PhoneNumber,
                    Email = request.UserDto.Email,
                    Role = RoleEnums.Roles.user,
                    CreatedAt = DateTime.UtcNow,
                    Events = new List<Event>(),
                    Messages = new List<Message>()
                };

                var result = await _userManager.CreateAsync(CreatedUser, request.UserDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Error when creating a user: {errors}");

                    return OperationResult<User>.Fail($"Failed to create user: {errors}", "Application");
                }

                var roleResult = await _userManager.AddToRoleAsync(CreatedUser,CreatedUser.Role.ToString());

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError($"Error when creating a user: {errors}");

                    return OperationResult<User>.Fail($"Failed to create user: {errors}", "Application");
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
