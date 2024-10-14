
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Exceptions;
using WebApplication1.Filters;
using WebApplication1.Infrastructure.Data;
using WebApplication1.Infrastructure.Repositories;
using WebApplication1.Infrastructure.Repositories.Impl;
using WebApplication1.Services;
using WebApplication1.Services.Impl;
using WebApplication1.Services.Mappings;
using WebApplication1.Validators;
using Microsoft.Extensions.Configuration; // Thêm using này
using WebApplication1.Filters;
using WebApplication1.Core.SwaggerConfig;
using Microsoft.Extensions.FileProviders;
using OfficeOpenXml;
using WebApplication1.Core.Configs; // Thêm using này cho JwtConfigure

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Nếu bạn có tệp cấu hình khác, hãy chắc chắn rằng nó được nạp đúng cách.
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

Env.Load();
var key = Environment.GetEnvironmentVariable("AES_KEY");
var iv = Environment.GetEnvironmentVariable("AES_IV");

// Kiểm tra và in ra giá trị các biến môi trường
Console.WriteLine($"NOW_ENVIRONMENT: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DB_HOST: {Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"DB_PORT: {Environment.GetEnvironmentVariable("DB_PORT")}");
Console.WriteLine($"DB_USERNAME: {Environment.GetEnvironmentVariable("DB_USERNAME")}");
Console.WriteLine($"DB_PASSWORD: {Environment.GetEnvironmentVariable("DB_PASSWORD")}");
Console.WriteLine($"DB_DATABASE: {Environment.GetEnvironmentVariable("DB_DATABASE")}");
Console.WriteLine($"WALKS_CONNECTION_STRING: {builder.Configuration.GetConnectionString("WalksConnectionString")}");


// Thiết lập LicenseContext cho EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Hoặc LicenseContext.Commercial nếu bạn có giấy phép thương mại

// Add services to the container.
builder.Services.AddControllers();

// Add url file img.
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// Cấu hình Swagger
SwaggerConfig.AddSwaggerGenServices(builder.Services);

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

builder.Services.AddDbContext<AuthWalksDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("AuthWalksConnectionString")));

//--comment vì cái này chạy vào appsetting.Development.json mà tôi không muốn hard code vào đó rất nguy hiểm
//options.UseNpgsql(builder.Configuration.GetConnectionString("WalksConnectionString1"))); 

// Đăng ký Repository, Service 
builder.Services.AddApplicationServices();

// Đăng ký Validate request Api
builder.Services.AddFluentValidationServices();

// Đăng ký AutoMapper và quét các lớp Profile
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Gọi phương thức ConfigureServices từ JwtConfigure và truyền Configuration
JwtConfigure.ConfigureJWTServices(builder.Services, builder.Configuration);

// Sử dụng IdentityConfiguration để cấu hình Identity
builder.Services.ConfigureIdentity();

// Cấu hình Authentication và các dịch vụ khác
builder.Services.AddAuthentication();

// Cấu hình Google OAuth2 Authentication
GgOAuth2Config.AddGgOAuth2Config(builder.Services, builder.Configuration);

var app = builder.Build();

// Sử dụng CORS cho toàn bộ ứng dụng hoặc chỉ một phần
app.UseCors("AllowAllOrigins");

// Bật middleware để phục vụ các tệp tĩnh
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "ImageUp")),
    RequestPath = "/Images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        // Đường dẫn cho Swagger UI
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");

        // Cấu hình cho OAuth2 để đăng nhập qua Google
        options.OAuthClientId(builder.Configuration["Authentication:Google:ClientId"]);
        options.OAuthClientSecret(builder.Configuration["Authentication:Google:ClientSecret"]);
        options.OAuthAppName("Google OAuth2 Login");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
