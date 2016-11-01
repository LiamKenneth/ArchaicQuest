using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Controllers
{
    using MIMWebClient.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GenerateName()
        {

            var playerName = PlayerName.GetHumanName();
           
            return Json(playerName, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ValidateUser(string Name, string Password)
        {

            var player = Save.GetPlayer(Name);

            if (player == null)
            {
                return Json("Character does not exist", JsonRequestBehavior.AllowGet);

            }

            var valid = player.Password == Password;

            if (!valid)
            {
                return Json("Password is incorrect", JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Isname_Available(string Name)
        {

            var playerName = Save.GetPlayer(Name);

            if (playerName != null)
            {
                return Json("Character name already taken", JsonRequestBehavior.AllowGet);

            }
          
                return Json(true, JsonRequestBehavior.AllowGet);

            
        }

        [HttpPost]
        public JsonResult CreateCharacter(CreatePlayer createPlayer)
        {

            return this.Json(createPlayer);
        }

        [HttpPost]
        public JsonResult Login(Account login)
        {

            return this.Json(login);
        }
    }
}