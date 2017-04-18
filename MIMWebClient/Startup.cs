using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using MIMWebClient.Core.AI;
using MIMWebClient.Core.Events;

[assembly: OwinStartup(typeof(MIMWebClient.Startup))]

namespace MIMWebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
              app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();

            var roomSetUp = new BreadthFirstSearch();

            Map._roomList = roomSetUp.AssignCoords();
        }
    }
}