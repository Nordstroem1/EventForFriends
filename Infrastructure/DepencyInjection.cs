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

namespace Infrastructure
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
            var secretKey = jwtSettings["secret"]!;

            services.AddAuthentication("ApplicationToken")
            .AddJwtBearer("ApplicationToken", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["issuer"],
                    ValidAudience = jwtSettings["audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireRole("user").RequireAuthenticatedUser().AddAuthenticationSchemes("ApplicationToken"));
                options.AddPolicy("admin", policy => policy.RequireRole("admin").RequireAuthenticatedUser().AddAuthenticationSchemes("ApplicationToken"));
                options.AddPolicy("superadmin", policy => policy.RequireRole("superadmin").RequireAuthenticatedUser().AddAuthenticationSchemes("ApplicationToken"));
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
