using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(MIMWebClient.Startup))]

namespace MIMWebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
             //app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}