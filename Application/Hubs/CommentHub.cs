using Application.Commands.CommentCommands.CreateComment;
using Application.Commands.CommentCommands.DeleteComment;
using Application.Commands.CommentCommands.UpdateComment;
using Application.Dtos.Comment;
using Application.Queries.CommentQueries.GetCommentById;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Hubs
{
    public class CommentHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CommentHub> _logger;
        public CommentHub(IMediator mediator, UserManager<User> userManager, ILogger<CommentHub> logger)
        {
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task CreateComment(CreateCommentDto commentdto,Guid UserId)
        {
            await FindUser();

            var result = await _mediator.Send(new CreateComment(commentdto, UserId));
            
            if (result.Succeeded)
            {
                await Clients.Group(commentdto.EventId.ToString()).SendAsync("CommentCreated", result.Data);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }
       
        public async Task UpdateComment(UpdateCommentDto commentDto,Guid commentId) 
        {
            await FindUser();

            var result = await _mediator.Send(new UpdateCommentCommand(commentId, commentDto));
            if (result.Succeeded)
            {
                await Clients.Group(commentDto.EventId.ToString()).SendAsync("CommentUpdated", result.Data);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }

        public async Task DeleteComment(Guid commentId)
        {
            var user = await FindUser();

            var isAdmin = Context.User?.IsInRole("Admin");

            if (isAdmin == false)
            {
                await Clients.Caller.SendAsync("Error", "You do not have permission to delete this comment.");
                return;
            }

            var foundComment = await _mediator.Send(new GetCommentByIdQuery(commentId));
            if (foundComment == null)
            {
                await Clients.Caller.SendAsync("Error", "Comment not found.");
                return;
            }

            var result = await _mediator.Send(new DeleteCommentCommand(commentId));

            if (result.Succeeded)
            {
                await Clients.Group(commentId.ToString()).SendAsync("CommentDeleted", commentId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }

        private async Task<Claim> FindUser()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                await Clients.Caller.SendAsync("Error", "User not authenticated.");
                return null;
            }
            return userId;
        }
    }
}
