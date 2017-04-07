using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Events
{
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;



    public class Communicate
    {
        public static void Say(string message, Player player, Room room)
        {
            string playerId = player.HubGuid;

            HubContext.SendToClient("You say " + message, playerId, null, false, false);
            HubContext.broadcastToRoom(player.Name + " says " + message, room.players, player, true);


            //check npc response
            foreach (var mob in room.mobs)
            {
                var response = string.Empty;
                var hasQuest = false;
                var questId = 0;
                var GivePrerequisiteItem = false;

                if (mob.Dialogue != null)
                {
                    foreach (var dialogue in mob.Dialogue)
                    {
                        foreach (var keyword in dialogue.Keyword)
                        {
                            if (message.Contains(keyword))
                            {
                                response = dialogue.Response;

                            }
                        }

                    }
                }



                if (response == string.Empty)
                {
                    if (mob.DialogueTree != null)
                    {
                        foreach (var tree in mob.DialogueTree)
                        {
                            if (message.Equals(tree.MatchPhrase))
                            {
                                response = tree.Message;
                                if (tree.GiveQuest != null) hasQuest = (bool)tree.GiveQuest;
                                if (tree.GivePrerequisiteItem != null)
                                    GivePrerequisiteItem = (bool)tree.GivePrerequisiteItem;
                                if (tree.QuestId != null) questId = (int)tree.QuestId;


                            }
                        }
                    }
                }

                if (response != String.Empty)
                {
                    Thread.Sleep(120); // hack, sometimes the responses calls before the questions??

                    if (!hasQuest)
                    {
                        var quest = mob.Quest.FirstOrDefault(x => x.Id.Equals(questId));
                        HubContext.SendToClient(
           mob.Name + " says to you " + quest.AlreadyOnQuestMessage, playerId,
           null, true);
                        return;
                    }


                    HubContext.SendToClient(
                       mob.Name + " says to you " + response.Replace("$playerName", player.Name), playerId,
                       null, true);


                    //check branch to show responses from
                    var speak = mob.DialogueTree.FirstOrDefault(x => x.Message.Equals(response));

                    if (speak?.PossibleResponse.Count > 0)
                    {
                        HubContext.SendToClient(
                            mob.Name + " says to you anything else?", playerId,
                            null, true);
                    }



                    var i = 1;
                    foreach (var respond in speak.PossibleResponse)
                    {
                        if (player.QuestLog != null && respond.QuestId > 0)
                        {
                            var doneQuest =
                                player.QuestLog.FirstOrDefault(
                                    x => x.Id.Equals(respond.QuestId) && x.QuestGiver.Equals(mob.Name) && x.Completed.Equals(true));

                            if (doneQuest == null)
                            {
                                var textChoice =
                                    "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " +
                                    respond.Response + "\",\"" + player.HubGuid + "\")'>" + i + ". " + respond.Response +
                                    "</a>";
                                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(textChoice);
                                i++;
                            }
                            else
                            {
                                var textChoice =
                                    "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " +
                                    respond.Response + "\",\"" + player.HubGuid + "\")'>" + i + ". " + respond.Response +
                                    "</a>";
                                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(textChoice);
                                i++;
                            }
                        }
                    }


                    if (hasQuest)
                    {
                        //find quest
                        var quest = mob.Quest.FirstOrDefault(x => x.Id.Equals(questId));

                        var playerHasQuest = player.QuestLog.FirstOrDefault(x => quest != null && x.Name.Equals(quest.Name));

                        if (playerHasQuest == null)
                        {
                            //to player log
                            player.QuestLog.Add(quest);

                            HubContext.SendToClient("<h5>Quest Added: </h5> "+  quest.Name, playerId);

                            if (GivePrerequisiteItem)
                            {
                                //  Command.ParseCommand("Give 5 gold " + player.Name, mob, room);
                                player.Gold += 5;
                                HubContext.broadcastToRoom(mob.Name + " " + quest.PrerequisiteItemEmote,
                                    room.players, mob);
                                HubContext.SendToClient("You get 5 gold from " + mob.Name, playerId);
                            }
                        }

                    }

                }


                else
                {
                    //generic responses?
                }

                if (mob.EventOnComunicate.Count > 0)
                {
                    var triggerEvent = mob.EventOnComunicate.FirstOrDefault(x => x.Value.Equals("yes"));

                    Event.ParseCommand(triggerEvent.Key, player, mob, room, "yes", "player");
                }
            }



        }

        public static void SayTo(string message, Room room, Player player)
        {
            string playerName = message;
            string actualMessage = string.Empty;
            int indexOfSpaceInUserInput = message.IndexOf(" ", StringComparison.Ordinal);

            if (indexOfSpaceInUserInput > 0)
            {
                playerName = message.Substring(0, indexOfSpaceInUserInput);

                if (indexOfSpaceInUserInput != -1)
                {
                    actualMessage = message.Substring(indexOfSpaceInUserInput, message.Length - indexOfSpaceInUserInput).TrimStart();
                    // message is everythign after the 1st space
                }
            }

            string playerId = player.HubGuid;

            Player recipientPlayer = (Player)ManipulateObject.FindObject(room, player, "", playerName, "all");

            if (recipientPlayer != null)
            {
                string recipientName = recipientPlayer.Name;
                HubContext.SendToClient("You say to " + recipientName + " " + actualMessage, playerId, null, false, false);
                HubContext.SendToClient(Helpers.ReturnName(player, recipientPlayer, string.Empty) + " says to you " + actualMessage, playerId, recipientName, true, true);



            }
            else
            {
                HubContext.SendToClient("No one here by that name", playerId, null, false, false);
            }
        }


        public static void NewbieChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.SendToClient("Newbie: " + player.Name + " says " + message, pc.HubGuid);
            }

        }

        public static void GossipChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.SendToClient("Gossip: " + player.Name + " says " + message, pc.HubGuid);
            }

        }

        public static void OocChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.SendToClient("OOC: " + player.Name + " says " + message, pc.HubGuid);
            }

        }

    }
}
