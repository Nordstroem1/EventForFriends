using Application.Dtos.Comment;
using Domain.Models;
using MediatR;

namespace Application.Commands.CommentCommands.CreateComment
{
    public class CreateComment : IRequest<OperationResult<Comment>>
    {
        public CreateCommentDto CommentDto { get; set; }
        public string UserID { get; set; }
        public CreateComment(CreateCommentDto commentDto, string userID)
        {
            CommentDto = commentDto;
            UserID = userID;
        }
    }
}
