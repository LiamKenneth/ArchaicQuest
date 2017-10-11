using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.World.Items.Consumables.Drinks;
using MIMWebClient.Core.World.Items.MiscEQ.Light;

namespace MIMWebClient.Core.Player.Skills
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class CreateSpring : Skill
    {
        private static bool _taskRunnning = false;
        public static Skill CreateSpringSkill { get; set; }

        public static void StartCreateSpring(Player player, Room room)
        {

            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, CreateSpringAb().Name);

            if (hasSpell == false)
            {
                HubContext.Instance.SendToClient("You don't know that spell.", player.HubGuid);
                return;
            }

            var canDoSkill = Skill.CanDoSkill(player);

            if (!canDoSkill)
            {
                return;
            }

            if (!_taskRunnning)
            {

                if (player.ManaPoints < CreateSpringAb().ManaCost)
                {
                    HubContext.Instance.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);

                    return;
                }

                player.ManaPoints -= CreateSpringAb().ManaCost;

                Score.UpdateUiPrompt(player);


                HubContext.Instance.SendToClient($"You utter aqua fons.", player.HubGuid);

  
                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters aqua fons.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }



                Task.Run(() => DoCreateSpring(player, room));

            }
           

        }

        private static async Task DoCreateSpring(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);
 
                var castingTextAttacker = $"A pool of water suddenly appears on the floor and starts to spew water in the air.";

                HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);

                foreach (var character in room.players)
                {
                    if (character != attacker)
                    {

                        var roomMessage = $"A pool of water suddenly appears on the floor and starts to spew water in the air.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                var magicalSpring = MagicalSpring.MagicalWaterSpring();
               
                room.items.Add(magicalSpring);


            Player.SetState(attacker);
            _taskRunnning = false;
        }

      

        public static Skill CreateSpringAb()
        {


            var skill = new Skill
            {
                Name = "Create Spring",
                SpellGroup = SpellGroupType.Abjuration,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 2,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = " cast 'create spring'"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Create spring conjures a mystical fountain from which you may drink from.",
                DateUpdated = "27/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
