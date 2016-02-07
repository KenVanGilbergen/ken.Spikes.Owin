using ken.Spikes.Owin.ServeDirectory;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class ServeDirectoryMiddlewareExtensions
    {
        public static void UseServeDirectoryMiddleware(this IAppBuilder app, ServeDirectoryMiddlewareOptions options = null)
        {
            if (options == null) options = new ServeDirectoryMiddlewareOptions();

            app.Use<ServeDirectoryMiddleware>(options);
        }
    }
}