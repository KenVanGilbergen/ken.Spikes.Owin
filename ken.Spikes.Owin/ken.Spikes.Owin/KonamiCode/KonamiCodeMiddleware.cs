using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ken.Spikes.Owin.HtmlAppender;
using Microsoft.Owin;

namespace ken.Spikes.Owin.KonamiCode
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class KonamiCodeMiddleware : HtmlAppenderMiddleware
    {
        private readonly KonamiCodeMiddlewareOptions _options;

        public KonamiCodeMiddleware(AppFunc next, KonamiCodeMiddlewareOptions options) : 
            base(next, new HtmlAppenderMiddlewareOptions {
                Head = String.Format("<script type='text/javascript' src='{0}/konami.js'></script>", options.AssetsPath),
                Body = String.Format("<script>var easter_egg = new Konami(function() {{ {0} }});</script>", options.Action)
            })
        {
            if (null == options) throw new ArgumentNullException("options");
            _options = options;
        }

        public override async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("KonamiCode IN");
            
            var ctx = new OwinContext(environment);
            if (await HandledAsset(ctx, new PathString(_options.AssetsPath), "ken.Spikes.Owin.KonamiCode.assets.")) return;

            await base.Invoke(environment);        

            Debug.WriteLine("KonamiCode OUT");
        }

    }
}
