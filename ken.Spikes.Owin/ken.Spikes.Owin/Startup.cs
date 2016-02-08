using System.Diagnostics;
using ken.Spikes.Owin.Cornify;
using ken.Spikes.Owin.KonamiCode;
using Owin;

namespace ken.Spikes.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #if DEBUG
            app.UseErrorPage();
            #endif
            app.UseWelcomePage("/welcome");

            app.Use(async (ctx, next) =>
            {
                Debug.WriteLine("IN - [{0}] {1}{2}", ctx.Request.Method, ctx.Request.Path, ctx.Request.QueryString);
                await next();
                if (ctx.IsHtmlResponse()) await ctx.Response.WriteAsync("Added at bottom will be moved into body by good browser");
                Debug.WriteLine("OUT - " + ctx.Response.ContentLength);
            });

            app.UseDebugMiddleware(new DebugMiddlewareOptions
            {
                OnIncomingRequest = ctx =>
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    ctx.Environment["DebugStopwatch"] = watch;
                },
                OnOutgoingRequest = ctx =>
                {
                    var watch = (Stopwatch)ctx.Environment["DebugStopwatch"];
                    watch.Stop();
                    Debug.WriteLine("Request took: " + watch.ElapsedMilliseconds + " ms");
                }
            });

            app.Map("/mapped", map => map.Run(ctx =>
                {
                    ctx.Response.ContentType = "text/html";
                    return ctx.Response.WriteAsync("<html><body>mapped</body></html>");
                }));
           
            //app.UseBeanMiddleware();
            //app.Use(async (ctx, next) =>
            //{
            //    if (ctx.Authentication.User != null &&
            //        ctx.Authentication.User.Identity != null &&
            //        ctx.Authentication.User.Identity.IsAuthenticated)
            //    {
            //        await next();
            //    }
            //    else
            //    {
            //        ctx.Authentication.Challenge(AuthenticationTypes.Federation);
            //    }
            //});

            app.UseCornifyMiddleware(new CornifyMiddlewareOptions { Autostart = false });
            app.UseKonamiCodeMiddleware(new KonamiCodeMiddlewareOptions { Action = "setInterval(function(){ cornify_add(); },500);" });

            app.UseServeDirectoryMiddleware();
        }
    }
}


//app.UseFileServer(new FileServerOptions()
//{
//    RequestPath = PathString.Empty,
//    FileSystem = new PhysicalFileSystem(@".\_site"),
//    EnableDefaultFiles = true,
//    EnableDirectoryBrowsing = false,
//});

// Only serve files requested by name.
// app.UseStaticFiles("/_site");
            
// Turns on static files, directory browsing, and default files.
//app.UseFileServer(new FileServerOptions()
//{
//    RequestPath = new PathString("/public"),
//    EnableDirectoryBrowsing = true,
//});

//// Browse the root of your application (but do not serve the files).
//// NOTE: Avoid serving static files from the root of your application or bin folder,
//// it allows people to download your application binaries, config files, etc..
//app.UseDirectoryBrowser(new DirectoryBrowserOptions()
//{
//    RequestPath = new PathString("/src"),
//    FileSystem = new PhysicalFileSystem(@""),
//});

//app.Use(async (ctx, next) => {
//    await ctx.Response.WriteAsync("Hello World");
//});

//// Configure the db context, user manager and signin manager to use a single instance per request
//app.CreatePerOwinContext(ApplicationDbContext.Create);
//app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
//app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

//// Enable the application to use a cookie to store information for the signed in user
//// and to use a cookie to temporarily store information about a user logging in with a third party login provider
//// Configure the sign in cookie
//app.UseCookieAuthentication(new CookieAuthenticationOptions
//{
//    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
//    LoginPath = new PathString("/Account/Login"),
//    Provider = new CookieAuthenticationProvider
//    {
//        // Enables the application to validate the security stamp when the user logs in.
//        // This is a security feature which is used when you change a password or add an external login to your account.  
//        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
//            validateInterval: TimeSpan.FromMinutes(30),
//            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
//    }
//});            
//app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
// app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

// Enables the application to remember the second login verification factor such as phone or email.
// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
// This is similar to the RememberMe option when you log in.
// app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
// Uncomment the following lines to enable logging in with third party login providers
//app.UseMicrosoftAccountAuthentication(
//    clientId: "",
//    clientSecret: "");
//app.UseTwitterAuthentication(
//   consumerKey: "",
//   consumerSecret: "");
//app.UseFacebookAuthentication(
//   appId: "",
//   appSecret: "");
//app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
//{
//    ClientId = "",
//    ClientSecret = ""
//});


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
