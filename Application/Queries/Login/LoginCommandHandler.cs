using Application.Token;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Queries.Login
{
    public class LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, TokenHelper tokenHelper) : IRequestHandler<LoginCommand, OperationResult<string>>
    {
        public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var foundUser = await LoginWithUsernameOrEmail(request);

                if(foundUser == null)
                {
                    return OperationResult<string>.Fail("User not found", "LoginCommandHandler");
                }

                var authenticationResult = tokenHelper.AuthenticateUser(foundUser.Id);

                if (!authenticationResult.IsCompletedSuccessfully)
                {
                    return OperationResult<string>.Fail("User not found", "LoginCommandHandler");
                }

                if (await IsUserLockedOut(foundUser))
                {
                    return OperationResult<string>.Fail("User is locked out", "LoginCommandHandler");
                }

                var result = await signInManager.CheckPasswordSignInAsync(foundUser, request.LoginDto.Password, true);

                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        return OperationResult<string>.Fail("User is locked out due to too many failed attempts", "LoginCommandHandler");
                    }

                    return OperationResult<string>.Fail("Invalid password", "LoginCommandHandler");
                }

                await userManager.ResetAccessFailedCountAsync(foundUser);
                var token = await tokenHelper.GenerateToken(foundUser);

                return OperationResult<string>.Success(token);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail(ex.Message, "LoginCommandHandler");
            }
        }

        private async Task<User> LoginWithUsernameOrEmail(LoginCommand request)
        {
            User foundUser = null;

            if (request.LoginDto.Email != null)
            {
                foundUser = await userManager.FindByEmailAsync(request.LoginDto.Email);
            }
            else if (request.LoginDto.UserName != null)
            {
                foundUser = await userManager.FindByNameAsync(request.LoginDto.UserName);
            }

            return foundUser;
        }
        public async Task<bool> IsUserLockedOut(User user)
        {
            if (await userManager.IsLockedOutAsync(user))
            {
                return true;
            }
            return false;
        }
    }
}
