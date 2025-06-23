using Microsoft.EntityFrameworkCore;
using ShipmentApi.DataContextClass;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContextPool<DataDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
    {
        npgsqlOptions.CommandTimeout(60); // ⏱ Increase timeout for long queries (in seconds)
        npgsqlOptions.EnableRetryOnFailure(3); // 🔁 Retry on transient failures
    });

    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // 🚀 Improves read-only query performance
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
