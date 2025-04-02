using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Delete
{
    public sealed record DeleteUserCommand(string loggedinUser ,string UserId) : IRequest<OperationResult<string>>;
}
