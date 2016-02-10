using Owin;

namespace ken.Spikes.Owin.LiveReload
{
    public static class LiveReloadMiddlewareExtensions
    {
        public static IAppBuilder UseLiveReload(this IAppBuilder builder)
        {
            return builder.Use<LiveReloadMiddleware>();
        }
    }
}