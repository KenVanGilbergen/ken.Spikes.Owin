using ken.Spikes.Owin.AuthBean;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public static class BeanMiddlewareExtensions
    {
        public const string BeanCookieName = "kenAuthBean";
 
        public static void UseBeanMiddleware(this IAppBuilder app, BeanMiddlewareOptions options = null)
        {
            if (options == null) options = new BeanMiddlewareOptions();

            app.Map("/signout", map => map.Run(ctx =>
            {
                //ctx.Authentication.SignOut(new[] { CookieAuthenticationDefaults.AuthenticationType, AuthenticationTypes.Federation });
                ctx.Response.Cookies.Delete(BeanCookieName);
                ctx.Response.Redirect(options.SignoutUrl);
                return ctx.Response.WriteAsync("Signing out... ");
            }));
            
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
                {
                    CookieName = BeanCookieName
                });
            app.Use<BeanMiddleware>(app, options);
        }
    }
}