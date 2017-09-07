using MIMWebClient.Core.PlayerSetup;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Ajax.Utilities;

namespace MIMWebClient.Core.Events
{
    using MongoDB.Driver.Linq;

    using Player = MIMWebClient.Core.PlayerSetup.Player;

    public static class Save
    {

        public static void SavePlayer(Player player)
        {

            try
            {
                using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
                {
                    var col = db.GetCollection<Player>("Player");


                    var duration = player.LastLoginTime.Ticks - player.LastCommandTime.Ticks;

                        player.PlayTime = duration;

                        player.TotalPlayTime += duration;

                    col.Upsert(player); 
                }

            }
            catch(Exception e)
            {
              
            }    
          
        }

        public static void LogError(Error.Error error)
        {

            try
            {
                using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
                {

                    var col = db.GetCollection<Error.Error>("Error");

                    col.Insert(Guid.NewGuid(), error);

                }


            }
            catch (Exception e)
            {

            }



        }

       

        public static Player GetPlayer(string name)
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {

                var col = db.GetCollection<Player>("Player");

                var cleanName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

                var returnPlayer = col.FindOne(x => x.Name.Equals(cleanName));

                return returnPlayer;

            }

        }
    }
}
