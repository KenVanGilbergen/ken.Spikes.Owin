using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ken.Spikes.Owin.KonamiCode
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class KonamiCodeMiddleware
    {
        private readonly AppFunc _next;
        private readonly KonamiCodeMiddlewareOptions _options;

        public KonamiCodeMiddleware(AppFunc next, KonamiCodeMiddlewareOptions options)
        {
            if (null == next) throw new ArgumentNullException("next");
            if (null == options) throw new ArgumentNullException("options");
            _next = next;
            _options = options;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("KonamiCode IN");
            
            var ctx = new OwinContext(environment);

            if (await HandledCornifyAsset(ctx, new PathString(_options.AssetsPath))) return;

            var realStream = ctx.Response.Body;
            var bufferStream = new MemoryStream();
            ctx.Response.Body = bufferStream;
            
            await _next(environment);

            ctx.Response.Body = realStream;

            bufferStream.Seek(0, SeekOrigin.Begin);

            if (!ctx.IsHtmlResponse())
            {
                await bufferStream.CopyToAsync(ctx.Response.Body);
                return;
            }

            var sr = new StreamReader(bufferStream);
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                if (str.Contains("<head>"))
                {
                    str = str.Replace(
                        "<head>",
                        String.Format("<head><script type='text/javascript' src='{0}/konami.js'></script>", _options.AssetsPath)
                        );
                }
                if (str.Contains("</body>"))
                {
                    str = str.Replace(
                        "</body>",
                        String.Format("<script>var easter_egg = new Konami(function() {{ {0} }});</script></body>",_options.Action));
                }
                await ctx.Response.WriteAsync(str);
            }

            Debug.WriteLine("KonamiCode OUT");
        }

        private async Task<Byte[]> GetFromResources(string location)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ken.Spikes.Owin.KonamiCode.assets." + location.Replace("/", "");
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
                    ctx.Response.ContentType = path.ToMimeType();
                    await ctx.Response.WriteAsync(asset.Result);
                    return true;
                }
            }
            return false;
        }

    }
}

           