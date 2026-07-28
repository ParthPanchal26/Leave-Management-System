namespace Leave_Management_System.API.ExceptionHandler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                _logger.LogError($"System Error: {ex.Message}");

                await context.Response.WriteAsJsonAsync(new
                {
                    Success = false,
                    StatusCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = (object?)null
                });
            }
        }
    }
}
