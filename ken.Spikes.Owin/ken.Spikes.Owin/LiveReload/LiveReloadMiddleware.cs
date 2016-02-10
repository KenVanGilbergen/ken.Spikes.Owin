using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ken.Spikes.Owin.HtmlAppender;

namespace ken.Spikes.Owin.LiveReload
{
    public class LiveReloadMiddleware : HtmlAppenderMiddleware
    {
        private readonly LiveReloadMiddlewareOptions _options;

        public LiveReloadMiddleware(Func<IDictionary<string, object>, Task> next, LiveReloadMiddlewareOptions options) :
            base(next, new HtmlAppenderMiddlewareOptions
            {
                Head = String.Format("<script src='//{0}:{1}/livereload.js'></script>", options.Host, options.Port),
                AppendToBody = false
            })
        {
            if (null == options) throw new ArgumentNullException("options");
            _options = options;
        }

        public override async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("LiveReload IN");
            await base.Invoke(environment);
            Debug.WriteLine("LiveReload OUT");
        }

    }
}
