﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderApi.DbContextClass;
using OrderApi.Helpers;
using OrderApi.RabbitMqConsumer;
using OrderApi.RabbitMqReceiver;
using OrderApi.Repository;
using OrderApi.Utilities.ContextHelper;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------- Configuration & Validation -----------------------------

// Validate JWT Secret
var jwtSecret = builder.Configuration["JWTKey:Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
    throw new Exception("JWTKey:Secret must be at least 32 characters long.");

// ----------------------------- Services Registration -----------------------------

// Database: High-performance DB context with connection pooling and no tracking by default
builder.Services.AddDbContextPool<DataDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds)
    )
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

// Controllers with consistent, fast JSON serialization config
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = false;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// CORS Policy (you can restrict origin in production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApi", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ✅ Register Application Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IContextUser, ContextUser>();
builder.Services.AddScoped<IRabbitMqConsumerService, RabbitMqConsumerService>();

// Add Hosted Service for RabbitMQ Consumer
builder.Services.AddHostedService<OrderConsumer>();

// ✅ Mapster Config for DTO Mapping
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(new MapsterProfile());


//builder.Services.AddHttpClient(); // Required for IHttpClientFactory
builder.Services.AddHttpClient("ProductApi", client =>
{
    client.BaseAddress = new Uri("https://api.example.com/"); // ⬅️ Change this to your actual API base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    // Optionally add API key headers here if needed:
    // client.DefaultRequestHeaders.Add("Authorization", "Bearer your_api_key");
});

builder.Services.AddHttpContextAccessor();

// JWT Authentication & Authorization
builder.AddAppAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();

// 🧭 Swagger Configuration with JWT Bearer
builder.Services.AddSwaggerGen(options =>
{
    // JWT Bearer auth setup
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter your JWT token below with **Bearer** prefix.\r\nExample: Bearer eyJhbGciOi...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, // Use Http instead of ApiKey for better support
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
               {
            new OpenApiSecurityScheme
                  {
                      Reference = new OpenApiReference
                       {
                             Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                       }
                   },
            Array.Empty<string>()
               }
            });

    // Optional: Add XML comment support for controller documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add Response Caching (optional)
builder.Services.AddResponseCaching();

// ----------------------------- App Pipeline -----------------------------

var app = builder.Build();

// ✅ Set OrderId seed to start from 1000
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataDbContext>();

    // If using migrations:
    // await dbContext.Database.MigrateAsync();

    // Optional safety check
    if (!dbContext.OrderDetails.Any())
    {
        await dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('OrderDetails', RESEED, 999)");
    }
}

// Swagger for development only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowApi");

app.UseResponseCaching();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
