using Application.Dtos.Comment;
using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.UpdateComment
{
    public class UpdateCommentCommand : IRequest<OperationResult<UpdateCommentDto>>
    {
        public string CommentId { get; set; }
        public UpdateCommentDto UpdateCommentDto { get; set; }
        public UpdateCommentCommand(string commentId, UpdateCommentDto updateCommentDto)
        {
            CommentId = commentId;
            UpdateCommentDto = updateCommentDto;
        }
    }
}
