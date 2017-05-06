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

    public class Blindness : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill BlindnessSkill { get; set; }

        public static void StartBlind(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, BlindAb().Name);

            if (hasSpell == false)
            {
                HubContext.SendToClient("You don't know that spell.", player.HubGuid);
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

            _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }


            if (!_taskRunnning && _target != null)
            {


                if (player.ManaPoints < BlindAb().ManaCost)
                {
                    HubContext.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= BlindAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("You utter oculi caecorum", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} utters oculi caecorum.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoBlind(player, room));

            }
            else if (_target == null)
            {
                HubContext.SendToClient("You can't cast this on yourself", player.HubGuid);
            }
        

        }

        private static async Task DoBlind(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < BlindAb().ManaCost)
            {
                HubContext.SendToClient("You attempt to draw energy but fail", attacker.HubGuid);

                return;
            }

            //already blind check
            var isBlind = _target.Affects?.FirstOrDefault(
                    x => x.Name.Equals("Blindness", StringComparison.CurrentCultureIgnoreCase)) != null;

 
                if (isBlind)
                {
                    HubContext.SendToClient("They are already Blind.", attacker.HubGuid);
                }
                else
                {
                    var castingTextAttacker =
                        Helpers.ReturnName(_target, attacker, null) + "'s eyes bind shut.";

                    var castingTextDefender = "Your eyes bind shut.";

                    HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                    HubContext.SendToClient(castingTextDefender, _target.HubGuid);

                    foreach (var character in room.players)
                    {

                        if (character == attacker)
                        {
                            continue;
                        }

                        if (character != _target)
                        {
                            var roomMessage =
                                $"{Helpers.ReturnName(attacker, character, string.Empty)}'s eyes bind shut.";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                }

            //add blind affect


            var blindAff = new Affect
            {
                Name = "Blindness",
                Duration = attacker.Level + 5,
                AffectLossMessagePlayer = "Your eyes regain the ability to see again.",
                AffectLossMessageRoom = $"'s regain the ability to see again."
            };


            if (_target.Affects == null)
            {
                _target.Affects = new List<Affect> {blindAff};

            }
            else
            {
                _target.Affects.Add(blindAff);
            }


                Score.ReturnScoreUI(_target);


            Player.SetState(attacker);

            _target = null;
            _taskRunnning = false;


        }

        public static Skill BlindAb()
        {


            var skill = new Skill
            {
                Name = "Blind",
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
                Syntax = "cast blind <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Blinds person or mob, stopping them from seeing and severely affects their defensive skill.",
                DateUpdated = "20/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}