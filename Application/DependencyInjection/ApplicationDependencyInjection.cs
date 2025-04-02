using Microsoft.Extensions.DependencyInjection;
using Application.Token;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;
using Application.Services;

namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services,IConfiguration configuration)
        {
            var assembly = typeof(ApplicationDependencyInjection).Assembly;
            services.AddAutoMapper(assembly);
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
            services.AddScoped<TokenHelper>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IGetUser, GetUser>();
            services.AddSignalR();
            return services;
        }
    }
}
