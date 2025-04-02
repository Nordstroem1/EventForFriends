using Domain.Models;
using MediatR;

namespace Application.Queries.CommentQueries.GetCommentById
{
    public class GetCommentByIdQuery : IRequest<OperationResult<Comment>>
    {
        public string CommentId { get; set; }
        public GetCommentByIdQuery(string commentId)
        {
            CommentId = commentId;
        }
    }
}
