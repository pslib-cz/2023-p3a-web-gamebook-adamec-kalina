using Microsoft.AspNetCore.Builder.Extensions;

namespace HackAttack.Middlewares;

public class RestrictingUsePathBaseMiddleware
{
    private readonly PathString pathBase;
    private readonly UsePathBaseMiddleware usePathBaseMiddleware;

    public RestrictingUsePathBaseMiddleware(RequestDelegate next, PathString pathBase)
    {
        this.pathBase = pathBase;
        usePathBaseMiddleware =new UsePathBaseMiddleware(next, pathBase);
    }

    public Task Invoke(HttpContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Request.Path.StartsWithSegments(pathBase, out var matchedPath, out var remainingPath))
        {
            return usePathBaseMiddleware.Invoke(context);
        }

        context.Response.StatusCode = StatusCodes.Status404NotFound; // do what is appropriate with the Response
        return Task.CompletedTask;
    }
}

public static class RestrictingUsePathBaseExtensions
{
    public static IApplicationBuilder UseRestrictingPathBase(this IApplicationBuilder app, PathString pathBase)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        pathBase = pathBase.Value?.TrimEnd('/');
        if (!pathBase.HasValue)
        {
            return app;
        }

        return app.UseMiddleware<RestrictingUsePathBaseMiddleware>(pathBase);
    }
}