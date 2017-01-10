using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob.Events
{
    public class Greet
    {

        public static void GreetMob(PlayerSetup.Player player, Room.Room room, string mob)
        {

            var npc = room.mobs.FirstOrDefault(x => x.Name.StartsWith(mob, StringComparison.CurrentCultureIgnoreCase));

            if (npc == null)
            {
                HubContext.SendToClient("No one here by the name of " + mob, player.HubGuid);
                return;
            }

            //greet should enable dialoge between npc and char

            if (npc.DialogueTree != null && npc.DialogueTree.Count > 0)
            {
                var dialogue = npc.DialogueTree.FirstOrDefault(x => x.Id.Equals(npc.Name + 1, StringComparison.CurrentCultureIgnoreCase));

                /* check quests */

                foreach (var qlog in player.QuestLog)
                {
                    // QuestGiver in the case of lance he is not the quest giver 
                    // so maybe properties that set how a quest is completed
                    // QuestDo ?wtf
                    // QuestItem get item or somthing
                    // QuestKill mob
                    // add QuestFind mob?
                    if (qlog.Completed.Equals(false) && qlog.QuestGiver.Equals(npc.Name) || qlog.Completed.Equals(false) && qlog.QuestFindMob != null && qlog.QuestFindMob.Equals(npc.Description))
                    {

                        qlog.Completed = true;
                        player.Experience += qlog.RewardXp;

                        HubContext.SendToClient(qlog.RewardDialog.Message, player.HubGuid);

                        HubContext.SendToClient(qlog.Name + " Completed, you are rewarded " + qlog.RewardXp + " xp", player.HubGuid);

                         

                    }
                }

                if (dialogue != null)
                {
                    HubContext.SendToClient(dialogue.Message, player.HubGuid);
                }
                else
                {
                    HubContext.SendToClient(npc.Name + " greets you back", player.HubGuid);
                }
            }

        }

    }
}