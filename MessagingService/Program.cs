using MessagingService_BusinessLayer.Helper;
using MessagingService_BusinessLayer.Interface;
using MessagingService_BusinessLayer.Service;
namespace MessagingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IEmailBL, EmailBL>();
            builder.Services.AddScoped<SmtpHelper>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger(); 
            app.UseSwaggerUI();



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
