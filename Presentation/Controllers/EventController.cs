using Application.Dtos;
using Azure;
using Domain.Models;
using MediatR;
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
        public EventController(IMediator mediator, ILogger<EventController> logger, UserManager<User> _userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = _userManager;
        }

        [HttpPost("createEvent")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var logedInUser = _userManager.FindByNameAsync(ClaimTypes.NameIdentifier);
                
                if (logedInUser.Id == null)
                {
                    return Unauthorized(OperationResult<User>.Fail("User is not logged in.","EventController"));
                }
                else if(logedInUser == null)
                {
                    return Unauthorized(OperationResult<User>.Fail("Could not find user.", "EventController"));
                }

                var result = await _mediator.Send(new CreateEventCommand(eventDto, logedInUser.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEvent Threw an exeption.");
                return BadRequest(OperationResult<Event>.Fail("Could Not create user.","Controller"));
            }
        }
    }
}