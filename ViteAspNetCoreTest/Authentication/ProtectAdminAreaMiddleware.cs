namespace ViteAspNetCoreTest.Authentication;

public class ProtectAdminAreaMiddleware
{
    private readonly RequestDelegate _next;

    public ProtectAdminAreaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        /*if (context.Request.Path.StartsWithSegments("/js/admin"))
        {
            var user = context.User;

            if (user.Identity is { IsAuthenticated: false })
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }*/

        await _next(context);
    }
}

public static class ProtectAdminAreaMiddlewareExtensions
{
    public static IApplicationBuilder UseProtectAdminArea(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ProtectAdminAreaMiddleware>();
    }
}