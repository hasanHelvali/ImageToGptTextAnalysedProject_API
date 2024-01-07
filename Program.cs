
using GIPAPI.Abstracts.Services;
using GIPAPI.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using OpenAI.GPT3.Extensions;
namespace GIPAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
                    options.AddDefaultPolicy(policy =>
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
            //policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            builder.Services.AddOpenAIService(settings => settings.ApiKey = builder.Configuration["ChatGptApiKey"]);

            builder.Services.AddScoped<IImageToTextService, ImageToTextService>();
            builder.Services.AddScoped<IChatGptService, ChatGptService>();
            
            bool resulta = IronOcr.License.IsValidLicense(builder.Configuration["IronOcrLicenceKey"]);

            //builder.Services.Configure<FormOptions>(o =>
            //{
            //    o.ValueLengthLimit = int.MaxValue;
            //    o.MultipartBodyLengthLimit = int.MaxValue;
            //    o.MemoryBufferThreshold = int.MaxValue;
            //});
            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseCors();
            app.UseStaticFiles();//wwwroot icin cagrýlan bir middleware dir
            // Configure the HTTP request pipeline.
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
