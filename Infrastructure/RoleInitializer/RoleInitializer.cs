using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.RoleInitializer
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(RoleEnums.Roles)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName});
                }
            }
        }
    }
}