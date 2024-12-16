using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Delete
{
    public class DeleteUserCommand : IRequest<OperationResult<string>>
    {
        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; set; }
    }
}
