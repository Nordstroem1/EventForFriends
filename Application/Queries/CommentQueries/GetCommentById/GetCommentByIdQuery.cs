using Domain.Models;
using MediatR;

namespace Application.Queries.CommentQueries.GetCommentById
{
    public class GetCommentByIdQuery : IRequest<OperationResult<Comment>>
    {
        public Guid CommentId { get; set; }
        public GetCommentByIdQuery(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
