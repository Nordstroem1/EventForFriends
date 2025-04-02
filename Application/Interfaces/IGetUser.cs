using Domain.Models;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IGetUser 
    {
         OperationResult<string> GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
