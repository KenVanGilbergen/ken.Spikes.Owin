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
            
            app.UseWelcomePage("/");
        }
    }
}
