using Application.Dtos.User;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.ChangeRole
{
    public class ChangeRoleCommand : IRequest<OperationResult<User>>
    {
        public ChangeRoleDto ChangeRoleDto { get; set; }
        public ChangeRoleCommand(ChangeRoleDto changeRoleDto)
        {
            ChangeRoleDto = changeRoleDto;
        }
    }
}
