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

    public class Foraging : Skill
    {


        public void StartForaging(Player player, Room room)
        {

            if (player.PlayerIsForaging)
            {
                HubContext.Instance.SendToClient("You are already foraging.", player.HubGuid);
                return;
            }

            var foragingAB = player.Skills.FirstOrDefault(x => x.Name.Equals("Foraging"));
            if (player.ActiveSkill != null)
            {

                HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = foragingAB;
            }


            if (player.MovePoints < 10)
            {


                HubContext.Instance.SendToClient("You are too tired to forage", player.HubGuid);
                player.ActiveSkill = null;

                return;
            }



            player.MovePoints -= 10;

            if (player.MovePoints < 0)
            {
                player.MovePoints = 0;
            }

            Score.UpdateUiPrompt(player);


            HubContext.Instance.SendToClient("You begin foraging.",
                player.HubGuid);


            player.PlayerIsForaging = true;
            foreach (var character in room.players)
            {
                if (character != player)
                {

                    var roomMessage =
                        $"{Helpers.ReturnName(player, character, string.Empty)} begins foraging.";

                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                }
            }

            Task.Run(() => DoForaging(player, room));




        }

        private async Task DoForaging(Player player, Room room)
        {

            player.Status = Player.PlayerStatus.Busy;

            await Task.Delay(500);

            HubContext.Instance.SendToClient("You continue searching the area.",
                  player.HubGuid);


            player.PlayerIsForaging = true;
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

            var foragingAB = player.Skills.FirstOrDefault(x => x.Name.Equals("Foraging"));
            double getSkillProf = 0;
            if (foragingAB != null)
            {
                getSkillProf = foragingAB.Points;
            }

            var getItems = room.ForageItems.Where(x => x.ForageRank <= player.ForageRank).ToList();

            if (getItems.Count == 0 && room.terrain == Room.Terrain.City)
            {
                getItems = new ItemContainer();

                if (Helpers.Rand(1, 100) <= 50)
                {
                    getItems.Add(Held.TatteredRag());
              
                    getItems.Add(Held.ScrapMetal());
                }
            }
            else if (room.terrain == Room.Terrain.City)
            {
                if (Helpers.Rand(1, 100) <= 50)
                {
                    getItems.Add(Held.TatteredRag());
               
                    getItems.Add(Held.ScrapMetal());
                }
            }

            var successChance = Helpers.Rand(1, 100);

            if (getSkillProf >= successChance && getItems.Count > 0)
            {

                var YouFound = "You found " +
                               Helpers.ReturnName(null, null, getItems[Helpers.Rand(0, getItems.Count)].name).ToLower() + ".";


                var item = getItems[Helpers.Rand(0, getItems.Count)];

                item.location = Item.Item.ItemLocation.Inventory;
                item.type = Item.Item.ItemType.Food;
                item.hidden = false;
                
                PlayerSetup.Player.AddItem(player, item);

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
                            failMessage = "A bee has stung you on the hand, Ouch!";
                            break;
                        case 2:
                        case 3:
                            failMessage = "You fail to find anything worth taking.";
                            break;
                        default:
                            failMessage = "You don't recognise any of the flora here.";
                            break;
                    }
                }




                HubContext.Instance.SendToClient(failMessage, player.HubGuid);


                if (getSkillProf < 99)
                {

                    HubContext.Instance.SendToClient("You learn from your mistakes and gain 100 experience points",
                        player.HubGuid);

                    var xp = new Experience();
                    player.Experience += 100;

                    xp.GainLevel(player);

                    foragingAB.Points += Helpers.Rand(1, 5);


                    if (foragingAB.Points > 99)
                    {

                        foragingAB.Points = 99;
                    }

                 

                }

                if (foragingAB.Points == 99)
                {
                    HubContext.Instance.SendToClient("You must commit to a craft to progress further",
                        player.HubGuid);
                }



                Score.ReturnScoreUI(player);

            }

            player.ActiveSkill = null;
            player.PlayerIsForaging = false;
        }



    }
}