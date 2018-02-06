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
using MIMWebClient.Core.World;
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

    public class ErrorList
    {
        public string name { get; set; }
        public int count { get; set; }
        public string type { get; set; }
    }

    public class QuitList
    {
        public string name { get; set; }
        public int roomId { get; set; }
        public string roomName { get; set; }
    }

    public class PlayerList
    {
        public int id { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        [JsonProperty(PropertyName = "class")]
        public string className { get; set; }
        public string race { get; set; }
        public string gender { get; set; }
        public string lastPlayed { get; set; }
        public string totalHoursPlayed { get; set; }
    }

    public class WhoList: PlayerList
    {
        public string location { get; set; }
        public string idle { get; set; }
        public string playingTime { get; set; }
    }




    public class MonthStat
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
        public string ReturnQuitLocation()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {
                
                var col = db.GetCollection<QuitLocation>("QuitLocation");

                var quitLoc = col.FindAll();

                var quits = new List<QuitList>();

                foreach (var q in quitLoc)
                {
                    var x = new QuitList()
                    {
                        roomId = q.RoomId,
                        roomName = q.RoomName,
                        name = q.PlayerName,
                    };

                    quits.Add(x);
                }
                var json = JsonConvert.SerializeObject(quits);

                return json;
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
                    Name = "Fighter",
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
                    if (player.SelectedClass == "Fighter")
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
        public string GetRaceBreakdown()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Player>("Player");

                var Human = new InfoCount()
                {
                    Name = "Human",
                    Value = 0
                };

                var Elf = new InfoCount()
                {
                    Name = "Elf",
                    Value = 0
                };

                var DarkElf = new InfoCount()
                {
                    Name = "Dark Elf",
                    Value = 0
                };

                var Dwarf = new InfoCount()
                {
                    Name = "Dwarf",
                    Value = 0
                };


                foreach (var player in col.FindAll())
                {
                    if (player.Race == "Human")
                    {
                        Human.Value += 1;

                        continue;
                    }

                    if (player.Race == "Elf")
                    {
                        Elf.Value += 1;

                        continue;
                    }

                    if (player.Race == "Dark Elf")
                    {
                        DarkElf.Value += 1;

                        continue;
                    }

                    if (player.SelectedClass == "Dwarf")
                    {
                        Dwarf.Value += 1;

                        continue;
                    }
                }

                var raceBreakdown = new List<InfoCount>();

                raceBreakdown.Add(Human);
                raceBreakdown.Add(Elf);
                raceBreakdown.Add(DarkElf);
                raceBreakdown.Add(Dwarf);

                var json = JsonConvert.SerializeObject(raceBreakdown);

                return json;
            }
        }

        [System.Web.Http.HttpGet]
        public string GetGenderBreakdown()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Player>("Player");

                var Male = new InfoCount()
                {
                    Name = "Male",
                    Value = 0
                };

                var Female = new InfoCount()
                {
                    Name = "Female",
                    Value = 0
                };
 


                foreach (var player in col.FindAll())
                {
                    if (player.Gender == "Male")
                    {
                        Male.Value += 1;

                        continue;
                    }

                    if (player.Gender == "Female")
                    {
                        Female.Value += 1;

                        continue;
                    }
  
                }

                var genderBreakdown = new List<InfoCount>();

                genderBreakdown.Add(Male);
                genderBreakdown.Add(Female);
 

                var json = JsonConvert.SerializeObject(genderBreakdown);

                return json;
            }
        }

        [System.Web.Http.HttpGet]
        public string GetAllPlayers()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Player>("Player");

                var players = new List<PlayerList>();
          


                foreach (var player in col.FindAll())
                {
                    var x = new PlayerList()
                    {
                        id = player.Id,
                        className = player.SelectedClass,
                        lastPlayed =  Math.Round((DateTime.Now - player.LastLoginTime).TotalDays) + " day(s) ago",
                        level = player.Level,
                        name = player.Name,
                        race = player.Race,
                        gender = player.Gender,
                        totalHoursPlayed = new DateTime(player.TotalPlayTime).Hour + " hour(s)",
                    };

                    players.Add(x);

                }

               
                var json = JsonConvert.SerializeObject(players);

                return json;
            }
        }

        [System.Web.Http.HttpGet]
        public string GetErrors()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Error.Error>("Error");

                var errors = new List<ErrorList>();


                foreach (var err in col.FindAll())
                {
                    var x = new ErrorList()
                    {
                            name = err.ErrorMessage,
                            type = err.MethodName,
                            count = 1
                    };

                    if (errors.Find(y => y.name == x.name) != null)
                    {
                        errors.Find(y => y.name == x.name).count++;
                    }
                    else
                    {
                        errors.Add(x);
                    }
        

                }


                var json = JsonConvert.SerializeObject(errors);

                return json;
            }
        }

   

        [System.Web.Http.HttpGet]
        public string GetWhoList()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = MIMHub._PlayerCache.Values;

                var players = new List<WhoList>();



                foreach (var player in col)
                {
                    var who = new WhoList()
                    {
                        id = player.Id,
                        className = player.SelectedClass,
                        lastPlayed = Math.Round((DateTime.Now - player.LastLoginTime).TotalHours) + " hours(s) ago",
                        level = player.Level,
                        name = player.Name,
                        race = player.Race,
                        location = Startup.ReturnRooms.FirstOrDefault(x => x.areaId == player.AreaId && x.area == player.Area && x.region == player.Region).title ?? "Unknown",
                        playingTime = Math.Round((DateTime.Now - player.LastLoginTime).TotalHours) + " hours(s)",
                        idle = Math.Round((DateTime.Now - player.LastCommandTime).TotalMinutes) + " minutes(s)"

                    };

                    players.Add(who);

                }


                var json = JsonConvert.SerializeObject(players);

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
 

                var averagePlayTime =  (int)col.FindAll().Select(x => x.PlayTime).ToArray().Average();
                var longestPlayTime = (int)col.FindAll().Select(x => x.PlayTime).Max();
                var shortestPlayTime = col.FindAll().ToList().Min(x => x.PlayTime);

 

                var stats = new List<Stats>()
                {
                    new Stats {Stat = "Month", Now = playersThisMonth.Count(), Before = playersLastMonth.Count()},
                    new Stats {Stat = "Week", Now = playersThisWeek, Before = playersLastWeek},
                    new Stats {Stat = "today", Now = playersToday, Before = playersYesterday},
                    new Stats {Stat = "Average Play Time", Now = averagePlayTime, Before = 0},
                    new Stats {Stat = "Longest Play Time", Now = longestPlayTime, Before = 0},
                    new Stats {Stat = "Shortest Play Time", Now = db.GetCollection<Deaths>("Deaths").FindAll().Where(x => x.Type == Player.PlayerTypes.Player.ToString()).Count() },
                };

                return stats;
            }

         
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<MonthStat> SignUpCount(int monthCount)
        {
 
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {
               
                var col = db.GetCollection<Player>("Player");

                var players = col.FindAll().ToList();
                var signups = new List<MonthStat>();
                var months = DateTime.Now.AddMonths(-monthCount);

                for (int i = 1; i <= monthCount; i++)
                {
                    signups.Add(new MonthStat
                    {
                        Month = months.AddMonths(i).ToString("MMM") + " " + months.AddMonths(i).Year,
                        Count = players.Where(x => x.JoinedDate.Month == months.AddMonths(i).Month).ToList().Count
                    });
                }
                
       
                return signups;
            }


        }


        [System.Web.Http.HttpGet]
        public IEnumerable<MonthStat> MobKillCount(int monthCount)
        {

            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Deaths>("Deaths");

                var kills = col.FindAll().ToList();
                var mobDeaths = new List<MonthStat>();
                var months = DateTime.Now.AddMonths(-monthCount);

                for (int i = 1; i <= monthCount; i++)
                {
                    mobDeaths.Add(new MonthStat
                    {
                        Month = months.AddMonths(i).ToString("MMM") + " " + months.AddMonths(i).Year,
                        Count = kills.Where(x => x.Date.Month == months.AddMonths(i).Month).ToList().Count
                    });
                }


                return mobDeaths;
            }


        }
    }
}
