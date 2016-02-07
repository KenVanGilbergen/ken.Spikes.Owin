using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ken.Spikes.Owin.Cornify
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    
    public class CornifyMiddleWare
    {
        private readonly AppFunc _next;
        private PathString _cornifyAssetsPath = new PathString("/cornify");
        
        public CornifyMiddleWare(AppFunc next)
        {
            if (null == next) throw new ArgumentNullException("next");
            _next = next;
        }

        private async Task<Byte[]> GetFromResources(string location)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ken.Spikes.Owin.Cornify.assets." + location.Replace("/","");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                var assemblyData = new Byte[stream.Length];
                await stream.ReadAsync(assemblyData, 0, assemblyData.Length);
                return assemblyData;
            }
        }

        private async Task<bool> HandledCornifyAsset(IOwinContext ctx)
        {
            var path = ctx.Request.Path;
            PathString remainingPath;
            if (path.StartsWithSegments(_cornifyAssetsPath, out remainingPath))
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

        public async Task Invoke(IDictionary<string, object> environment)
        {
            Debug.WriteLine("Cornify IN");
            
            var ctx = new OwinContext(environment);

            if (await HandledCornifyAsset(ctx)) return;
            
            var realStream = ctx.Response.Body;
            var bufferStream = new MemoryStream();
            ctx.Response.Body = bufferStream;
            
            await _next(environment);

            ctx.Response.Body = realStream;

            bufferStream.Seek(0,SeekOrigin.Begin);
            //await stream.CopyToAsync(ctx.Response.Body);
            var sr = new StreamReader(bufferStream);
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                if (str.Contains("<head>"))
                {
                    //str = str.Replace("<head>", "<head><script type='text/javascript' src='http://www.cornify.com/js/cornify.js'></script>");
                    str = str.Replace(
                        "<head>", 
                        String.Format("<head><script type='text/javascript' src='{0}/cornify.js'></script>", _cornifyAssetsPath)
                        );
                }
                if (str.Contains("</body>"))
                {
                    str = str.Replace("</body>", "<script>(function() { setInterval(function(){ cornify_add(); }, 300); })();</script>");
                }
                await ctx.Response.WriteAsync(str);
            }

            Debug.WriteLine("Cornify OUT");
        }
    }
}

            //app.Use(async (ctx, next) =>
            //{
            //    Debug.WriteLine("IN2");

            //    //ctx.Request.Headers.Remove("If-Modified-Since");
            //    //ctx.Request.Headers.Remove("If-None-Match");

            //    //var oldContext = ctx;
            //    //var fakeContext = new OwinContext(ctx.Environment);
            //    //ctx = fakeContext;
            //    var responseStream = ctx.Response.Body;
            //    var stream = new MemoryStream();
            //    ctx.Response.Body = stream;
            //    await next();
            //    Debug.WriteLine("MIDDLE2 : " + stream.Position + " / " + stream.Length);
            //    ctx.Response.Body = responseStream;
            //    //ctx = oldContext;

            //    stream.Position = 0;
            //    //await stream.CopyToAsync(responseStream);
            //    var sr = new StreamReader(stream);
            //    string str;
            //    while ((str = sr.ReadLine()) != null)
            //    {
            //        if (str.Contains("<head>"))
            //        {
            //            str = str.Replace("<head>", "<head><script type='text/javascript' src='http://www.cornify.com/js/cornify.js'></script>");
            //        }
            //        if (str.Contains("</body>"))
            //        {
            //            str = str.Replace("</body>", "<script>(function() { setInterval(function(){ cornify_add(); }, 2000); })();</script>");
            //        }
            //        await ctx.Response.WriteAsync(str);
            //    }

            //    Debug.WriteLine("OUT2");
            //});

