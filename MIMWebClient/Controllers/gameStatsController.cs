using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Http;
using System.Web.Mvc;
using LiteDB;
using MIMWebClient.Core.Loging;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Core.Util;
using MIMWebClient.Hubs;
using Newtonsoft.Json;

namespace MIMWebClient.Controllers
{

    public class Stats
    {
        public string Stat { get; set; }
        public int Now { get; set; }
        public int Before { get; set; }
    }

    public class InfoCount
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }


    public class UnfinshedQuest
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }


    public class SignUps
    {
        public string Month { get; set; }
        public int Count { get; set; }  
    }

    public class GameStatsController : ApiController
    {
        ///// <summary>
        ///// Returns list of logged in players
        ///// </summary>
        ///// <returns>Returns list of logged in players</returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<Player> ReturnWhoList()
        {
            return MIMHub._PlayerCache.Values.ToList();
        }

        ///// <summary>
        ///// Returns list of logged in players
        ///// </summary>
        ///// <returns>Returns list of logged in players</returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<QuitLocation> ReturnQuitLocation()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {
                
                var col = db.GetCollection<QuitLocation>("QuitLocation");

                var quitLoc = col.FindAll();
                
                return quitLoc;
            }
        }

        [System.Web.Http.HttpGet]
        public string GetClassBreakdown()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Player>("Player");

                var Warrior = new InfoCount()
                {
                    Name = "Warrior",
                    Value = 0
                };

                var Cleric = new InfoCount()
                {
                    Name = "Cleric",
                    Value = 0
                };

                var Thief = new InfoCount()
                {
                    Name = "Thief",
                    Value = 0
                };

                var Mage = new InfoCount()
                {
                    Name = "Mage",
                    Value = 0
                };


                foreach (var player in col.FindAll())
                {
                    if (player.SelectedClass == "Warrior")
                    {
                        Warrior.Value += 1;

                        continue;
                    }

                    if (player.SelectedClass == "Cleric")
                    {
                        Cleric.Value += 1;

                        continue;
                    }

                    if (player.SelectedClass == "Mage")
                    {
                        Mage.Value += 1;

                        continue;
                    }

                    if (player.SelectedClass == "Thief")
                    {
                        Thief.Value += 1;

                        continue;
                    }
                }

                var classBreakdown = new List<InfoCount>();

                classBreakdown.Add(Warrior);
                classBreakdown.Add(Cleric);
                classBreakdown.Add(Mage);
                classBreakdown.Add(Thief);

               var json = JsonConvert.SerializeObject(classBreakdown);

                return json;
            }
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<Deaths> ReturnDeaths(string type)
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {
            
                if (type == "mob")
                {
                    var deaths = db.GetCollection<Deaths>("Deaths").FindAll().Where(x => x.Type == Player.PlayerTypes.Mob.ToString());
                    return deaths;
                }
                else
                {
                    var deaths = db.GetCollection<Deaths>("Deaths").FindAll().Where(x => x.Type == Player.PlayerTypes.Player.ToString());
                    return deaths;
                }
                          
            }
        }

 

        /// <summary>
        /// Returns list of logged in players
        /// </summary>
        /// <returns>Returns list of logged in players</returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<Stats> NewPlayers()
        {
    
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
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

        [System.Web.Http.HttpGet]
        public IEnumerable<SignUps> SignUpCount(int monthCount)
        {
 
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {
               
                var col = db.GetCollection<Player>("Player");

                var players = col.FindAll().ToList();
                var signups = new List<SignUps>();
                var months = DateTime.Now.AddMonths(-monthCount);

                for (int i = 1; i <= monthCount; i++)
                {
                    signups.Add(new SignUps
                    {
                        Month = months.AddMonths(i).ToString("MMM") + " " + months.AddMonths(i).Year,
                        Count = players.Where(x => x.JoinedDate.Month == months.AddMonths(i).Month).ToList().Count
                    });
                }
                
       
                return signups;
            }


        }
    }
}
