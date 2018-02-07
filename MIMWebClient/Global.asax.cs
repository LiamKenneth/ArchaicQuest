using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Core.Update;
using MIMWebClient.Core.World;
using MIMWebClient.Hubs;

namespace MIMWebClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UpdateWorld.Init();
            UpdateWorld.CleanRoom();
            UpdateWorld.UpdateMob();
            PlayerName.GenerateHumanNames();
            var x = Startup.SetMappedRooms;

            foreach (var area in Startup.ReturnRooms)
            {

                var room = new Tuple<string, string, int>(area.region, area.area, area.areaId);

                MIMHub._AreaCache.TryAdd(room, area);

            }
        }
    }
}
