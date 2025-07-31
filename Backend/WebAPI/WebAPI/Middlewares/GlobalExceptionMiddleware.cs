using WebAPI.Core.Exceptions;

namespace WebAPI.WebAPI.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            } 
            catch (NotAutentificationException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 501;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
