using Microsoft.Extensions.DependencyInjection;
using Application.Token;
using Domain.Interfaces;
using Infrastructure.Data;
using Domain.Models;
using Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services,IConfiguration configuration)
        {
            var assembly = typeof(ApplicationDependencyInjection).Assembly;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdmin", policy =>
                {
                    policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("SuperAdmin");
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin");
                });
                options.AddPolicy("User", policy =>
                {
                    policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("User");
                });
            });

            services.AddDbContext<mySqlDb>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
            services.AddScoped<TokenHelper>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddSignalR();

            return services;
        }
    }
}
