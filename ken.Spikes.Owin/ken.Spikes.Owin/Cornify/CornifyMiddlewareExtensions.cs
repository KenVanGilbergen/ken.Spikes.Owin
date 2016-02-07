using ken.Spikes.Owin.Cornify;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class CornifyMiddlewareExtensions
    {
        public static void UseCornifyMiddleware(this IAppBuilder app, CornifyMiddlewareOptions options = null)
        {
            if (options == null) options = new CornifyMiddlewareOptions();

            app.Use<CornifyMiddleware>(options);
        }
    }
}