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

    public class FaerieFire : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill FaerieFireSkill { get; set; }

        public static void StartFaerieFire(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, FaerieFireAB().Name);

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


            _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }


            if (!_taskRunnning && _target != null)
            {

                if (player.ManaPoints < FaerieFireAB().ManaCost)
                {
                    HubContext.Instance.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= FaerieFireAB().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.Instance.SendToClient("You utter mediocris ignis.", player.HubGuid);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters mediocris ignis.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoFaerieFire(player, room));

            }
            else
            {

                HubContext.Instance.SendToClient("You can't cast this on yourself", player.HubGuid);

            }

        }

        private static async Task DoFaerieFire(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            var faerieFireAff = new Effect
            {
                Name = "Faerie Fire",
                Duration = attacker.Level + 5,
                AffectLossMessagePlayer = "The Faerie fire surrounding you decapitates.",
                AffectLossMessageRoom = $" stops glowing as the Faerie fire decapitates."
            };



            if (_target == null)
            {
               HubContext.Instance.SendToClient("You can't cast this on yourself", attacker.HubGuid);
              
            }
            else
            {
                var castingTextAttacker =
                     Helpers.ReturnName(_target, attacker, null) + " is surrounded by pink glowing Faerie Fire.";

                var castingTextDefender = "You are surrounded by pink glowing faerie fire.";

                HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.Instance.SendToClient(castingTextDefender, _target.HubGuid);

                foreach (var character in room.players)
                {

                    if (character == attacker)
                    {
                        continue;
                    }

                    if (character != _target)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(attacker, character);
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)} is surrounded by pink glowing Faerie Fire.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                if (_target.Effects == null)
                {
                    _target.Effects = new List<Effect>();
                    _target.Effects.Add(faerieFireAff);

                }
                else
                {
                    _target.Effects.Add(faerieFireAff);
                }
                Score.UpdateUiAffects(_target);
                Score.ReturnScoreUI(_target);
            }

            Player.SetState(attacker);
            _target = null;
            _taskRunnning = false;


        }

        public static Skill FaerieFireAB()
        {


            var skill = new Skill
            {
                Name = "Faerie Fire",
                SpellGroup = SpellGroupType.Transmutation,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast 'Faerie Fire' <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Makes the target glow red, reducing their ability to hide or sneak.",
                DateUpdated = "17/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}