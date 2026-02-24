using System.Net;
using System.Text.Json;

namespace KuranGuide.Api.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlenmeyen bir hata oluştu: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                KeyNotFoundException _ => (HttpStatusCode.NotFound, "İstenen kaynak bulunamadı."),
                UnauthorizedAccessException _ => (HttpStatusCode.Unauthorized, "Bu işlem için yetkiniz bulunmamaktadır."),
                ArgumentException _ => (HttpStatusCode.BadRequest, "Geçersiz istek parametreleri."),
                InvalidOperationException _ => (HttpStatusCode.Conflict, "İşlem şu anki durumla çakışmaktadır."),
                _ => (HttpStatusCode.InternalServerError, "Sunucuda beklenmeyen bir hata oluştu.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                // Development ortamında detayları göster
                Detail = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                    ? exception.Message
                    : null
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }

    // Extension method
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
