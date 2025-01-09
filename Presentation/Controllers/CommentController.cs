using Application.Hubs;
using Application.Queries.CommentQueries.GetAllComments;
using Application.Queries.CommentQueries.GetCommentById;
using Application.Queries.EventQueries.GetAllEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly Mediator _mediator;

        public CommentController(ILogger<CommentController> logger, Mediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet("getbyId")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            if (id == Guid.Empty)
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllComments()
        {
            try
            {
                var result = await _mediator.Send(new GetAllEventsQuery());
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
