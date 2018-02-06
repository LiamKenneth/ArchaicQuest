using System;
using System.Collections.Generic;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using MIMWebClient.Core;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World;

[assembly: OwinStartup("MIMWebClientConfig", typeof(MIMWebClient.Startup))]

namespace MIMWebClient
{
    public class Startup
    {

        private static List<Room> _listOfRooms;
        private static Dictionary<string, Action> _commands;

        public static List<Room> ReturnRooms
        {
            get
            {
                if (_listOfRooms == null)
                {
                    _listOfRooms = Areas.ListOfRooms();
                }

                return _listOfRooms;
            }
        }

 
        public void Configuration(IAppBuilder app)
        {
              app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}