using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ken.Spikes.Owin
{
    public static class IOwinContextExtensions
    {
        public static bool IsHtmlRequest(this IOwinContext ctx)
        {
            string[] contentType;
            if (ctx.Request.Headers.TryGetValue("content-type", out contentType))
            {
                return ("text/html" == contentType.First());
            }
            return false;
        }

        public static bool IsHtmlResponse(this IOwinContext ctx)
        {
            return ("text/html" == ctx.Response.ContentType);
        }
    }
}
