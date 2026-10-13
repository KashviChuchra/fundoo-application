using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Runtime.Intrinsics.X86;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunDo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Business Layer
            builder.Services.AddScoped<IUserBL, UserBL>();
            builder.Services.AddScoped<IForgotPasswordBL, ForgotPasswordBL>();
            builder.Services.AddScoped<IResetPasswordBL, ResetPasswordBL>();

            // Repository Layer
            builder.Services.AddScoped<IUserRL, UserRL>();
            builder.Services.AddScoped<IForgotPasswordRL, ForgotPasswordRL>();
            builder.Services.AddScoped<IResetPasswordRL, ResetPasswordRL>();


            // JWT Service
            builder.Services.AddScoped<IJwtService, JwtService>();

            // HTTP client for Messaging Service
            builder.Services.AddHttpClient("MessagingService");


            // PostgreSQL Database
            builder.Services.AddDbContext<FundooContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("FundooConnection"))
            );

            // Add CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FundooCorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // JWT Authentication
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // This configures  1) authentication services and selects the default authentication scheme.
                // default scheme is Bearer, which tells ASP.NET Core to use JWT bearer authentication by default.
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // only accept tokens from an expected issuer.
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                     };
                }); // ClockSkew	Controls the allowed tolerance around time validation. In this example, ClockSkew = TimeSpan.Zero means no additional clock tolerance is allowed.The default in many token-validation configurations is five minutes, which may be appropriate when servers have minor clock differences.

            builder.Services.AddAuthorization();
            //They enforce endpoint requirements such as [Authorize], roles and policies.   
            // Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token."
                });

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] =
                            new List<string>()
                    });
            });


            var app = builder.Build();

            // Middleware
            if (app.Environment.IsDevelopment()) 
            { 
                app.UseSwagger(); 
                app.UseSwaggerUI(); 
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseRouting();

            // cors
            app.UseCors("FundooCorsPolicy");

            // JWT Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            // Authentication must run before authorization so the application can establish the user's identity before checking their permissions.

            app.MapControllers();

            app.Run();
        }
    }
}
