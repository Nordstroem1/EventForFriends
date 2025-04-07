using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.DeleteComment
{
    public sealed record DeleteCommentCommand(string CommentId) : IRequest<OperationResult<string>>;
}
