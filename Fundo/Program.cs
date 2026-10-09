using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text;

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

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                     };
                });

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


            app.MapControllers();

            app.Run();
        }
    }
}
