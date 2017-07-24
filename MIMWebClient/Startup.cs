using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartup("MIMWebClientConfig", typeof(MIMWebClient.Startup))]

namespace MIMWebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
              app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}