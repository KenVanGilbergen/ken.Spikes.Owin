using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ken.Spikes.Owin.ServeDirectory
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    
    public class ServeDirectoryMiddleware
    {
        private readonly AppFunc _next;

        private static string GetFullRoot(string root)
        {
            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullRoot = Path.GetFullPath(Path.Combine(applicationBase, root));
            if (!fullRoot.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                fullRoot += Path.DirectorySeparatorChar;
            }
            return fullRoot;
        }

        public ServeDirectoryMiddleware(AppFunc next)
        {
            if (null == next) throw new ArgumentNullException("next");
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);
            Debug.WriteLine("Serve IN : " + ctx.Request.Path);

            var rootDirectory = GetFullRoot("_site");
            Debug.WriteLine(rootDirectory);

            var defaultFile = Path.Combine(rootDirectory, "index.html");
            Debug.WriteLine(defaultFile);

            ctx.Response.ContentType = "text/html";
            using (var stream = new FileStream(defaultFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                //    //var buffer = new byte[0x1000];
                //    //int numRead;
                //    //while ((numRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                //    //{
                //    //    await ctx.Response.WriteAsync(buffer,0,numRead, CancellationToken.None);
                //    //    //await next();
                //    //}
                await stream.CopyToAsync(ctx.Response.Body);
            }

            //await _next(environment);

            //var reading = File.OpenText(defaultFile);
            //string str;
            //while ((str = reading.ReadLine()) != null)
            //{
            //    if (str.Contains("<head>"))
            //    {
            //        str = str.Replace("<head>", "<head><script type='text/javascript' src='http://www.cornify.com/js/cornify.js'></script>");
            //    }
            //    if (str.Contains("</body>"))
            //    {
            //        str = str.Replace("</body>", "<script>(function() { setInterval(function(){ cornify_add(); }, 2000); })();</script>");
            //    }
            //    await ctx.Response.WriteAsync(str);
            //}

            Debug.WriteLine("Serve OUT");
        }
    }
}


            //app.Use(async (ctx, next) =>
            //{
            //    var rootDirectory = GetFullRoot("_site");
            //    Debug.WriteLine(rootDirectory);

            //    var defaultFile = Path.Combine(rootDirectory, "index.html");
            //    Debug.WriteLine(defaultFile);

            //    ctx.Response.ContentType = "text/html";
            //    using (var stream = new FileStream(defaultFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            //    {
            //        //    //var buffer = new byte[0x1000];
            //        //    //int numRead;
            //        //    //while ((numRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            //        //    //{
            //        //    //    await ctx.Response.WriteAsync(buffer,0,numRead, CancellationToken.None);
            //        //    //    //await next();
            //        //    //}
            //        await stream.CopyToAsync(ctx.Response.Body);
            //    }

            //    //var reading = File.OpenText(defaultFile);
            //    //string str;
            //    //while ((str = reading.ReadLine()) != null)
            //    //{
            //    //    if (str.Contains("<head>"))
            //    //    {
            //    //        str = str.Replace("<head>", "<head><script type='text/javascript' src='http://www.cornify.com/js/cornify.js'></script>");
            //    //    }
            //    //    if (str.Contains("</body>"))
            //    //    {
            //    //        str = str.Replace("</body>", "<script>(function() { setInterval(function(){ cornify_add(); }, 2000); })();</script>");
            //    //    }
            //    //    await ctx.Response.WriteAsync(str);
            //    //}
            //});
