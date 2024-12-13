using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Create
{
    public class CreateUserCommand : IRequest<OperationResult<User>>
    {
        public CreateUserCommand(CreateUserDto userDto)
        {
            UserDto = userDto;
        }
        public CreateUserDto UserDto { get; }
    }
}
