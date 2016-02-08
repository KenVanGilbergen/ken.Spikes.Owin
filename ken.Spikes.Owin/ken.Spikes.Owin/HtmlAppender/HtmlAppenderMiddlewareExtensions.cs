using ken.Spikes.Owin.HtmlAppender;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class HtmlAppenderMiddlewareExtensions
    {
        public static void UseHtmlAppenderMiddleware(this IAppBuilder app, HtmlAppenderMiddlewareOptions options = null)
        {
            if (null == options) options = new HtmlAppenderMiddlewareOptions();

            app.Use<DebugMiddleware>(options);
        }
    }
}