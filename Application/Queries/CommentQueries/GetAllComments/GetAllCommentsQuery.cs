using Domain.Models;
using MediatR;

namespace Application.Queries.CommentQueries.GetAllComments
{
    public class GetAllCommentsQuery : IRequest<OperationResult<List<Comment>>>
    {
        public Guid EventId { get; set; }
        public GetAllCommentsQuery(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
