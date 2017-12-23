using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player.Skills
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class DetectInvis : Skill
    {
        private static bool _taskRunnning = false;
        public static DetectInvis DetectInvisSkill { get; set; }

        public static DetectInvis DetectInvisAb()
        {


            var skill = new DetectInvis
            {
                Name = "Detect Invis",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                ManaCost = 10,
                UsableFromStatus = "Staning",
                Syntax = "Detect Invis"
            };


            var help = new Help
            {
                Syntax = "Detect Invis",
                HelpText = "",
                DateUpdated = "18/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }
        public static void DoDetectInvis(IHubContext context, PlayerSetup.Player player, Room room)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, DetectInvisAb().Name);

            if (hasSkill == false)
            {
                context.SendToClient("You don't know that skill.", player.HubGuid);
                return;
            }

            var canDoSkill = Skill.CanDoSkill(player);

            if (!canDoSkill)
            {
                return;
            }

            HubContext.Instance.SendToClient("You utter invisibilia deprehendere.", player.HubGuid);

         
            foreach (var character in room.players)
            {
                if (character != player)
                {

                    var roomMessage =
                        $"{Helpers.ReturnName(player, character, string.Empty)} utters invisibilia deprehendere.";

                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                }
            }


            var chanceOfSuccess = Helpers.Rand(1, 100);
            var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Detect Invis"));

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {
                HubContext.Instance.SendToClient("Your eyes tingle.", player.HubGuid);
                player.Effects.Add(Effect.DetectInvis(player));
                player.DetectInvis = true;
                Score.UpdateUiAffects(player);
            }
            else
            {

                HubContext.Instance.SendToClient("You lose your concentration .", player.HubGuid);
                PlayerSetup.Player.LearnFromMistake(player, DetectInvisAb(), 250);


                Score.ReturnScoreUI(player);
            }
        }
    }
}
 
