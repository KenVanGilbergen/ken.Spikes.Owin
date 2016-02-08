using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ken.Spikes.Owin.HtmlAppender
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class HtmlAppenderMiddleware
    {
        private readonly AppFunc _next;
        private readonly HtmlAppenderMiddlewareOptions _options;

        public HtmlAppenderMiddleware(AppFunc next, HtmlAppenderMiddlewareOptions options)
        {
            if (null == next) throw new ArgumentNullException("next");
            if (null == options) throw new ArgumentNullException("options");
            _next = next;
            _options = options;
        }

        public virtual async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("HtmlAppender IN");
            
            var ctx = new OwinContext(environment);

            var realStream = ctx.Response.Body;
            var bufferStream = new MemoryStream();
            ctx.Response.Body = bufferStream;
            
            await _next(environment);

            ctx.Response.Body = realStream;
            
            bufferStream.Seek(0,SeekOrigin.Begin);

            if (!ctx.IsHtmlResponse())
            {
                await bufferStream.CopyToAsync(ctx.Response.Body);
                return;
            }

            var sr = new StreamReader(bufferStream);
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                if (_options.AppendToHead && str.Contains("<head>"))
                {
                    str = str.Replace(
                        "<head>",
                        "<head>" +_options.Head
                        );
                }
                if (_options.AppendToBody && str.Contains("</body>"))
                {
                    str = str.Replace(
                        "</body>",
                        _options.Body + "</body>");
                }
                await ctx.Response.WriteAsync(str);
            }

            Debug.WriteLine("HtmlAppender OUT");
        }

        protected async Task<Byte[]> GetFromResources(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //var resourceName = "ken.Spikes.Owin.Cornify.assets." + location.Replace("/", "");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                var assemblyData = new Byte[stream.Length];
                await stream.ReadAsync(assemblyData, 0, assemblyData.Length);
                return assemblyData;
            }
        }

        protected async Task<bool> HandledAsset(IOwinContext ctx, PathString assetsPath, string resourcePrefix)
        {
            var path = ctx.Request.Path;
            PathString remainingPath;
            if (path.StartsWithSegments(assetsPath, out remainingPath))
            {
                var asset = GetFromResources(resourcePrefix + remainingPath.Value.Replace("/", ""));
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

           