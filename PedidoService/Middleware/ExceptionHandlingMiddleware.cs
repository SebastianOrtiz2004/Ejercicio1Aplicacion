using System.Text.Json;

namespace PedidoService.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next; _logger = logger;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";
                var res = JsonSerializer.Serialize(new { error = "Ocurrió un error interno." });
                await ctx.Response.WriteAsync(res);
            }
        }
    }
}
