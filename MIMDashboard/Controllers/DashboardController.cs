using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MIMWebClient.Core.PlayerSetup;
using PlayerStats = MIMDashboard.Models.Core.PlayerStats;

/// <summary>
/// http://www.dotnetcurry.com/aspnet/1223/secure-aspnet-web-api-using-tokens-owin-angularjs
/// </summary>
namespace MIMDashboard.Controllers
{
    [AllowAnonymous]
    public class DashboardController : ApiController
    {

            public string Get()
            {

                var stats = new PlayerStats();
                var players = stats.GetNewUsers();
            return players;
            }
        }

}