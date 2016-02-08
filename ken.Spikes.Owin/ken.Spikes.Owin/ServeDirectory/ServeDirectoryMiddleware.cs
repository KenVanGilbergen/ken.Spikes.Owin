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
        private readonly ServeDirectoryMiddlewareOptions _options;

        public ServeDirectoryMiddleware(AppFunc next, ServeDirectoryMiddlewareOptions options)
        {
            if (null == next) throw new ArgumentNullException("next");
            if (null == options) throw new ArgumentNullException("options");
            
            _next = next;
            _options = options;
        }

       public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);
            Debug.WriteLine("Serve IN : " + ctx.Request.Path);

            var path = ctx.Request.Path.Value;
            if (_options.UseDefaultIndex && path.EndsWith("/")) path += "index.html";
            
            var folderPath = path.TrimStart('/').Replace("/","\\");
            var defaultFile = Path.Combine(_options.RootDirectory, folderPath);
            
            ctx.Response.ContentType = path.ToMimeType();

            using (var stream = new FileStream(defaultFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                //    //var buffer = new byte[0x1000];
                //    //int numRead;
                //    //while ((numRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                //    //{
                //    //    await ctx.Response.WriteAsync(buffer,0,numRead, CancellationToken.None);
                //    //}
                await stream.CopyToAsync(ctx.Response.Body);
            }

            //await _next(environment);

            Debug.WriteLine("Serve OUT");
        }
    }
}
