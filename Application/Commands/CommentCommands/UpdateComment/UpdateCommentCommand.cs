using Application.Dtos.Comment;
using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.UpdateComment
{
    public sealed record UpdateCommentCommand(string UserId,string CommentId, UpdateCommentDto UpdateCommentDto) : IRequest<OperationResult<Comment>>;
}