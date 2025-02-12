using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Databases;
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

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();

            return services;
        }
    }
}
