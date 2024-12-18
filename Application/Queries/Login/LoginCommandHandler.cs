using Application.Token;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Queries.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenHelper _tokenHelper;

        public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, TokenHelper tokenHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
        }
        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User foundUser = null;

            foundUser = await LoginWithUsernameOrEmail  (request, foundUser);

            if (foundUser == null)
            {
                return OperationResult<string>.Fail("User was null", "Application");
            }

            if (await IsUserLockedOut(foundUser))
            {
                return OperationResult<string>.Fail("User is locked out", "Application");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(foundUser, request.LoginDto.Password, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    return OperationResult<string>.Fail("User is locked out due to too many failed attempts", "Application");
                }

                return OperationResult<string>.Fail("Invalid password", "Application");
            }

            await _userManager.ResetAccessFailedCountAsync(foundUser);
            var token = _tokenHelper.GenerateToken(foundUser);

            return OperationResult<string>.Success(token);
        }

        private async Task<User> LoginWithUsernameOrEmail(LoginCommand request, User foundUser)
        {
            if (request.LoginDto.Email == null)
            {
                foundUser = await _userManager.FindByNameAsync(request.LoginDto.UserName);
            }
            else if (request.LoginDto.UserName == null)
            {
                foundUser = await _userManager.FindByEmailAsync(request.LoginDto.Email);
            }
            else
            {
                return null;
            }

            return foundUser;
        }
        public async Task<bool> IsUserLockedOut(User user)
        {
            if (await _userManager.IsLockedOutAsync(user))
            {
                return true;
            }
            return false;
        }
    }
}
