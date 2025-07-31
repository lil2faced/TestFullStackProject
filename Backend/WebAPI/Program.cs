
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPI.Core.Interfaces;
using WebAPI.Infrastructure.EfCore;
using WebAPI.Infrastructure.Services;
using WebAPI.WebAPI.Mapping;
using WebAPI.WebAPI.Middlewares;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseConsoleLifetime();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtKey = builder.Configuration["JWT:KEY"]
                        ?? throw new InvalidOperationException("JWT Key not configured");
                    var jwtIssuer = builder.Configuration["JWT:ISSUER"]
                        ?? throw new InvalidOperationException("JWT Issuer not configured");
                    var jwtAudience = builder.Configuration["JWT:AUDIENCE"]
                        ?? throw new InvalidOperationException("JWT Audience not configured");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["auth-token"];
                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<DefaultMappingProfile>();
            }, typeof(Program).Assembly);

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IUserApiService, UserApiService>();
            builder.Services.AddScoped<IJWTProvider, JWTProvider>();
            builder.Services.AddScoped<IUserApiRoleService, ApiRoleService>();
            builder.Services.AddDbContext<ApiDbContext>();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseForwardedHeaders();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
                    };
                });
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger"; 
            });

            app.UseRouting();
            app.UseCors("AllowAll");
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
