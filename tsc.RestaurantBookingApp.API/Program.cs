
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;
using tsc.RestaurantBookingApp.Data;
using tsc.RestaurantBookingApp.Service;
using tsc.RestaurantTableBookingApp.API.Middleware;
using System.Text.Json.Serialization;
using tsc.RestaurantTableBookingApp.API;

namespace tsc.RestaurantBookingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Register Application Insights telemetry
                builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:ConnectionString"]);

                // Configure Serilog
                builder.Host.UseSerilog((context, services, loggerConfiguration) =>
                {
                    var telemetryConfiguration = services.GetRequiredService<TelemetryConfiguration>();
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services)
                        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
                });

                Log.Information("Starting the application...");

                // Add services to the container
                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();

                builder.Services.AddDbContext<RestaurantTableBookingDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext") ?? "")
                        .EnableSensitiveDataLogging()
                        .LogTo(Console.WriteLine, LogLevel.Information));

                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });

                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected exception occurred, please try again later.");
                    });
                });

                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                app.UseMiddleware<DelayMiddleware>();

                //if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();

                Log.Information("Application is running...");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }


    /*public class Program
    {
        public static void Main(string[] args)
        {

            //Configuree Serilog with the settings
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Debug()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateBootstrapLogger();
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.AddServiceDefaults();
                var configuration = builder.Configuration;
                builder.Services.AddApplicationInsightsTelemetry();

                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Events));
                Log.Information("Starting The Application...");
                // Add services to the container.

                builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
                builder.Services.AddScoped<IRestaurantService, RestaurantService>();
               


                builder.Services.AddDbContext<RestaurantTableBookingDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbContext") ?? "")
                .EnableSensitiveDataLogging() //should not be used in production, only for development purpose
                );

                builder.Services.AddControllers()
                    .AddJsonOptions(options => {
                        // Ignore self reference loop
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });
                builder.Services.AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

                //builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                //exception handling, creating a middleware and inclduing that here
                //enabling serilog exception logging
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occured. {ExceptionDetails}", exception?.ToString());
                        Console.WriteLine(exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An Unexpected exception occured, please try again later");
                    });
                });
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                app.UseMiddleware<DelayMiddleware>();


                app.MapDefaultEndpoints();

                // Configure the HTTP request pipeline.

                
                    app.UseSwagger();
                    app.UseSwaggerUI();
                

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host Terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }*/
}





