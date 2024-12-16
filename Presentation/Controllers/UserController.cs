using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Commands.UserCommands.Create;
using Application.Commands.UserCommands.Delete;
using Application.Commands.UserCommands.Update;
using Application.Queries.GetUserById;

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
                var result = await _mediator.Send(new CreateUserCommand(user));

                if(result == null || !result.Succeeded)
                {
                    _logger.LogError("Failed to create user");
                    return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
                }

                return CreatedAtAction(nameof(GetUserById), new { id = result}, result.Data);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "CreateUser Threw an exeption.");
                return BadRequest("Failed to create user");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Invalid user id");
                return BadRequest("Invalid user id");
            }

            var result = await _mediator.Send(new DeleteUserCommand(id));

            if ( result == null || !result.Succeeded)
            {
                _logger.LogError("Failed to delete user");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded});
            }

            return Ok(new {result.Succeeded, result.Data});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new UpdateUserCommand(id, updatedUser));

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
            }

            return Ok(new { result.Succeeded, result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if(id == Guid.Empty)
            {
                _logger.LogError("Invalid user id");
                return BadRequest("Invalid user id");
            }

            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (result == null || !result.Succeeded)
            {
                _logger.LogError("Failed to get user");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
            }

            return Ok(new { result.Succeeded, result.Data });
        }
    }
}
