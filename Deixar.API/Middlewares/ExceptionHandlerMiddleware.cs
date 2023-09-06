using Sentry;

namespace Deixar.API.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.WriteAsync(ex.ToString());
                SentrySdk.CaptureException(ex);
            }
        }
    }
}
