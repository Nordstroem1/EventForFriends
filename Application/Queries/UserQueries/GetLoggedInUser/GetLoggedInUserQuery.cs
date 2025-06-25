using Application.Dtos.User;
using Domain.Models;
using MediatR;

namespace Application.Queries.UserQueries.GetLoggedInUser
{
    public sealed record GetLoggedInUserQuery(string UserId) : IRequest<OperationResult<LoggedInUserDto>>;
}