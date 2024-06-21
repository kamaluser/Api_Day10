using CourseApp.UI.Exceptions;
using System.Net;

namespace CourseApp.UI.Middlewares
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpException ex)
            {
                if (ex.Status == HttpStatusCode.Unauthorized)
                {
                    context.Response.Redirect("/auth/login");
                }
            }
            catch (Exception)
            {
                context.Response.Redirect("/home/error");
            }
        }
    }

}
