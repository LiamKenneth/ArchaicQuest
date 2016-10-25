using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Mob.Events
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Trainer
    {
        public static void Practice(Player player, Room room, string skillToPractice) { 


            var foundTrainer = FindItem.Trainer(room.mobs);

            if (foundTrainer == null)
            {
                HubContext.SendToClient("There is no one here that can train you.", player.HubGuid);
                return;
            }

            if (skillToPractice == string.Empty)
            {
                ShowSkills(player);
            }
            else
            {
                PracticeSkill(player, foundTrainer, skillToPractice);
            }
           
        }

        public static void ShowSkills(Player player)
        {
            Core.Events.ShowSkills.ShowPlayerSkills(player, string.Empty);
        }

        public static void PracticeSkill(Player player, Player trainer, string skillToPractice)
        {
            var findSkill =
                Core.Events.ShowSkills.GetSkills(player)
                    .FirstOrDefault(x => x.LevelObtained <= player.Level && x.Name.StartsWith(skillToPractice, StringComparison.CurrentCultureIgnoreCase));

         

            if (findSkill == null)
            {
                HubContext.SendToClient(trainer.Name + " says " + "you cannot learn that skill", player.HubGuid);
                return;
            }

            var findPlayerSkill = player.Skills.FirstOrDefault(x => x.Name.Equals(findSkill.Name));

            if (findPlayerSkill == null)
            {
                HubContext.SendToClient("You don't know that skill", player.HubGuid);
                return;
            }

            if (findPlayerSkill.Proficiency == 0.75)
            {
                HubContext.SendToClient(trainer.Name + " says " + "You are already skilled in " + findSkill.Name, player.HubGuid);
                return;
            }

            //take away a practice session
            if (player.Practices == 0)
            {
                HubContext.SendToClient(trainer.Name + " says " + "You don't have any practices left", player.HubGuid);
                return;
            }

            player.Practices -= 1;

            if (player.Practices < 0)
            {
                player.Practices = 0;
            }

            var skill = FindSkillToPractice(player, skillToPractice);
            var practiceGain = Helpers.Rand(0, player.Intelligence*2);
            skill.Proficiency = practiceGain + skill.Proficiency; //what should this be;

            var updatedPlayer = player;



            findPlayerSkill.Proficiency = skill.Proficiency;

            HubContext.SendToClient(trainer.Name + " teaches you " + findSkill.Name, player.HubGuid);
            HubContext.SendToClient("Your " + findSkill.Name + " Skill has increased to " + findPlayerSkill.Proficiency +"%", player.HubGuid);

            if (findPlayerSkill.Proficiency >= skill.MaxProficiency)
            {
                findPlayerSkill.Proficiency = 75;
            }

            if (findPlayerSkill.Proficiency == 75)
            {
                HubContext.SendToClient(trainer.Name + " says " + "You are now skilled in " + findSkill.Name, player.HubGuid);
              
            }

            Cache.updatePlayer(updatedPlayer, player);

            Save.SavePlayer(updatedPlayer);
 

        }


        public static Skill FindSkillToPractice(Player player, string skillToPractice)
        {
            var findSkill = player.Skills.Find(x => x.Name.StartsWith(skillToPractice, StringComparison.CurrentCultureIgnoreCase));

            return findSkill;

        }

        public static void Train(Player player, Room room, string statToTrain)
        {
            var foundTrainer = FindItem.Trainer(room.mobs);

            if (foundTrainer == null)
            {
                HubContext.SendToClient("There is no one here that can train you.", player.HubGuid);
                return;
            }

            if (statToTrain == string.Empty)
            {
                ShowTrain(player, foundTrainer);
            }
            else
            {
                PracticeSkill(player, foundTrainer, statToTrain);
            }
        }

        public static void ShowTrain(Player player, Player trainer)
        {

            HubContext.SendToClient(trainer.Name + " says " + "You can train these attributes", player.HubGuid);

            //show stats that are not maxed
        }

    }
}