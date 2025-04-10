using Application.Interfaces;
using Domain.Models;
using System.Security.Claims;

namespace Application.Services
{
    public class GetUser : IGetUser
    {
        public OperationResult<string> GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return OperationResult<string>.Fail("User ID claim not found.", "GetUser");
            }
            return OperationResult<string>.Success(userIdClaim.Value);
        }
    }
}
