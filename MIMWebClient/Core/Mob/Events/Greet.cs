using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Mob.Events
{
    public class Greet
    {

        public static void GreetMob(PlayerSetup.Player player, Room.Room room, string mob)
        {

            var npc = room.mobs.FirstOrDefault(x => x.Name.StartsWith(mob, StringComparison.CurrentCultureIgnoreCase));

            if (npc == null)
            {
                HubContext.Instance.SendToClient("No one here by the name of " + mob, player.HubGuid);
                return;
            }

            var hasQuest = string.Empty;
            var showIfOnQuest = string.Empty;
            var questComplete = false;
            //greet should enable dialoge between npc and char


            /* check quests */

            if (!string.IsNullOrEmpty(npc.DialogueTree[0].ShowIfOnQuest))
            {
                if (player.QuestLog.FirstOrDefault(x => x.Name.Equals(npc.DialogueTree[0].ShowIfOnQuest)) == null)
                {
                    return;
                }
            }


            foreach (var qlog in player.QuestLog.Where(x => x.QuestGiver == npc.Name))
            {
            
                foreach (var respond in npc.DialogueTree[0].PossibleResponse)
                {

                    if (player.QuestLog.FirstOrDefault(x => x.Name.Equals(respond.DontShowIfOnQuest)) != null)
                    {
                        hasQuest = respond.DontShowIfOnQuest;
                     
                    }
                    else if (player.QuestLog.FirstOrDefault(x => x.Name == respond.ShowIfOnQuest) != null)
                    {
                        showIfOnQuest = respond.ShowIfOnQuest;
                    }
                }
            
            // QuestGiver in the case of lance he is not the quest giver 
                // so maybe properties that set how a quest is completed
                // QuestDo ?wtf
                // QuestItem get item or somthing
                // QuestKill mob
                // add QuestFind mob?
                if (qlog.Completed == false && qlog.QuestGiver != null && qlog.QuestGiver == npc.Name)
                {


                    //   HubContext.Instance.SendToClient(qlog.RewardDialog.Message, player.HubGuid);

                    if (npc.DialogueTree != null && npc.DialogueTree.Count >= 1)
                    {


                        var speak = npc.DialogueTree[0];

                

                        foreach (var quest in player.QuestLog)
                        {

                            speak = npc.DialogueTree.FirstOrDefault(x => x.ShowIfOnQuest.Equals(quest.Name));

                        }

                        if (speak == null)
                        {
                            speak = npc.DialogueTree[0];
                        }

                        HubContext.Instance.AddNewMessageToPage(player.HubGuid, "<span class='sayColor'>" +
                                                                                npc.Name + " says to you \"" +
                                                                                speak.Message + "</span>\"");

                    }




                    return;

                }

                questComplete = true;
                if (!qlog.RewardCollected)
                {


                    HubContext.Instance.SendToClient("<span class='sayColor'>" + npc.Name + " says to you \"" +
                                                     qlog.RewardDialog.Message.Replace("$playerName", player.Name) +
                                                     "\"</span>", player.HubGuid);

                  

                    PlayerSetup.Player.AddItem(player, qlog.RewardItem);

                    if (qlog.RewardSkill != null)
                    {
                        player.Skills.Add(qlog.RewardSkill);

                        HubContext.Instance.SendToClient(
                            $"<p class='roomExits'>You have learnt {qlog.RewardSkill.Name}!</p>", player.HubGuid);


                    }

                    player.Experience += qlog.RewardXp;

                    HubContext.Instance.SendToClient(
                        qlog.Name + " Completed, you are rewarded " + qlog.RewardXp + " experience points.",
                        player.HubGuid);

                    Score.UpdateUiInventory(player);
                    Score.ReturnScoreUI(player);

                  
                    qlog.RewardCollected = true;
                    Score.UpdateUiQlog(player);
                    Save.SavePlayer(player);
                }
            }

            if (npc.DialogueTree != null && npc.DialogueTree.Count > 0)
            {

                if (!questComplete)
                {

                    var speak = npc.DialogueTree[0];

                    HubContext.Instance.AddNewMessageToPage(player.HubGuid,
                        "<span class='sayColor'>" + npc.Name + " says to you \"" + speak.Message + "\"</span>");

                    foreach (var respond in speak.PossibleResponse)
                    {
                        if (!string.IsNullOrEmpty(hasQuest) && hasQuest == respond.DontShowIfOnQuest)
                        {
                            //
                        }
                        else if (!string.IsNullOrEmpty(respond.ShowIfOnQuest) && showIfOnQuest != respond.ShowIfOnQuest)
                        {
                            //
                        }
                        else
                        {
                            var textChoice =
                                "<a class='multipleChoice' href='javascript:void(0)' onclick='$.connection.mIMHub.server.recieveFromClient(\"say " +
                                respond.Response + "\",\"" + player.HubGuid + "\")'>[say, " +
                                respond.Response + "]</a>";
                            HubContext.Instance.AddNewMessageToPage(player.HubGuid, textChoice);
                        }

                    }

                }
                else
                {
                    var message = "Thank you for helping me $playerName.".Replace("$playerName", player.Name);

                    HubContext.Instance.AddNewMessageToPage(player.HubGuid,
                        "<span class='sayColor'>" + npc.Name + " says to you \"" + message + "\"</span>");

                }
            }
            else
            {
                HubContext.Instance.SendToClient(npc.Name + " greets you back", player.HubGuid);
            }

        }
    }
}