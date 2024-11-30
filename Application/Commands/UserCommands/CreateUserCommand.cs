using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands
{
    public class CreateUserCommand : IRequest<User>
    {
        public CreateUserCommand(CreateUserDto userDto)
        {
            UserDto = userDto;
        }
        public CreateUserDto UserDto { get;}
    }
}
