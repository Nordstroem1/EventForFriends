using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Application.Commands.UserCommands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreateUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<CreateUserCommandHandler> logger, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<OperationResult<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdUser = _mapper.Map<User>(request.UserDto);

                if(!await _roleManager.RoleExistsAsync("User"))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(createdUser.Role));

                    if (!roleResult.Succeeded)
                    {
                        _logger.LogError($"Error when creating a user role: ");

                        return OperationResult<User>.Fail($"Failed to create user role: ", "Application");
                    }
                }
                var userResult = await _userManager.CreateAsync(createdUser, request.UserDto.Password);
                await _userManager.AddToRoleAsync(createdUser, createdUser.Role);

                if (!userResult.Succeeded)
                {
                    var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                    _logger.LogError($"Error when creating a user: {errors}");

                    return OperationResult<User>.Fail($"Failed to create user: {errors}", "Application");
                }

                return OperationResult<User>.Success(createdUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error." + ex.Message);
            }
        }
    }
}
