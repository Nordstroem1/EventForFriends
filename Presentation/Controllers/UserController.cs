using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Commands.UserCommands;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _mediator.Send(new CreateUserCommand(user));

            if(createdUser == null)
            {
                return BadRequest("Failed to create user");
            }

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id}, createdUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return Ok();
        }
    }
}
