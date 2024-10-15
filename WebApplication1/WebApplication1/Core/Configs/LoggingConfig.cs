using Serilog;

namespace WebApplication1.Core.Configs
{
    public static class LoggingConfig
    {
        public static void ConfigureLogging(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information() // Thiết lập mức độ tối thiểu là Information
                .WriteTo.Console() // Ghi log ra console
                .WriteTo.File("Logs/Walks_log.txt", rollingInterval: RollingInterval.Minute) // Ghi log ra tệp
                .CreateLogger();

            Log.Logger = logger; // Đặt logger toàn cục

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Xóa nhà cung cấp log mặc định
                loggingBuilder.AddSerilog(); // Thêm Serilog
            });


            Log.Information("Logging is configured."); // Ghi log thông báo
        }
    }
}
