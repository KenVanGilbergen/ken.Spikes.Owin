using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.WsFederation;
using Owin;

namespace ken.Spikes.Owin.AuthBean
{
    public class BeanMiddleware
    {

        private readonly OwinMiddleware _next;
        private readonly IAppBuilder _app;
        private readonly BeanMiddlewareOptions _options;

        private readonly WsFederationAuthenticationMiddleware _innerMiddleware;

        public BeanMiddleware(OwinMiddleware next, IAppBuilder app, BeanMiddlewareOptions options)
        {
            if (null == next) throw new ArgumentNullException("next");
            if (null == app) throw new ArgumentNullException("app");
            if (null == options) throw new ArgumentNullException("options");
            _next = next;
            _app = app;
            _options = options;

            //app.Use<WsFederationAuthenticationMiddleware>(app, options);
            _innerMiddleware = new WsFederationAuthenticationMiddleware(next, app, options);
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("Bean IN");

            var ctx = new OwinContext(environment);

            await _innerMiddleware.Invoke(ctx);

            Debug.WriteLine("Bean OUT");
        }

    }
}

//app.Use(async (ctx, next) =>
//{
//    var qs = ctx.Request.Query;
//    var wa = qs.Get("wa");
 
//    if (wa != null)
//    {
//        if (wa == "wsignoutcleanup1.0")
//        {
//            // clean up resources, e.g. the logon cookie
 
//            ctx.Authentication.SignOut("Cookies");
//        }
//    }
 
//    await next();
//});

//if (signupPath.IsNotNullOrWhitespace())
//{
//    //WsFederationConfiguration configuration = null;

//    app.Map(signupPath, map =>
//    {
//        map.Use((c, n) =>
//        {
//            //if (configuration == null)
//            //{
//            //    var getOptionsTask =
//            //        options.ConfigurationManager.GetConfigurationAsync(c.Request.CallCancelled);
//            //    getOptionsTask.Wait();
//            //    configuration = getOptionsTask.Result;
//            //}

//            //var builder = new UriBuilder(configuration.TokenEndpoint
//            var metadataUri = new Uri(metadataEndpoint);
//            var builder = new UriBuilder
//            {
//                Scheme =  metadataUri.Scheme,
//                Host = metadataUri.Host,
//                Port = metadataUri.Port,
//                Query = $"wa=registeruser1.0&wtrealm={options.Wtrealm}&wreply={options.Wreply}"
//            };
//            c.Response.StatusCode = 301;
//            c.Response.Headers.Set("Location", builder.Uri.AbsoluteUri);

//            return Task.FromResult(0);
//        });
//    });
//}
