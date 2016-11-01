using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Core.Update;

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
        }
    }
}
