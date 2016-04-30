using MIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIM.Controllers
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin.Security.Provider;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new CreatePlayer();
            return this.View(model);
        }

        [HttpPost]
        public ActionResult CreateCharacter(CreatePlayer createPlayer)
        {

            var context = GlobalHost.ConnectionManager.GetHubContext<MIMHub>();

            //save data
    
            string name = createPlayer.Name;
            return null;
        }
    }

  
}