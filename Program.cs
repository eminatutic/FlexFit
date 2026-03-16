using System.Text;
using FlexFit.Data;
using FlexFit.Middleware;
using FlexFit.MongoModels.Repositories;
using FlexFit.Repositoires;
using FlexFit.Repositoires.Interfaces;
using FlexFit.Repositories;
using FlexFit.Repositories.Interfaces;
using FlexFit.Token;
using FlexFit.UnitOfWorkLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FlexFit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // PostgreSQL
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repozitorijumi
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IFitnessObjectRepository, FitnessObjectRepository>();
            builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IPenaltyCardRepository, PenaltyCardRepository>();
            builder.Services.AddScoped<IPenaltyPointRepository, PenaltyPointRepository>();
            builder.Services.AddScoped<IMembershipCardRepository, MembershipCardRepository>();

            // UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MongoDB
            builder.Services.AddSingleton<MongoDbContext>();
            builder.Services.AddScoped<EntryLogRepository>();
            builder.Services.AddScoped<IncidentRepository>();
            builder.Services.AddScoped<RateLimitViolationRepository>();

            // JWT Token
            builder.Services.AddScoped<ITokenService, TokenService>();

            // Controllers
            builder.Services.AddControllers();

            // Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Middleware
            app.UseRouting();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<SmartThrottlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            app.Run();
        }
    }
}