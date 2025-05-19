using Application.Interfaces;
using Application.Services;
using Application.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services,IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddAutoMapper(assembly);
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
            services.AddScoped<TokenHelper>();
            services.AddScoped<IGetUser, GetUser>();
            services.AddScoped<IPermissionChecker, PermissionChecker>();
            services.AddSignalR();

            return services;
        }
    }
}
