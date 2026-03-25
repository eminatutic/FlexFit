using System.Reflection;
using System.Text;
using FlexFit.Infrastructure.Data;
using FlexFit.Presentation.Middleware;
using FlexFit.Domain.MongoModels.Repositories;
using FlexFit.Infrastructure.Token;
using FlexFit.Infrastructure.UnitOfWorkLayer;
using FlexFit.Infrastructure.Repositories.Interfaces;
using FlexFit.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FlexFit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();

            // PostgreSQL - Povezivanje na bazu
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // MongoDB
            builder.Services.AddSingleton<MongoDbContext>();

            // Neo4j
            builder.Services.AddSingleton<Neo4jContext>();

            // Unit of Work + Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<EntryLogRepository>();
            builder.Services.AddScoped<LoginRepository>();
            builder.Services.AddScoped<IncidentRepository>();
            builder.Services.AddScoped<RateLimitViolationRepository>();
            builder.Services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
            builder.Services.AddScoped<ReservationLogRepository>();
            builder.Services.AddScoped<PenaltyLogRepository>();
            builder.Services.AddScoped<MembershipLogRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IPenaltyCardRepository, PenaltyCardRepository>();
            builder.Services.AddScoped<IPenaltyPointRepository, PenaltyPointRepository>();
            builder.Services.AddScoped<IMemberGraphRepository, MemberGraphRepository>();

            // Background Service za rezervacije
            builder.Services.AddHostedService<FlexFit.Application.Services.ReservationBackgroundService>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Token service
            builder.Services.AddScoped<ITokenService, TokenService>();

            // Authentication (JWT + Google)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            })
            .AddCookie()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
            });

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Swagger konfiguracija
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Unesi token u formatu: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // --- AUTOMATSKE MIGRACIJE PRI STARTU ---
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                    Console.WriteLine(">>> Postgres migracije su uspešno izvršene.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>> GREŠKA PRI MIGRACIJI: {ex.Message}");
                }
            }
            // ---------------------------------------

            // Global exception handler
            app.UseMiddleware<ExceptionMiddleware>();

            // Omogu?avamo Swagger uvek (i u produkciji) da možeš da testiraš na Railway-u
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlexFit API V1");
                c.RoutePrefix = "swagger"; // Swagger ?e biti na /swagger
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            // Custom throttling middleware
            app.UseMiddleware<SmartThrottlingMiddleware>();

            // --- POCETNA PORUKA (Health Check) ---
            app.MapGet("/", () => Results.Json(new
            {
                Message = "FlexFit API uspešno radi!",
                Status = "Online",
                Database = "Connected (Postgres & MongoDB)",
                Environment = app.Environment.EnvironmentName,
                Time = DateTime.UtcNow
            }));
            // -------------------------------------

            app.MapControllers();

            app.Run();
        }
    }
}