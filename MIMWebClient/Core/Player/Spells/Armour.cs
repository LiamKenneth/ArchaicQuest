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
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill ArmourSkill { get; set; }

        public static void StartArmour(Player player, Room room, string target = "")
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

            _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }


            if (!_taskRunnning && _target != null)
            {

                if (player.ManaPoints < ArmourAb().ManaCost)
                {
                    HubContext.Instance.SendToClient("You attempt to draw energy to your hands but fail", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= ArmourAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.Instance.SendToClient("Your hands start to glow as you begin chanting the armour spell", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} 's hands start to glow as they begin chanting the Armour spell";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoArmour(player, room));

            }
            else
            {
                if (_target == null)
                {


                    if (player.ManaPoints < ArmourAb().ManaCost)
                    {
                        HubContext.Instance.SendToClient("You attempt to draw energy but fail", player.HubGuid);

                        return;
                    }

                    //TODO REfactor
                    player.ManaPoints -= ArmourAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    HubContext.Instance.SendToClient("Your hands start to glow as you begin chanting the armour spell", player.HubGuid);
 
                    foreach (var character in room.players)
                    {
                        if (character != player)
                        {
                            var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} 's hands start to glow as they begin chanting the Armour spell";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    Task.Run(() => DoArmour(player, room));
                     
                }

                 

            }

        }

        private static async Task DoArmour(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            if (_target == null)
            {
                var castingTextAttacker =
                    "You place your hands upon your chest engulfing yourself in a white protective glow.";

                HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);

                var excludePlayers = new List<string> {attacker.HubGuid};

                foreach (var character in room.players)
                {
                    if (character != attacker)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(attacker, character);
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)} places {hisOrHer} hands upon {hisOrHer} chest engulfing themselves in a white protective glow.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                attacker.ArmorRating += 20;

                Score.ReturnScoreUI(attacker);

            }
            else
            {
                var castingTextAttacker =
                      "You place your hands upon " + Helpers.ReturnName(_target, attacker, null) + " engulfing them in a white protective glow.";

                var castingTextDefender = Helpers.ReturnName(attacker, _target, null) + " touches your chest engulfing you in a white protective glow.";

                HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.Instance.SendToClient(castingTextDefender, _target.HubGuid);
 
                foreach (var character in room.players)
                {
                    if (character != attacker || character != _target)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(attacker, character);
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)} touches {Helpers.ReturnName(_target, character, string.Empty)} engulfing them in a white protective glow.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

             

                _target.ArmorRating += 20;

                Score.ReturnScoreUI(_target);

            }
            Player.SetState(attacker);
            _target = null;
            _taskRunnning = false;
     

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
