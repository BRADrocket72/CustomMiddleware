public class AuthMiddleWare
{
    private readonly RequestDelegate _next;

    public AuthMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        String? username = httpContext.Request.Query["username"];
        String? password = httpContext.Request.Query["password"];
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await httpContext.Response.WriteAsync("Not Authorized");
            return;
        }
        if (username != "user1" || password != "password1")
        {
            await httpContext.Response.WriteAsync("Not Authorized");
            return;
        }
        await _next(httpContext);

    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleWare>();
    }
}