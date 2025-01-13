using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<OperationResult<Guid>>
    {
        public Guid CommentId { get; set; }
        public DeleteCommentCommand(Guid commentId)
        {
            CommentId = commentId;
        }
    }
}
