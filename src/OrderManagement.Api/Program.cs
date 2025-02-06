using OrderManagement.Application;
using OrderManagement.Infrastructure;
using OrderManagement.Infrastructure.DataAccess;
using OrderManagement.Infrastructure.DataAccess.Management;
using OrderManagement.Infrastructure.Logging;
using Serilog;
using FluentValidation;
using OrderManagement.Api.Filter;
using OrderManagement.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

// TODO: Add health checks

var builder = WebApplication.CreateBuilder(args);

// Load environment-specific appsettings file
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add Serilog to the application
builder.Host.UseSerilogLogging(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); // This applies validation to all controllers
});

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddInfrastructureDataAccess(builder.Configuration).AddInfrastructure().AddApplication();

// Register AutoMapper and Load All Profiles Automatically
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Management API", Version = "v1" });

    // Add JWT Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer <your_token>' in the field below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.EnableAnnotations();

    // Require JWT Authentication in Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    // Enforce authorization globally for all controllers
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

app.UseMiddleware<JwtMiddleware>();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();

app.Services.Migrate().SeedData(app.Environment.IsDevelopment());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Handle Unauthorized Responses Gracefully
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 401)
    {
        response.ContentType = "application/json";
        await response.WriteAsync("{\"message\": \"Unauthorized. Please provide a valid token.\"}");
    }
});

app.MapControllers();

try
{
    Log.Information("Starting application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush(); // Ensures logs are flushed before shutdown
}
