using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Databases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DepencyInjection
{
    public static class DepencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<mySqlDb>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<mySqlDb>()
                .AddDefaultTokenProviders();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();

            return services;
        }
    }
}
