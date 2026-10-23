using BusinessLayer.Interface;
using BusinessLayer.Note.Command;
using BusinessLayer.Note.Command.Handler_Interface;
using BusinessLayer.User.Command.HandlerInterface;
using BusinessLayer.Note.Handler;
using BusinessLayer.Note.Query;
using BusinessLayer.Service;
using BusinessLayer.User.Command;
using BusinessLayer.User.Query;
using Fundoo.ExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Middleware.Services.Email;
using Middleware.Services.Hashing;
using Middleware.Services.Jwt;
using NLog;
using NLog.Web;
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
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("Application Started");


            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Controllers
                builder.Services.AddControllers();

                // N log
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

                // Notes CQRS
                builder.Services.AddScoped<ICreateNoteHandler,CreateNoteHandler>();
                builder.Services.AddScoped<IUpdateNoteHandler,UpdateNoteHandler>();
                builder.Services.AddScoped<IDeleteNoteHandler,DeleteNoteHandler>();
                builder.Services.AddScoped<IArchiveNoteHandler,ArchiveNoteHandler>();

                builder.Services.AddScoped<IGetAllNotesQueryHandler,GetAllNotesQueryHandler>();
                builder.Services.AddScoped<IGetNotesByIdQueryHandler, GetNotesByIdQueryHandler>();

                // User CQRS 
                builder.Services.AddScoped<IRegisterUserHandler,RegisterUserHandler>();
                builder.Services.AddScoped<ILoginUserQueryHandler,LoginUserQueryHandler>();
                builder.Services.AddScoped<IForgotPasswordHandler,ForgotPasswordHandler>();
                builder.Services.AddScoped<IResetPasswordHandler,ResetPasswordHandler>();

                // Business Layer
                builder.Services.AddScoped<IUserBL, UserBL>();
                builder.Services.AddScoped<INoteBL, NoteBL>();

                // Middleware
                builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
                builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
                builder.Services.AddSingleton<CustomHashing>();


                // Repository Layer
                builder.Services.AddScoped<IUserRL, UserRL>();
                builder.Services.AddScoped<INoteRL, NoteRL>();

                // PostgreSQL Database
                builder.Services.AddDbContext<FundooContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("FundooConnection"))
                );

                // HTTP client for Messaging Service
                //builder.Services.AddHttpClient("MessagingService");

                // CORS

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

                builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
                builder.Services.AddProblemDetails();

                var app = builder.Build();

                app.UseExceptionHandler();

                // Middleware
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                // Configure the HTTP request pipeline.

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseCors("FundooCorsPolicy");

                app.UseAuthentication();
                app.UseAuthorization();

                // Authentication must run before authorization so the application can establish the user's identity before checking their permissions.

                app.MapControllers();

                app.Run();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Application stopped because of an exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
