using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Hubs;

namespace MIMWebClient.Controllers
{
    public class GameStatsController : ApiController
    {
        /// <summary>
        /// Returns list of logged in players
        /// </summary>
        /// <returns>Returns list of logged in players</returns>
        public IEnumerable<Player> ReturnWhoList()
        {
            return MIMHub._PlayerCache.Values.ToList();
        }
    }
}
