using MIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIM.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new CreatePlayer();
            return this.View(model);
        }

        [HttpPost]
        public JsonResult CreateCharacter(CreatePlayer createPlayer)
        {
             
            return this.Json(createPlayer);
        }
    }

  
}