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
        private readonly ILogger<CommentHub> _logger;
        private readonly UserManager<User> _userManager;   
        public CommentHub(IMediator mediator, ILogger<CommentHub> logger, UserManager<User> userManager)
        {
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task CreateComment(CreateCommentDto commentdto,Guid UserId)
        {
            var user = await FindUser();
            var result = await _mediator.Send(new CreateComment(commentdto, UserId));
            
            if (result.Succeeded)
            {
                await Clients.Group(commentdto.EventId.ToString()).SendAsync("CommentCreated", result.Data);
            }
            else
            {
                _logger.LogError(result.ErrorMessage);
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }
       
        public async Task UpdateComment(UpdateCommentDto commentDto,Guid commentId) 
        {
            var user = await FindUser();

            var result = await _mediator.Send(new UpdateCommentCommand(commentId, commentDto));
            if (result.Succeeded)
            {
                await Clients.Group(commentDto.EventId.ToString()).SendAsync("CommentUpdated", result.Data);
            }
            else
            {
                _logger.LogError(result.ErrorMessage);
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }

        public async Task DeleteComment(Guid commentId)
        {
            var user = await FindUser();


            var foundComment = await _mediator.Send(new GetCommentByIdQuery(commentId));
            if(user.Role != "Admin"|| foundComment.Data.UserId == user.Id)
            {
                await Clients.Caller.SendAsync("Error", "You do not have permission to delete this comment.");
                return;
            }
            if (foundComment == null)
            {
                _logger.LogError(foundComment.ErrorMessage);
                await Clients.Caller.SendAsync("Error", "Comment not found.");
                return;
            }
            if (user.Role != "Admin" && foundComment.Data.UserId != user.Id)
            {
                await Clients.Caller.SendAsync("Error", "You do not have permission to delete this comment.");
                return; // Stop execution if user is not an admin or the owner of the comment
            }

            var result = await _mediator.Send(new DeleteCommentCommand(commentId));
            if (result.Succeeded)
            {
                await Clients.Group(foundComment.Data.EventId.ToString()).SendAsync("CommentDeleted", commentId);
            }
            else
            {
                _logger.LogError(result.ErrorMessage);  
                await Clients.Caller.SendAsync("Error", result.ErrorMessage);
            }
        }

        private async Task<User> FindUser()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                await Clients.Caller.SendAsync("Error", "User not authenticated.");
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId.Value);
            if (user == null)
            {
                await Clients.Caller.SendAsync("Error", "User not found.");
                return null;
            }

            return user;
        }
    }
}
