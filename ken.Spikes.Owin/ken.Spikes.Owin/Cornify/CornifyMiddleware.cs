using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ken.Spikes.Owin.HtmlAppender;
using Microsoft.Owin;

namespace ken.Spikes.Owin.Cornify
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class CornifyMiddleware : HtmlAppenderMiddleware
    {
        private readonly CornifyMiddlewareOptions _options;

        public CornifyMiddleware(AppFunc next, CornifyMiddlewareOptions options) : 
            base(next, new HtmlAppenderMiddlewareOptions {
                Head = String.Format("<script type='text/javascript' src='{0}/cornify.js'></script>", options.AssetsPath),
                Body = "<script>(function() { setInterval(function(){ cornify_add(); }, " + options.AddDelayInMilliseconds + "); })();</script>",
                AppendToBody = options.Autostart
            })
        {
            if (null == options) throw new ArgumentNullException("options");
            _options = options;
        }

        public override async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("Cornify IN");
            
            var ctx = new OwinContext(environment);
            if (await HandledAsset(ctx, new PathString(_options.AssetsPath), "ken.Spikes.Owin.Cornify.assets.")) return;

            await base.Invoke(environment);        

            Debug.WriteLine("Cornify OUT");
        }

    }
}

           