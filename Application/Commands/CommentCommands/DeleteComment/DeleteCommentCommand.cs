using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<OperationResult<string>>
    {
        public string CommentId { get; set; }
        public DeleteCommentCommand(string commentId)
        {
            CommentId = commentId;
        }
    }
}
