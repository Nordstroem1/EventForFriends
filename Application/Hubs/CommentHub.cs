using Application.Commands.CommentCommands.CreateComment;
using Application.Commands.CommentCommands.DeleteComment;
using Application.Commands.CommentCommands.UpdateComment;
using Application.Dtos.Comment;
using Application.Queries.CommentQueries.GetCommentById;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public class CommentHub : Hub
    {
        private readonly IMediator _mediator;
        public CommentHub(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task CreateComment(CreateCommentDto commentdto,Guid UserId)
        {
            var result = await _mediator.Send(new CreateComment(commentdto, UserId));
            if (result.Succeeded)
            {
                await Clients.All.SendAsync("CommentCreated", result.Data);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }

        public async Task GetCommentById(Guid commentId)
        {
            if (commentId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("Error", "Invalid commentDto commentID");
                return;
            }

            var result = await _mediator.Send(new GetCommentByIdQuery(commentId));
            if (result.Succeeded)
            {
                await Clients.Caller.SendAsync("ReceiveComment", result.Data);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }
        public async Task UpdateComment(UpdateCommentDto commentDto,Guid commentId) 
        { 
            var result = await _mediator.Send(new UpdateCommentCommand(commentId, commentDto));
            if (result.Succeeded)
            {
                await Clients.All.SendAsync("CommentUpdated", result.Data);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }

        public async Task DeleteComment(Guid commentID)
        {
            var result = await _mediator.Send(new DeleteCommentCommand(commentID));
            if (result.Succeeded)
            {
                await Clients.All.SendAsync("CommentDeleted", commentID);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }
    }
}
