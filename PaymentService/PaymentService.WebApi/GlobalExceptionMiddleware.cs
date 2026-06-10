namespace PaymentService.WebApi.GlobalExceptionMiddleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
            }
        }
    }
}
