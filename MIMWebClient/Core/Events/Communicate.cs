using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MIMWebClient.Controllers;
using MIMWebClient.Core.World;

namespace MIMWebClient.Core.Events
{
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;



    public class Communicate
    {
        public static void Say(string message, Player player, Room room)
        {
            string playerId = player.HubGuid;

            HubContext.Instance.SendToClient($"<span class='sayColor'>You say, \"{message}\"</span>", playerId, null, false, false);

            Score.UpdateUIChannels(player, $"<span class='sayColor'>You say, \"{message}\"</span>", "roomChannelF");

            foreach (var character in room.players)
            {
                if (player != character)
                {

                    var roomMessage = $"<span class='sayColor'>{ Helpers.ReturnName(player, character, string.Empty)} says \"{message}\"</span>";

                    Score.UpdateUIChannels(character, $"<span class='sayColor'>{ Helpers.ReturnName(player, character, string.Empty)} says \"{message}\"</span>", "roomChannelF");

                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                }
            }


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

                    HubContext.Instance.SendToClient("<span class='sayColor'>" + mob.Name + " says to you \"" + response.Replace("$playerName", player.Name) + "\"<span>", playerId);

                    Score.UpdateUIChannels(player, "<span class='sayColor'>" + mob.Name + " says to you \"" + response.Replace("$playerName", player.Name) + "\"<span>", "roomChannelF");

                    //check branch to show responses from
                    var speak = mob.DialogueTree.FirstOrDefault(x => x.Message.Equals(response));

                    //if (speak?.PossibleResponse.Count > 0)
                    //{
                    //    HubContext.Instance.SendToClient("<span class='sayColor'>" +
                    //        mob.Name + " says to you \"anything else?\"</span>", playerId);
                    //}




                    foreach (var respond in speak.PossibleResponse)
                    {

                        var textChoice =
                               "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " +
                               respond.Response + "\",\"" + player.HubGuid + "\")'>[say, " + respond.Response +
                               "]</a>";
                        HubContext.Instance.AddNewMessageToPage(player.HubGuid, textChoice);




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
                            Score.UpdateUiQlog(player);

                            HubContext.Instance.SendToClient("<span class='questColor'>Quest Added:<br />" + quest.Name + " </span> ", playerId);

                            if (GivePrerequisiteItem)
                            {
                                //  Command.ParseCommand("Give 5 gold " + player.Name, mob, room);
                                player.Gold += 5;

                                foreach (var character in room.players)
                                {
                                    if (player != character)
                                    {

                                        var roomMessage = $"{ Helpers.ReturnName(mob, character, string.Empty)}  {quest.PrerequisiteItemEmote}";

                                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                    }
                                }


                                HubContext.Instance.SendToClient("You get 5 gold from " + mob.Name, playerId);
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

            Player recipientPlayer =
                room.players.FirstOrDefault(
                    x => x.Name.StartsWith(playerName, StringComparison.CurrentCultureIgnoreCase));

            if (recipientPlayer != null)
            {
                string recipientName = recipientPlayer.Name;
                HubContext.Instance.SendToClient("You say to " + recipientName + " \"" + actualMessage + "\"", playerId, null, false, false);
                HubContext.Instance.SendToClient(Helpers.ReturnName(player, recipientPlayer, string.Empty) + " says to you \"" + actualMessage + "\"", playerId, recipientName, true, true);

                Score.UpdateUIChannels(player, "You say to " + recipientName + " \"" + actualMessage + "\"", "roomChannelF");
                Score.UpdateUIChannels(recipientPlayer, Helpers.ReturnName(player, recipientPlayer, string.Empty) + " says to you \"" + actualMessage + "\"", "roomChannelF");

            }
            else
            {
                HubContext.Instance.SendToClient("No one here by that name.", playerId, null, false, false);
            }
        }

        public static void Yell(string message, Room room, Player player)
        {

            var rooms = Cache.ReturnRooms().Where(x => x.area.Equals(room.area));
            var yellMessage = $"<span class='yellColor'>You yell \"{message}\"</span>";
            HubContext.Instance.SendToClient(yellMessage, player.HubGuid);

            foreach (var area in rooms)
            {
                foreach (var i in area.players)
                {
                    if (i.Name != player.Name)
                    {
                        var roomMessage = $"<span class='yellColor'>{ Helpers.ReturnName(player, i, string.Empty)} yells \"{message}\"</span>";
                        HubContext.Instance.SendToClient(roomMessage, i.HubGuid);
                    }

                }
            }


        }


        public static void NewbieChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.Instance.SendToClient("<span style='color:red'>[Newbie] " + player.Name + ":</span> " + message, pc.HubGuid);
                Score.UpdateUIChannels(pc, "<span class='newbieChannel' style='color:red'>[Newbie] " + player.Name + ":</span> " + message, "newbieChannelF");
            }


            var discordToSay =   "[Newbie] " + player.Name + " " + message;

            var discordBot = new HomeController();
            discordBot.PostToDiscord(discordToSay);


        }

        public static void GossipChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.Instance.SendToClient("<span style='color:#7CEECE'>[Gossip] " + player.Name + ":</span> " + message, pc.HubGuid);
                Score.UpdateUIChannels(pc, "<span class='gossipChannel' style='color:#7CEECE'>[Gossip] " + player.Name + ":</span> " + message, "gossipChannelF");
            }




        }

        public static void OocChannel(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.NewbieChannel.Equals(true));

            foreach (var pc in players)
            {
                HubContext.Instance.SendToClient("<span style='color:#00AFF0'>[OOC] " + player.Name + ":</span> " + message, pc.HubGuid);
                Score.UpdateUIChannels(pc, "<span class='oocChannel' style='color:#00AFF0'>[OOC] " + player.Name + ":</span> " + message, "oocChannelF");
            }


            var discordToSay = "[OOC] " + player.Name + " " + message;

            var discordBot = new HomeController();
            discordBot.PostToDiscord(discordToSay);

        }

    }
}
