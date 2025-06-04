using EF_DBFIRST.DAL;
using EF_DBFIRST.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EF_DBFIRST;

public class Program
{
    public static void Main(string[] args)
    {
         var builder = WebApplication.CreateBuilder(args);
        
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        
        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        
        builder.Services.AddDbContext<TripsContext>(opt =>
        {
            opt.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        });
        
        
        //registering dependencies
        //builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
        
        

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "PrescriptionApi",
                Description = "api for managing prescriptions",
                Contact = new OpenApiContact
                {
                    Name="Api Support",
                    Email="apiSupport@gmail.com",
                    Url = new Uri("https://github.com/apiSupport")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseGlobalExceptionHandling();
        
        app.UseSwagger();
        
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "webTripApi");
            
            c.DocExpansion(DocExpansion.List);
            c.DefaultModelExpandDepth(0);
            c.DisplayRequestDuration();
            c.EnableFilter();
            
        });
        

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}