using Application.Dtos.Comment;
using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.CreateComment
{
    public sealed record CreateComment(CreateCommentDto CommentDto, string UserId) : IRequest<OperationResult<Comment>>;
}
