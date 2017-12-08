using System.Linq;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Events
{
    using System.Text;
    using System.Web.Helpers;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    using MIMWebClient.Core.Room;

    using MongoDB.Bson;

    using PlayerSetup;

    class Score
    {

        public static void ReturnScore(Player playerData)
        {
            string scoreTest = "Score:\r\n Name: " + playerData.Name + " Race: " + playerData.Race;

            HubContext.Instance.AddNewMessageToPage(playerData.HubGuid, scoreTest);
        }

        public static void ReturnScoreUI(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                HubContext.Instance.UpdateScore(playerData);

            }
        }

        public static void UpdateUiPrompt(Player playerData)
        {

            if (playerData.HubGuid != null)
            {

                var context = HubContext.Instance;
                context.UpdateStat(playerData.HubGuid, playerData.HitPoints, playerData.MaxHitPoints, "hp");
                context.UpdateStat(playerData.HubGuid, playerData.ManaPoints, playerData.MaxManaPoints, "mana");
                context.UpdateStat(playerData.HubGuid, playerData.MovePoints, playerData.MaxMovePoints, "endurance");
                context.UpdateStat(playerData.HubGuid, playerData.Experience, playerData.ExperienceToNextLevel, "tnl");

            }
        }

        public static void UpdateUiInventory(Player playerData)
        {
            if (playerData?.HubGuid != null)
            {
                HubContext.Instance.UpdateInventory(playerData.HubGuid, ItemContainer.List(playerData.Inventory.Where(x => x.location == Item.Item.ItemLocation.Inventory && x.type != Item.Item.ItemType.Gold), true));

            }

        }

        public static void UpdateUiEquipment(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                HubContext.Instance.UpdateEquipment(playerData.HubGuid, Equipment.DisplayEq(playerData, playerData.Equipment));
            }
             
        }

        public static void UpdateUiQlog(Player playerData)
        {

            if (playerData.HubGuid != null)
            {

                var qlog = new StringBuilder();

                qlog.Append("<div><p>Your current quests:</p></div>");

                foreach (var quest in playerData.QuestLog)
                {
                    if (!quest.Completed)
                    {
                        qlog.Append("<p>" + quest.Name + "</p><br />");
                        qlog.Append("<p>" + quest.Description + "</p>");
                    }
                }

                HubContext.Instance.UpdateQuestLog(playerData.HubGuid, qlog.ToString());

            }

        }

        public static void UpdateUiAffects(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                var context = HubContext.Instance;


                if (playerData.Effects != null)
                {

                    if (playerData.Effects.Count > 0)
                    {
                        var aff = new StringBuilder();

                        aff.Append("<li><p>You are affected by the following affects:</p></li>");
                       

                        foreach (var affect in playerData.Effects)
                        {
                            aff.Append("<li>" + affect.Name + " (" + affect.Duration + ") ticks</li> ");
                            
                        }

                        context.UpdateEffects(playerData.HubGuid, aff.ToString());
                    }
                    else
                    {
                        context.UpdateEffects(playerData.HubGuid, "You are not affected by anything.");
                    }
                }
                else
                {
                    context.UpdateEffects(playerData.HubGuid, "You are not affected by anything.");
                }

              

            }

        }

        public static void UpdateUIChannels(Player playerData, string text, string className)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            HubContext.Instance.UpdateUiChannels(playerData.HubGuid, text, className);

        }

        public static void UpdateUiRoom(Player playerData, string room)
        {

            if (playerData.HubGuid != null)
            {
                //var room = new Room.Room();
                //var currentRoom = p
                HubContext.Instance.UpdateUIRoom(playerData.HubGuid, room);
                //context.Clients.Client(playerData.HubGuid).updateRoom(playerData.Inventory);
            }
        }

        public static void UpdateUiMap(Player playerData, int roomId, string area, string region, int zindex)
        {

            if (playerData.HubGuid != null)
            {
                //var room = new Room.Room();
                //var currentRoom = p
                HubContext.Instance.UpdateUiMap(playerData.HubGuid, roomId, area, region, zindex);
                //context.Clients.Client(playerData.HubGuid).updateRoom(playerData.Inventory);
            }
        }


        public static void UpdateDescription(Player playerData, string description )
        {

            if (playerData.HubGuid != null)
            {


                playerData.Description = description;

 
                //var room = new Room.Room();
                //var currentRoom = p
                HubContext.Instance.UpdateUiDescription(playerData.HubGuid, description);
                //context.Clients.Client(playerData.HubGuid).updateRoom(playerData.Inventory);
            }
        }
    }
}
