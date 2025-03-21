using System.Data.SqlTypes;
using System.Text.Json;

namespace TodoList.Handler
{
    public class HandlerException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandlerException> _logger;

        public HandlerException(RequestDelegate next, ILogger<HandlerException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandlerExceptionAsync(httpContext, e);
            }
        }

        private async Task HandlerExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                OverflowException => StatusCodes.Status500InternalServerError,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                SqlTypeException => StatusCodes.Status507InsufficientStorage,
                Exception => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };

            _logger.LogError(ex.Message);

            var response = new
            {
                error = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
