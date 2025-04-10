using Domain.Models;

namespace Application.Interfaces
{
    public interface IPermissionChecker
    {
        bool HasPermissionToModifyAsync(User user, Event eventEntity);
    }
}
