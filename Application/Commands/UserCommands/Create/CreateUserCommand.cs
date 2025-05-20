using Application.Dtos.User;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.UserCommands.Create
{
    public sealed record CreateUserCommand(CreateUserDto UserDto, IFormFile ProfilePicture) : IRequest<OperationResult<User>>;
}
