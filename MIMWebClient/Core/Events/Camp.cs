using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class Camp
    {
        public static void SetUpCamp(PlayerSetup.Player player, Room.Room room)
        {

            if (room.containsCamp)
            {
                HubContext.Instance.SendToClient("A camp has already been set up here.", player.HubGuid);
                return;
            }

            if (room.items.FirstOrDefault(x => x.name.ToLower().Equals("camp fire")) == null)
            {
                HubContext.Instance.SendToClient("You need to have a camp fire before setting up camp.", player.HubGuid);
                return;
            }

            if (player.Inventory.FirstOrDefault(x => x.name.ToLower().Contains("pot")) == null)
            {
                HubContext.Instance.SendToClient("You need a cooking pot if you want to set up camp.", player.HubGuid);
                return;
            }


            if (player.MovePoints < 10)
            {
                HubContext.Instance.SendToClient("You are too tired to set up camp.", player.HubGuid);
                player.ActiveSkill = null;

                return;
            }



            player.MovePoints -= 10;

            if (player.MovePoints < 0)
            {
                player.MovePoints = 0;
            }

         


            if (player.Inventory.FirstOrDefault(x => x.name.Contains("pot")) != null)
            {
                HubContext.Instance.SendToClient("You place the cooking pot upon the fire.", player.HubGuid);

                room.items.FirstOrDefault(x => x.name.ToLower().Equals("camp fire")).description.room =
                    "A cooking pot is suspended over the camp fire.";

                player.Inventory.Remove(player.Inventory.FirstOrDefault(x => x.name.Contains("pot")));
     

                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} places a pot over the fire.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }
            }

            if (player.Inventory.FirstOrDefault(x => x.name.Contains("bed")) != null)
            {
                HubContext.Instance.SendToClient("You place the bed roll by the fire.", player.HubGuid);

                room.items.FirstOrDefault(x => x.name.ToLower().Equals("bed")).description.room =
                    "A bed roll is layed out by the fire.";

                player.Inventory.Remove(player.Inventory.FirstOrDefault(x => x.name.Contains("pot")));


                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} places a bed roll by the fire.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }
            }


            Score.UpdateUiPrompt(player);


        }
    }
}