using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Update
{

    using MIMWebClient.Core.PlayerSetup;

    public static class RestoreVitals
    {

        public static void UpdatePlayers()
        {
            var context = HubContext.getHubContext;
            var players = Cache.ReturnPlayers();

            if (players.Count == 0)
            {
                return;
            }

            foreach (var player in players)
            {

                UpdateHp(player, context);
                UpdateMana(player, context);
                UpdateEndurance(player, context);

            }
        }

        public static void UpdateRooms()
        {
            var context = HubContext.getHubContext;
            var rooms = Cache.ReturnRooms();

            if (rooms.Count == 0)
            {
                return;
            }

            foreach (var room in rooms)
            {

                foreach (var mob in room.mobs)
                {
                    UpdateHp(mob, context);
                    UpdateMana(mob, context);
                    UpdateEndurance(mob, context);
                }

               // add missing items
               // add mob if found in corpse list

            }
        }



        public static void UpdateHp(PlayerSetup.Player player, IHubContext context)
        {
            if (player.HitPoints <= player.MaxHitPoints)
            {

                var die = new Helpers();
                var maxGain = player.Constitution;

                if (player.Status == Player.PlayerStatus.Fighting)
                {
                    maxGain = maxGain / 2;
                }

                if (player.Status == Player.PlayerStatus.Sleeping)
                {
                    maxGain = maxGain * 2;
                }


                if (player.Status == Player.PlayerStatus.Resting)
                {
                    maxGain = (maxGain * 2) / 2;
                }


                player.HitPoints += die.dice(1, 1, maxGain);

                if (player.HitPoints > player.MaxHitPoints)
                {
                    player.HitPoints = player.MaxHitPoints;
                }

                if (player.Type == Player.PlayerTypes.Player)
                {
                    context.Clients.Client(player.HubGuid).updateStat(player.HitPoints, player.MaxHitPoints, "hp");
                }



            }

        }

        public static void UpdateMana(PlayerSetup.Player player, IHubContext context)
        {
            if (player.ManaPoints <= player.MaxHitPoints)
            {

                var die = new Helpers();
                var maxGain = player.Intelligence;

                if (player.Status == Player.PlayerStatus.Fighting)
                {
                    maxGain = maxGain / 2;
                }

                if (player.Status == Player.PlayerStatus.Sleeping)
                {
                    maxGain = maxGain * 2;
                }


                if (player.Status == Player.PlayerStatus.Resting)
                {
                    maxGain = (maxGain * 2) / 2;
                }

                player.ManaPoints += die.dice(1, 1, maxGain);

                if (player.ManaPoints > player.MaxHitPoints)
                {
                    player.ManaPoints = player.MaxHitPoints;
                }


                if (player.Type == Player.PlayerTypes.Player)
                {
                    context.Clients.Client(player.HubGuid).updateStat(player.ManaPoints, player.MaxManaPoints, "mana");

                }
            }

        }

        public static void UpdateEndurance(PlayerSetup.Player player, IHubContext context)
        {
            if (player.MovePoints <= player.MaxMovePoints)
            {

                var die = new Helpers();
                var maxGain = player.Dexterity;

                if (player.Status == Player.PlayerStatus.Fighting)
                {
                    maxGain = maxGain / 2;
                }

                if (player.Status == Player.PlayerStatus.Sleeping)
                {
                    maxGain = maxGain * 2;
                }


                if (player.Status == Player.PlayerStatus.Resting)
                {
                    maxGain = (maxGain * 2) / 2;
                }

                player.MovePoints += die.dice(1, 1, maxGain);

                if (player.MovePoints > player.MaxMovePoints)
                {
                    player.MovePoints = player.MaxMovePoints;
                }


                if (player.Type == Player.PlayerTypes.Player)
                {

                    context.Clients.Client(player.HubGuid).updateStat(player.MovePoints, player.MaxMovePoints, "endurance");
                }
            }

        }


    }
}