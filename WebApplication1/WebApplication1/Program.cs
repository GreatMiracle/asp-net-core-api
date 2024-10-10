
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Services.Impl;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Nếu bạn có tệp cấu hình khác, hãy chắc chắn rằng nó được nạp đúng cách.
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

Env.Load();

// Kiểm tra và in ra giá trị các biến môi trường
Console.WriteLine($"NOW_ENVIRONMENT: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DB_HOST: {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"DB_PORT: {Environment.GetEnvironmentVariable("DB_PORT")}");
Console.WriteLine($"DB_USERNAME: {Environment.GetEnvironmentVariable("DB_USERNAME")}");
Console.WriteLine($"DB_PASSWORD: {Environment.GetEnvironmentVariable("DB_PASSWORD")}");
Console.WriteLine($"DB_DATABASE: {Environment.GetEnvironmentVariable("DB_DATABASE")}");
Console.WriteLine($"WALKS_CONNECTION_STRING: {builder.Configuration.GetConnectionString("WalksConnectionString")}");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WalksDbContext>(options =>
{
    var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                           $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                           $"Username={Environment.GetEnvironmentVariable("DB_USERNAME")};" +
                           $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                           $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                           "Trust Server Certificate=true;";

    options.UseNpgsql(connectionString);
});
//--comment vì cái này chạy vào appsetting.Development.json mà tôi không muốn hard code vào đó rất nguy hiểm
//options.UseNpgsql(builder.Configuration.GetConnectionString("WalksConnectionString1"))); 

// Đăng ký Repository và Service
builder.Services.AddScoped<IRepository<Region>, RegionRepository>();
builder.Services.AddScoped<IRegionService, RegionService>();

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
