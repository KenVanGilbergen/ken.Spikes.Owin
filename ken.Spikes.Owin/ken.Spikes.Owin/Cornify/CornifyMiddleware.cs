using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ken.Spikes.Owin.Cornify
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class CornifyMiddleware
    {
        private readonly AppFunc _next;
        private readonly CornifyMiddlewareOptions _options;

        public CornifyMiddleware(AppFunc next, CornifyMiddlewareOptions options)
        {
            if (null == next) throw new ArgumentNullException("next");
            if (null == options) throw new ArgumentNullException("options");
            _next = next;
            _options = options;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("Cornify IN");
            
            var ctx = new OwinContext(environment);

            if (await HandledCornifyAsset(ctx, new PathString(_options.CornifyAssetsPath))) return;

            var realStream = ctx.Response.Body;
            var bufferStream = new MemoryStream();
            ctx.Response.Body = bufferStream;
            
            await _next(environment);

            ctx.Response.Body = realStream;

            if (!ctx.IsHtmlResponse()) return;

            bufferStream.Seek(0,SeekOrigin.Begin);
            //await stream.CopyToAsync(ctx.Response.Body);
            var sr = new StreamReader(bufferStream);
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                if (str.Contains("<head>"))
                {
                    str = str.Replace(
                        "<head>", 
                        String.Format("<head><script type='text/javascript' src='{0}/cornify.js'></script>", _options.CornifyAssetsPath)
                        );
                }
                if (str.Contains("</body>"))
                {
                    str = str.Replace(
                        "</body>",
                        "<script>(function() { setInterval(function(){ cornify_add(); }, " + _options.AddDelayInMilliseconds + "); })();</script>");
                }
                await ctx.Response.WriteAsync(str);
            }

            Debug.WriteLine("Cornify OUT");
        }

        private async Task<Byte[]> GetFromResources(string location)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ken.Spikes.Owin.Cornify.assets." + location.Replace("/", "");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                var assemblyData = new Byte[stream.Length];
                await stream.ReadAsync(assemblyData, 0, assemblyData.Length);
                return assemblyData;
            }
        }

        private async Task<bool> HandledCornifyAsset(IOwinContext ctx, PathString cornifyAssetsPath)
        {
            var path = ctx.Request.Path;
            PathString remainingPath;
            if (path.StartsWithSegments(cornifyAssetsPath, out remainingPath))
            {
                var asset = GetFromResources(remainingPath.Value);
                if (null != asset.Result)
                {
                    await ctx.Response.WriteAsync(asset.Result);
                    return true;
                }
            }
            return false;
        }

    }
}

           