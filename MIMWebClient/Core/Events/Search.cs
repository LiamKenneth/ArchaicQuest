using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.World.Items.MiscEQ.Held;

namespace MIMWebClient.Core.Player.Skills
{

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using System.Threading.Tasks;

    public class Search : Skill
    {


        public void StartSearching(Player player, Room room)
        {
            var AB = player.Skills.FirstOrDefault(x => x.Name.Equals("Search"));

            if (AB == null)
            {
                HubContext.Instance.SendToClient("You don't know how to search.", player.HubGuid);
                return;
            };

            if (player.ActiveSkill == AB )
            {
                HubContext.Instance.SendToClient("You are already searching.", player.HubGuid);
                return;
            }
        
            if (player.ActiveSkill != null)
            {

                HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }

            player.ActiveSkill = AB;


            if (player.MovePoints < 10)
            {


                HubContext.Instance.SendToClient("You are too tired to search", player.HubGuid);
                player.ActiveSkill = null;

                return;
            }



            player.MovePoints -= 10;

            if (player.MovePoints < 0)
            {
                player.MovePoints = 0;
            }

            Score.UpdateUiPrompt(player);


            HubContext.Instance.SendToClient("You begin searching.",
                player.HubGuid);

            foreach (var character in room.players)
            {
                if (character != player)
                {

                    var roomMessage =
                        $"{Helpers.ReturnName(player, character, string.Empty)} begins searching.";

                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                }
            }

            Task.Run(() => DoSearching(player, room));




        }

        private async Task DoSearching(Player player, Room room)
        {

            player.Status = Player.PlayerStatus.Busy;

            await Task.Delay(500);

            HubContext.Instance.SendToClient("You continue searching the area.",
                  player.HubGuid);


            foreach (var character in room.players)
            {
                if (character != player)
                {

                    var roomMessage =
                        $"{Helpers.ReturnName(player, character, string.Empty)} continues searching the area.";

                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                }
            }

            await Task.Delay(500);

            var AB = player.Skills.FirstOrDefault(x => x.Name.Equals("Search"));
            double getSkillProf = 0;
            if (AB != null)
            {
                getSkillProf = AB.Proficiency / (double)95 * 100;
            }

            var getItems = room.items.Where(x => x.isHiddenInRoom).ToList();

            

            var successChance = Helpers.Rand(1, 100);

            if (getSkillProf >= successChance && getItems.Count > 0)
            {

                var YouFound = "You found " +
                               Helpers.ReturnName(null, null, getItems[Helpers.Rand(0, getItems.Count)].name).ToLower() + ".";


                var item = getItems[Helpers.Rand(0, getItems.Count)];

                item.location = Item.Item.ItemLocation.Inventory;
                item.isHiddenInRoom = false;


                player.Inventory.Add(item);


                HubContext.Instance.SendToClient(YouFound, player.HubGuid);

            }

            else
            {

                var failMessage = "";

                if (room.terrain == Room.Terrain.City)
                {

                    failMessage = "You fail to find anything.";


                }
                else
                {
                    switch (Helpers.Rand(1, 4))
                    {
                        case 1:
                            failMessage = "You search quickly but fail to find anything.";
                            break;
                        case 2:
                        case 3:
                            failMessage = "You fail to find anything.";
                            break;
                        default:
                            failMessage = "You didn't find anything from your search.";
                            break;
                    }
                }




                HubContext.Instance.SendToClient(failMessage, player.HubGuid);


                if (getSkillProf < 95)
                {

                    HubContext.Instance.SendToClient("You learn from your mistakes and gain 100 experience points",
                        player.HubGuid);

                    var xp = new Experience();
                    player.Experience += 100;

                    xp.GainLevel(player);

                    AB.Proficiency += Helpers.Rand(1, 5);



                }

                Score.ReturnScoreUI(player);

            }

            player.ActiveSkill = null;

            Score.UpdateUiInventory(player);
        }



    }
}