using Application.Dtos.User;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Create
{
    public sealed record CreateUserCommand(CreateUserDto UserDto) : IRequest<OperationResult<User>>;
}
