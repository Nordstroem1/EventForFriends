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
                var createdUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserDto.UserName,
                    Email = request.UserDto.Email,
                    PhoneNumber = request.UserDto.PhoneNumber,
                    PasswordHash = request.UserDto.Password,
                    CreatedAt = DateTime.UtcNow,
                    Role = RoleEnums.Roles.user,
                    Comments = new List<Comment>(),
                    Events = new List<Event>()
                };

                var userPassword = await _userManager.CreateAsync(createdUser, request.UserDto.Password);

                if (!userPassword.Succeeded)
                {
                    var errors = string.Join(", ", userPassword.Errors.Select(e => e.Description));
                    _logger.LogError($"Error when creating a user: {errors}");

                    return OperationResult<User>.Fail($"Failed to create user: {errors}", "Application");
                }

                var roleResult = await _userManager.AddToRoleAsync(createdUser, createdUser.Role.ToString());

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", userPassword.Errors.Select(e => e.Description));
                    _logger.LogError($"Error when creating a user: {errors}");

                    return OperationResult<User>.Fail($"Failed to create user: {errors}", "Application");
                }
                _logger.LogInformation("User added successfully.");

                return OperationResult<User>.Success(createdUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error." + ex.Message);
            }
        }
    }
}
