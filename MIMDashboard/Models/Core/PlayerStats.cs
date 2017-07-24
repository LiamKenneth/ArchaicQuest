using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.PlayerSetup;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MIMDashboard.Models.Core
{
    public class PlayerStats
    {
        public string GetNewUsers()
        {

            var db = Db.GetDb();

            var players = db.GetCollection<Player>("Player").AsQueryable<Player>().ToList().ToJson();

            return Regex.Unescape(players);
        }
    }
}