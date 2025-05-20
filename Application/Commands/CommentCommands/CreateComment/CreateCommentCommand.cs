using Application.Dtos.Comment;
using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.CreateComment
{
    public sealed record CreateCommentCommand(CreateCommentDto CommentDto, string UserId) : IRequest<OperationResult<Comment>>;
}
