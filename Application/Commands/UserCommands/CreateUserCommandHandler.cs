using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.UserCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly UserManager<User> _userManager;
        public CreateUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User CreatedUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserDto.UserName,
                PhoneNumber = request.UserDto.PhoneNumber.ToString(),
                Email = request.UserDto.Email,
                CreatedAt = DateTime.Now,
                Events = new List<Event>(),
                Messages = new List<Message>()
            };

            var result = await _userManager.CreateAsync(CreatedUser, request.UserDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }

            var roleResult = await _userManager.AddToRoleAsync(CreatedUser, request.UserDto.Role.ToString());
            
            if (!roleResult.Succeeded)
            {
                throw new Exception("Role creation failed");
            }

            return CreatedUser;
        }
    }
}
