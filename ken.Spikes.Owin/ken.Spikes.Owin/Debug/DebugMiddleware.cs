using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

// ReSharper disable once CheckNamespace
namespace Owin
{
    using AppFunc = System.Func<IDictionary<string, object>,Task>;

    public class DebugMiddleware
    {
        private readonly AppFunc _next;
        private readonly DebugMiddlewareOptions _options;

        public DebugMiddleware(AppFunc next, DebugMiddlewareOptions options)
        {
            if (null == next) throw new ArgumentNullException("next");
            if (null == options) throw new ArgumentNullException("options");
            _next = next;
            _options = options;

            if (_options.OnIncomingRequest == null)
                _options.OnIncomingRequest = (ctx) => { System.Diagnostics.Debug.WriteLine("Incoming request: " + ctx.Request.Path); };

            if (_options.OnOutgoingRequest == null)
                _options.OnOutgoingRequest = (ctx) => { System.Diagnostics.Debug.WriteLine("Outgoing request: " + ctx.Request.Path); };

        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);

            _options.OnIncomingRequest(ctx);
            await _next(environment);
            _options.OnOutgoingRequest(ctx);
        }
    }
}