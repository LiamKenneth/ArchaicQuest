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

    public class Armour : Skill
    {
        public static Skill ArmourSkill { get; set; }

        public void StartArmour(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, ArmourAb().Name);

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

            var _target = Skill.FindTarget(target, room);

            if (_target == null)
            {
                _target = player;
            }


            if (player.ActiveSkill != null)
            {

                HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = ArmourAb();
            }



            if (_target != null)
            {


                if (player.ManaPoints < ArmourAb().ManaCost)
                {


                    HubContext.Instance.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= ArmourAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.Instance.SendToClient("You utter induendum armatura.", player.HubGuid);


                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} utters induendum armatura.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoArmour(player, _target, room));

            }
            else 
            {
                HubContext.Instance.SendToClient("No one here by that name.", player.HubGuid);
                player.ActiveSkill = null;

                return;
            }


        }

        private async Task DoArmour(Player attacker, Player target, Room room)
        {

            attacker.Status = Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < ArmourAb().ManaCost)
            {
                HubContext.Instance.SendToClient("You attempt to draw energy but fail", attacker.HubGuid);
                attacker.ActiveSkill = null;
                Player.SetState(attacker);
                return;
            }


            if (target == attacker)
            {
                var castingTextAttacker =
                    "You place your hands upon your chest engulfing yourself in a white protective glow.";

                HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);

                foreach (var character in room.players)
                {
                    if (character != attacker)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(attacker, character);
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)} places {hisOrHer} hands upon {hisOrHer} chest engulfing themselves in a white protective glow.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

              

                if (Effect.HasEffect(attacker, "Armour"))
                {
                    attacker.Effects.Remove(attacker.Effects.FirstOrDefault(x => x.Name.Equals("Armour")));
                    attacker.ArmorRating -= 20;
                }
                target.ArmorRating += 20;
                attacker.Effects.Add(Effect.Armour(attacker));
                Score.UpdateUiAffects(attacker);

                Score.ReturnScoreUI(attacker);

            }
            else
            {
                var castingTextAttacker =
                    "You place your hands upon " + Helpers.ReturnName(target, attacker, null) + " engulfing them in a white protective glow.";

                var castingTextDefender = Helpers.ReturnName(attacker, target, null) + " touches your chest engulfing you in a white protective glow.";

                HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.Instance.SendToClient(castingTextDefender, target.HubGuid);

                foreach (var character in room.players)
                {
                    if (character != attacker || character != target)
                    {
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)} touches {Helpers.ReturnName(target, character, string.Empty)} engulfing them in a white protective glow.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }



               
                if (Effect.HasEffect(attacker, "Armour"))
                {
                    attacker.Effects.Remove(attacker.Effects.FirstOrDefault(x => x.Name.Equals("Armour")));
                    attacker.ArmorRating -= 20;
                }
                target.ArmorRating += 20;
                attacker.Effects.Add(Effect.Armour(target));
                Score.UpdateUiAffects(target);
                Score.ReturnScoreUI(target);

            }

            PlayerSetup.Player.SetState(attacker);
            attacker.ActiveSkill = null;

        }

        public static Skill ArmourAb()
        {


            var skill = new Skill
            {
                Name = "Armour",
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
                Syntax = "cast armour / cast armour <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Increases Armour rating by 20",
                DateUpdated = "28/03/2017"

            };

            skill.HelpText = help;


            return skill;

        }
    }
}
