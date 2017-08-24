using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiteDB;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Core.Util;
using MIMWebClient.Hubs;

namespace MIMWebClient.Controllers
{

    public class Stats
    {
        public string Stat { get; set; }
        public int Now { get; set; }
        public int Before { get; set; }
    }

    public class GameStatsController : ApiController
    {
        ///// <summary>
        ///// Returns list of logged in players
        ///// </summary>
        ///// <returns>Returns list of logged in players</returns>
        //[Route("whoList")]
        [HttpGet]
        public IEnumerable<Player> ReturnWhoList()
        {
            return MIMHub._PlayerCache.Values.ToList();
        }


        /// <summary>
        /// Returns list of logged in players
        /// </summary>
        /// <returns>Returns list of logged in players</returns>
        [HttpGet]
        public IEnumerable<Stats> NewPlayers()
        {

      
            using (var db = new LiteDatabase(ConfigurationManager.AppSettings["database"]))
            {
                var thisWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                var col = db.GetCollection<Player>("Player");

                var playersThisMonth = col.FindAll().Where(x => x.JoinedDate.Month == DateTime.Now.Month).ToList();
                var playersLastMonth = col.FindAll().Where(x => x.JoinedDate.Month < DateTime.Now.Month).ToList();

                var playersThisWeek =  playersThisMonth.Where(x => x.JoinedDate.Date >= thisWeek && x.JoinedDate <= thisWeek.AddDays(7)).ToList().Count();
                var playersLastWeek = playersThisMonth.Where(x => x.JoinedDate.Date <= thisWeek && x.JoinedDate >= thisWeek.AddDays(-7)).ToList().Count();

                var playersToday= playersThisMonth.Where(x => x.JoinedDate.Date == DateTime.Today).ToList().Count();
                var playersYesterday = playersThisMonth.Where(x => x.JoinedDate.Date == DateTime.Today.AddDays(-1)).ToList().Count();


                var averagePlayTime = col.FindAll().ToList().Average(x => x.PlayTime);
                var longestPlayTime = col.FindAll().ToList().Max(x => x.PlayTime);
                var shortestPlayTime = col.FindAll().ToList().Min(x => x.PlayTime);
                var stats = new List<Stats>()
                {
                    new Stats {Stat = "Month", Now = playersThisMonth.Count(), Before = playersLastMonth.Count()},
                    new Stats {Stat = "Week", Now = playersThisWeek, Before = playersLastWeek},
                    new Stats {Stat = "today", Now = playersToday, Before = playersYesterday},
                    new Stats {Stat = "Average Play Time", Now = (int)averagePlayTime, Before = 0},
                    new Stats {Stat = "Longest Play Time", Now = (int)longestPlayTime, Before = 0},
                    new Stats {Stat = "Shortest Play Time", Now = (int)shortestPlayTime, Before = 0},
                };

                return stats;
            }

         
        }
    }
}
