using Domain.Models;
using MediatR;

namespace Application.Queries.CommentQueries.GetAllComments
{
    public class GetAllCommentsQuery : IRequest<OperationResult<List<Comment>>>
    {
        public string EventId { get; set; }
        public GetAllCommentsQuery(string eventId)
        {
            EventId = eventId;
        }
    }
}
