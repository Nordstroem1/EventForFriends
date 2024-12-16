using Domain.Models;
using MediatR;

namespace Application.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<OperationResult<User>>
    {
        public Guid UserId { get; set; }
        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
