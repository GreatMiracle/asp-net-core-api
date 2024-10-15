
using System.Net;
using System.Text.Json;

namespace WebApplication1.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Tiếp tục xử lý request nếu không có exception
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Nếu exception là custom exception, gọi phương thức xử lý custom
                if (ex is CustomException)
                {
                    await HandleCustomExceptionAsync(httpContext, ex);
                }
                else
                {
                    // Gọi phương thức xử lý cho các exception thông thường
                    await HandleExceptionAsync(httpContext, ex);
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Ghi log lỗi
            _logger.LogError(exception, "An unhandled exception has occurred.");

            // Cấu hình response trả về cho client
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Something went wrong, please contact support.",
                Details = exception.Message
            };

            // Trả về response JSON
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleCustomExceptionAsync(HttpContext context, Exception exception)
        {
            // Kiểm tra nếu exception là custom exception
            if (exception is CustomException customException)
            {
                // Xử lý cho CustomException
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = customException.StatusCode;

                var response = new
                {
                    StatusCode = customException.StatusCode,
                    Message = customException.Message
                };

                return context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            // Ghi log lỗi cho các exception khác
            _logger.LogError(exception, "An unhandled exception has occurred.");

            // Xử lý các exception thông thường
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var defaultResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Something went wrong, please contact support.",
                Details = exception.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(defaultResponse));
        }
    }

}
