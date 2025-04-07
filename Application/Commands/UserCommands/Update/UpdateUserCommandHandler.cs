using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCommands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UpdateUserCommandHandler> logger, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
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
                var loggedInUser = await _userManager.FindByIdAsync(request.LoggedInUser);
                var foundUser = await _userManager.FindByIdAsync(request.UserId.ToString());

                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<User>.Fail("User not found", "Application");
                }
                if(loggedInUser == null)
                {
                    _logger.LogError("Logged in user not found");
                    return OperationResult<User>.Fail("Logged in user not found", "Application");
                }

                _mapper.Map(request.UpdatedUser, foundUser);

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
