using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.Update
{
    public class UpdateUserCommand : IRequest<OperationResult<User>>
    {
        public Guid UserId { get; set; }
        public CreateUserDto UpdatedUser { get; set; }

        public UpdateUserCommand(Guid userId, CreateUserDto updatedUser)
        {
            UserId = userId;
            UpdatedUser = updatedUser;
        }
    }
}
