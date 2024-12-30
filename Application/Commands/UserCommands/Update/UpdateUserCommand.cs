using Application.Dtos.User;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Update
{
    public class UpdateUserCommand : IRequest<OperationResult<User>>
    {
        public Guid UserId { get; set; }
        public UpdateUserDto UpdatedUser { get; set; }

        public UpdateUserCommand(Guid userId, UpdateUserDto updatedUser)
        {
            UserId = userId;
            UpdatedUser = updatedUser;
        }
    }
}
