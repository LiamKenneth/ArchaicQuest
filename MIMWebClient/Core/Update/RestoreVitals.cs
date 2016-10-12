using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Update
{
    public static class RestoreVitals
    {

        public static void UpdateHp()
        {
            var players = Cache.ReturnPlayers();

            if (players.Count == 0)
            {
                return;
            }

            foreach (var player in players)
            {
                if (player.HitPoints >= player.MaxHitPoints) continue;

                var die = new Helpers();
                var maxGain = player.Constitution * 2;

                player.HitPoints += die.dice(1, 1, maxGain);

                if (player.HitPoints > player.MaxHitPoints)
                {
                    player.HitPoints = player.MaxHitPoints;
                }

                var context = HubContext.getHubContext;
                context.Clients.Client(player.HubGuid).updateStat(player.HitPoints, player.MaxHitPoints, "hp");
            }
        }

      

    }
}