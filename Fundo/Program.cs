using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FunDo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserBL, UserBL>();
            builder.Services.AddScoped<IUserRL, UserRL>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            // Write vo class, jiska depenedency injection ban na chahtta hai

            //---------------------
            builder.Services.AddDbContext<FundooContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("FundooConnection"))
            );
            //yha jo connection bnate hue value pass kr rhe hai vo jayegi
            // options --> parameter, 
            // what are the methods inside options.??
            //------------------------


            // add authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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


            // Adding swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // enabling swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            // JWT Authentication
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
