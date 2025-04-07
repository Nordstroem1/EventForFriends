using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Commands.UserCommands.Create;
using Application.Commands.UserCommands.Delete;
using Application.Commands.UserCommands.Update;
using Application.Queries.UserQueries.GetUserById;
using Application.Queries.Login;
using Application.Dtos.User;
using Application.Queries.UserQueries.GetAllUsers;
using Application.Commands.UserCommands.ChangeRole;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;

namespace Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = "ApplicationToken")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        private readonly IGetUser _getUserService;

        public UserController(IMediator mediator, ILogger<UserController> logger, IGetUser getUserService)
        {
            _logger = logger;
            _mediator = mediator;
            _getUserService = getUserService;
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _mediator.Send(new CreateUserCommand(user));

                if (result == null || !result.Succeeded)
                {
                    _logger.LogError("Failed to create user");
                    return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
                }

                return CreatedAtAction(nameof(GetUserById), new { id = result }, result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUser Threw an exeption.");
                return BadRequest("Failed to create user");
            }
        }

        [Authorize(Roles = "user,admin,superadmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogError("Invalid user id");
                    return BadRequest("Invalid user id");
                }

                var loggedInUser = _getUserService.GetUserIdFromClaims(User);

                var result = await _mediator.Send(new DeleteUserCommand(loggedInUser.Data,id));

                if (result == null || !result.Succeeded)
                {
                    _logger.LogError("Failed to delete user");
                    return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
                }

                return Ok(result.Data);
            }
            catch
            {
                return BadRequest("Something went wrong while deleting the user.");
            }
        }

        [Authorize(Roles = "user,admin,superadmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id,[FromBody] UpdateUserDto updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = _getUserService.GetUserIdFromClaims(User);


            var result = await _mediator.Send(new UpdateUserCommand(loggedInUserId.Data, id, updatedUser));

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
            }

            return Ok(new { result.Succeeded, result.Data });
        }

        [Authorize(Roles = "user,admin,superadmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
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

        [Authorize(Roles = "user,admin,superadmin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUserQuery());

            if (result == null || !result.Succeeded)
            {
                _logger.LogError("Failed to get users");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
            }   

            return Ok(new { result.Succeeded, result.Data });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(new LoginCommand(loginDto));
            
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to login");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "admin,superadmin")]
        [HttpPut("ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleDto changeRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUserId = _getUserService.GetUserIdFromClaims(User);

            var result = await _mediator.Send(new ChangeRoleCommand(loggedInUserId.Data, changeRoleDto));
            
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to change role");
                return BadRequest(new { result.FailLocation, result.Data, result.ErrorMessage, result.Succeeded });
            }

            return Ok(new { result.Succeeded, result.Data });
        }
    }
}
