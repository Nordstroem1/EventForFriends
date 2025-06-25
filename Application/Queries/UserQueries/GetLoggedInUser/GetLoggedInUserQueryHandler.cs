using Application.Dtos.User;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Queries.UserQueries.GetLoggedInUser
{
    public class GetLoggedInUserQueryHandler(UserManager<User> userManager, ILogger<GetLoggedInUserQueryHandler> logger) : IRequestHandler<GetLoggedInUserQuery, OperationResult<LoggedInUserDto>>
    {
        public async Task<OperationResult<LoggedInUserDto>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var loggedInUser = await userManager.FindByIdAsync(request.UserId);

                if (loggedInUser == null)
                {
                    logger.LogError("User not found with ID: {UserId}", request.UserId);
                    return OperationResult<LoggedInUserDto>.Fail("User not found", "GetLoggedInUserQueryHandler");
                }

                var user = new LoggedInUserDto
                {
                    UserId = loggedInUser.Id,
                    UserName = loggedInUser.UserName,
                    ProfilePicture = loggedInUser.ProfilePicture,
                    Longitude = loggedInUser.Longitude.ToString(),
                    Latitude = loggedInUser.Latitude.ToString() 
                };

                return OperationResult<LoggedInUserDto>.Success(user);
            }
            catch
            {
                return OperationResult<LoggedInUserDto>.Fail("An error occurred while retrieving the logged-in user", "GetLoggedInUserQueryHandler");
            }
        }
    }
}
