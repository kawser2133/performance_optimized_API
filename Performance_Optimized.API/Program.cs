using Microsoft.EntityFrameworkCore;
using Performance_Optimized.API.Extensions;
using Performance_Optimized.API.Middlewares;
using Performance_Optimized.Core.Interfaces.IServices;
using Performance_Optimized.Core.Services;
using Performance_Optimized.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PrimaryDbConnection")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// For In-Memory Cache:
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MemoryCacheService>();

// For Redis Cache:
builder.Services.AddSingleton<RedisCacheService>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("RedisConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Redis connection string is missing or empty.");
    }
    return new RedisCacheService(connectionString);
});

// Add CacheServiceFactory
builder.Services.AddSingleton<ICacheServiceFactory, CacheServiceFactory>();

// Add services to the container.
builder.Services.RegisterService();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DataSeeder.SeedData(context);
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
