using System.Net;
using System.Text.Json;

namespace OficinaTech.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro capturado no middleware: {Mensagem}", ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var problem = new
                {
                    status = context.Response.StatusCode,
                    title = "Erro interno no servidor",
                    detail = ex.Message
                };

                var jsonResponse = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
