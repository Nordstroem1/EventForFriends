using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Commands.UserCommands;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator; 
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdUser = await _mediator.Send(new CreateUserCommand(user));

                if(createdUser == null)
                {
                    _logger.LogError("Failed to create user");
                    return BadRequest("Failed to create user");
                }

                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id}, createdUser);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "CreateUser Threw an exeption.");
                return BadRequest("Failed to create user");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok();
        }
    }
}
