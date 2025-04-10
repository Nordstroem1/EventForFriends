using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.DeleteComment
{
    public sealed record DeleteCommentCommand(string UserId,string CommentId) : IRequest<OperationResult<string>>;
}
