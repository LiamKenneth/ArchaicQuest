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
            var model = new createPlayer();
            return View(model);
        }
    }
}