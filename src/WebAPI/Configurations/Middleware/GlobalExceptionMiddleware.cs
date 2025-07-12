namespace WebAPI.Configurations.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            // catch (ValidationException ex)
            // {
            //     // Log the exception (optional)
            //     _logger.LogError(ex, "Validation error");

            //     // Format the response with validation errors
            //     var validationErrors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage });
            //     var response = new
            //     {
            //         status = "error",
            //         message = "Validação falhou",
            //         errors = validationErrors
            //     };

            //     // Set the status code and return the response with errors
            //     context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //     context.Response.ContentType = "application/json";
            //     await context.Response.WriteAsJsonAsync(response);
            // }
            catch (Exception ex)
            {
                // For other exceptions, you can proceed with the default handling
                _logger.LogError(ex, "Unexpected error");

                var response = _env.IsDevelopment()
                    ? new { message = ex.Message, stackTrace = ex.StackTrace }
                    : new { message = "An internal server error occurred.", stackTrace = (string?)null };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
