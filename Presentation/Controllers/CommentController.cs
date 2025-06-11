using Application.Commands.CommentCommands.CreateComment;
using Application.Dtos.Comment;
using Application.Hubs;
using Application.Interfaces;
using Application.Queries.CommentQueries.GetAllComments;
using Application.Queries.CommentQueries.GetCommentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = "ApplicationToken")]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly Mediator _mediator;
        private readonly IGetUser _getUser;
        private readonly IHubContext<CommentHub> _hubContext;
        public CommentController(IHubContext<CommentHub> hubContext, IGetUser getUser, ILogger<CommentController> logger, Mediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _getUser = getUser;
            _hubContext = hubContext;
        }

        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto comment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = _getUser.GetUserIdFromClaims(User);

                if (userId == null)
                {
                    _logger.LogError("User not found");
                    return BadRequest("User not found");
                }

                var result = await _mediator.Send(new CreateCommentCommand(comment, userId.Data));
                //signalR things??
                if (result == null || !result.Succeeded)
                {
                    _logger.LogError("Failed to create comment");
                    return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage });
                }


                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateComment Threw an exeption.");
                return BadRequest("Failed to create comment");
            }
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpGet("getbyId")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            if (id == string.Empty)
            {
                _logger.LogError("Invalid comment id");
                return BadRequest("Invalid comment id");
            }
            try
            {
                var result = await _mediator.Send(new GetCommentByIdQuery(id));
                if (result == null || !result.Succeeded)
                {
                    _logger.LogError("Failed to get comment");
                    return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage});
                }
                
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCommentById Threw an exeption.");
                return BadRequest("Failed to get comment");
            }
        }

        [Authorize(Roles = "admin,superadmin,user")]
        [HttpGet("{EventId}")]
        public async Task<IActionResult> GetAllComments(string EventId)
        {
            try
            {
                var result = await _mediator.Send(new GetAllCommentsQuery(EventId));
                if (result == null || !result.Succeeded)
                {
                    _logger.LogError("Failed to get comment");
                    return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCommentById Threw an exeption.");
                return BadRequest("Failed to get comment");
            }
        }
    }
}
