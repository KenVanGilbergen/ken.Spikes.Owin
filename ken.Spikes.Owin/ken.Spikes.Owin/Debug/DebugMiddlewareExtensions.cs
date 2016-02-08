// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class DebugMiddlewareExtensions
    {
        public static void UseDebugMiddleware(this IAppBuilder app, DebugMiddlewareOptions options = null)
        {
            if (null == options) options = new DebugMiddlewareOptions();

            app.Use<DebugMiddleware>(options);
        }
    }
}