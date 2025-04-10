using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ILogger<PermissionChecker> _logger;

        public PermissionChecker(ILogger<PermissionChecker> logger)
        {
            _logger = logger;
        }

        public bool HasPermissionToModifyAsync(User user, Event eventEntity)
        {
            if (user.Id == eventEntity.CreatedBy ||
                user.Role.ToLower() == "superadmin" ||
                user.Role.ToLower() == "admin")
            {
                return true;
            }

            _logger.LogError("User does not have permission to modify this event");
            return false;
        }
    }
}
