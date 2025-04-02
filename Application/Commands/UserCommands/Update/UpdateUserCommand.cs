using Application.Dtos.User;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Update
{
    public sealed record UpdateUserCommand(string LoggedInUser,string UserId, UpdateUserDto UpdatedUser) : IRequest<OperationResult<User>>;
}
