using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        public CreateUserCommandHandler()
        {
        }
        public Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User CreatedUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserDto.UserName,
                CreatedAt = DateTime.Now,
                Email = request.UserDto.Email,
                PasswordHash = request.UserDto.Password
            };
            return Task.FromResult(CreatedUser);
        }
    }
}
