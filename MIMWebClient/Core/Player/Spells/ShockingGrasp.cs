using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player.Skills
{

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using System.Threading.Tasks;

    public class ShockingGrasp : Skill
    {
 
        public static Skill ShockingGraspSkill { get; set; }

        public void StartShockingGrasp(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, ShockingGraspAb().Name);

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

            if (string.IsNullOrEmpty(target) && player.Target != null)
            {
                target = player.Target.Name;
            }

           var _target = Skill.FindTarget(target, room);
 
            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }

            if (player.ActiveSkill != null)
            {

                HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = ShockingGraspAb();
            }
 

      

            if (_target != null)
            {


                if (player.ManaPoints < ShockingGraspAb().ManaCost)
                {


                    HubContext.Instance.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= ShockingGraspAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.Instance.SendToClient("You utter fulgur iaculis.", player.HubGuid);

             
                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} utters fulgur iaculis.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoShockingGrasp(player, _target, room));

            }
            else if (_target == null)
            {
                HubContext.Instance.SendToClient("You can't cast this on yourself", player.HubGuid);
            }


        }

        private async Task DoShockingGrasp(Player attacker, Player target, Room room)
        {
          
            attacker.Status = Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < ShockingGraspAb().ManaCost)
            {
                HubContext.Instance.SendToClient("You attempt to draw energy but fail", attacker.HubGuid);
                attacker.ActiveSkill = null;
                Player.SetState(attacker);
                return;
            }

            var die = new PlayerStats();

            var dam = die.dice(5, 6);

            var toHit = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(ShockingGraspAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss
            int chance = die.dice(1, 100);

            Fight2.ShowAttack(attacker, target, room, toHit, chance, ShockingGraspAb(), dam);


            Score.ReturnScoreUI(target);


            Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);

            target = null;
            attacker.ActiveSkill = null;

        }

        public static Skill ShockingGraspAb()
        {


            var skill = new Skill
            {
                Name = "Shocking Grasp",
                SpellGroup = SpellGroupType.Invocation,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast 'shocking grasp' <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Sends a short burst of lightning towards the target.",
                DateUpdated = "02/07/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}