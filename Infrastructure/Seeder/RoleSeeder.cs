using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeder
{
    public class RoleSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        public RoleSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SeedRoles()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DepencyInjection.SeedRoles(services);
            }
        }
    }
}
