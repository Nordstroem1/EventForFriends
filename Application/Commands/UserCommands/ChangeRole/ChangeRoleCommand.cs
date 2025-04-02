using Application.Dtos.User;
using Domain.Models;
using MediatR;

namespace Application.Commands.UserCommands.ChangeRole
{
    public sealed record ChangeRoleCommand(string LoggedInUserId ,ChangeRoleDto ChangeRoleDto) : IRequest<OperationResult<User>>;
}
