using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Application.Token;
using Application.Interfaces;

namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationDependencyInjection).Assembly;

            services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
            services.AddScoped<TokenHelper>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

    }
}
