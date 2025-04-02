using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Databases;
using Infrastructure.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.DepencyInjection
{
    public static class DepencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, string connectionString, IConfiguration configuration)
        {
            services.AddDbContext<mySqlDb>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secret"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("superAdmin", policy =>
                {
                    policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("superadmin");
                });
                options.AddPolicy("admin", policy =>
                {
                    policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("admin");
                });
                options.AddPolicy("user", policy =>
                {
                    policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("user");
                });
            });

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddSingleton<RoleSeeder>();

            return services;
        }
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "superadmin", "admin", "user" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
