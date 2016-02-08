using ken.Spikes.Owin.KonamiCode;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class KonamiCodeMiddlewareExtensions
    {
        public static void UseKonamiCodeMiddleware(this IAppBuilder app, KonamiCodeMiddlewareOptions options = null)
        {
            if (options == null) options = new KonamiCodeMiddlewareOptions();

            app.Use<KonamiCodeMiddleware>(options);
        }
    }
}