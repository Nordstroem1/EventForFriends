using Application.Commands.EventCommands.CreateEvent;
using Application.Commands.EventCommands.DeleteEvent;
using Application.Commands.EventCommands.LikesEvent;
using Application.Commands.EventCommands.UpdateEvent;
using Application.Dtos.Event;
using Application.Interfaces;
using Application.Queries.EventQueries.GetAllEvents;
using Application.Queries.EventQueries.GetEventById;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = "ApplicationToken")]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IGetUser _getUserService;

        public EventController(IMediator mediator, IGetUser getUserService, ILogger<EventController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _getUserService = getUserService;
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto eventDto, IFormFile? ImageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdResponse = _getUserService.GetUserIdFromClaims(User);

                if (!userIdResponse.Succeeded)
                {
                    return Unauthorized(OperationResult<User>.Fail($"{"User is not logged in. "} {userIdResponse.ErrorMessage}", "EventController"));
                }

                var result = await _mediator.Send(new CreateEventCommand(userIdResponse.Data,eventDto, ImageFile));

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

                var result = await _mediator.Send(new UpdateEventCommand(userId, oldEventId, updateEventDto));

                if (!result.Succeeded)
                {
                    return BadRequest(OperationResult<Event>.Fail("Could Not create user.", "Controller"));
                }

                return Ok(OperationResult<Event>.Success(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEvent Threw an exeption.");
                return BadRequest(OperationResult<Event>.Fail("Could not update event.", "Controller"));
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

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(OperationResult<User>.Fail("User is not logged in.", "EventController"));
                }

                var result = await _mediator.Send(new DeleteEventCommand(userId, EventId));
                if (!result.Succeeded || !result.Data)
                {
                    return BadRequest(OperationResult<Guid>.Fail("Could not delete event.", "Controller"));
                }

                return Ok(OperationResult<bool>.Success(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error");
                return BadRequest(OperationResult<User>.Fail("Unexpected error", "Controller"));
            }
        }

        [HttpPost("likeEvent")]
        public async Task<IActionResult> LikeEvent([FromBody] string eventId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(OperationResult<User>.Fail("User is not logged in.", "EventController"));
            }

            var result = await _mediator.Send(new LikeEventCommand(userId, eventId));

            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("getEventById")]
        public async Task<IActionResult> GetEventById(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return BadRequest("EventId can't be null");
            }

            var result = await _mediator.Send(new GetEventbyIdQuery(eventId));

            if (!result.Succeeded)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var result = await _mediator.Send(new GetAllEventsQuery());

                if (!result.Succeeded)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch
            {
                return BadRequest("Server Error");
            } 
        }

        [HttpGet("getAllWithinDistance")]
        public async Task<IActionResult> GetAllEventsBasedOnDistance(int distansBetweenEventAndUser)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId)) 
                    return BadRequest("Could not find logged in user.");

                var result = await _mediator.Send(new GetAllEventsWithinAreaQuery(distansBetweenEventAndUser,userId));

                if (!result.Succeeded)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest("Somewhing went wring while fetching events.");
            }
        }
    }
}