using Application.Commands.EventCommands.CreateEvent;
using Application.Commands.EventCommands.DeleteEvent;
using Application.Commands.EventCommands.LikeEvent;
using Application.Commands.EventCommands.LikesEvent;
using Application.Commands.EventCommands.UpdateEvent;
using Application.Dtos.Event;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventController> _logger;
        private readonly UserManager<User> _userManager;
        public EventController(IMediator mediator, ILogger<EventController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _mediator.Send(new CreateEventCommand(eventDto, userId));

                if (!result.Succeeded)
                {
                    return BadRequest(OperationResult<Event>.Fail(result.ErrorMessage, result.FailLocation));
                }

                return Ok(OperationResult<Event>.Success(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEvent Threw an exeption.");
                return BadRequest(OperationResult<Event>.Fail("Could not create event.", "EventController"));
            }
        }

        [HttpPut("updateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventDto updateEventDto, string oldEventId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(OperationResult<User>.Fail("User is not logged in.", "EventController"));
                }

                var logedInUser = await _userManager.FindByIdAsync(userId);
                if (logedInUser == null)
                {
                    return Unauthorized(OperationResult<User>.Fail("Could not find user.", "EventController"));
                }

                var result = await _mediator.Send(new UpdateEventCommand(logedInUser.Id, oldEventId, updateEventDto));
                if (!result.Succeeded)
                {
                    return BadRequest(OperationResult<Event>.Fail("Could Not create user.", "Controller"));
                }

                return Ok(OperationResult<UpdateEventDto>.Success(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEvent Threw an exeption.");
                return BadRequest(OperationResult<Event>.Fail("Could Not create user.", "Controller"));
            }
        }

        [HttpDelete("deleteEvent")]
        public async Task<IActionResult> DeleteEvent([FromBody] string EventId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _mediator.Send(new DeleteEventCommand(EventId));
                if (!result.Succeeded || result.Data == Guid.Empty)
                {
                    return BadRequest(OperationResult<Guid>.Fail("Could Not create user.", "Controller"));
                }

                return Ok(OperationResult<Guid>.Success(result.Data));
            }
            catch (Exception ex) 
            {
                _logger.LogError("Unexpected error");
                return BadRequest(OperationResult<User>.Fail("Unexpected error","Controller"));
            }
        }

        [HttpPost("likeEvent")]
        public async Task<IActionResult> LikeEvent([FromBody]EventLikes likeEventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var result = await _mediator.Send(new LikeEventCommand(likeEventDto));

            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}