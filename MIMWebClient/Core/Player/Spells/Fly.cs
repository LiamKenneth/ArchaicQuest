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

    public class Fly : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill FlySkill { get; set; }

        public static void StartFly(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, FlyAb().Name);

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


            _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }


            if (!_taskRunnning && _target != null)
            {


                if (player.ManaPoints < FlyAb().ManaCost)
                {
                    HubContext.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= FlyAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("You utter volito autem.", player.HubGuid);
 

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
 
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters volito autem.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoFly(player, room));

            }
            else
            {

                player.ManaPoints -= FlyAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("You utter volito autem.", player.HubGuid);

 

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
 
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters volito autem.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoFly(player, room));

            }

        }

        private static async Task DoFly(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            var flyAff = new Affect
            {
                Name = "Fly",
                Duration = attacker.Level + 5,
                AffectLossMessagePlayer = "You float back down to the ground.",
                AffectLossMessageRoom = $" floats back down to the ground."
            };



            if (_target == null)
            {
                var castingTextAttacker = "Your feet levitate off the ground.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
 

                foreach (var character in room.players)
                {

                    if (character == attacker)
                    {
                        continue;
                    }

                    if (character != attacker)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(attacker, character, string.Empty)}'s feet rise off the ground.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }


                if (attacker.Affects == null)
                {
                    attacker.Affects = new List<Affect>();
                    attacker.Affects.Add(flyAff);

                }
                else
                {
                    attacker.Affects.Add(flyAff);
                }

                Score.UpdateUiAffects(attacker);
            }
            else
            {
                var castingTextAttacker =
                     Helpers.ReturnName(_target, attacker, null) + "'s feet rise off the ground.";

                var castingTextDefender = "Your feet levitate off the ground.";

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
                        var hisOrHer = Helpers.ReturnHisOrHers(attacker, character);
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)}'s feet rise off the ground.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                if (_target.Affects == null)
                {
                    _target.Affects = new List<Affect>();
                    _target.Affects.Add(flyAff);

                }
                else
                {
                    _target.Affects.Add(flyAff);
                }

                Score.ReturnScoreUI(_target);
                Score.UpdateUiAffects(_target);
            }

            Player.SetState(attacker);
            _target = null;
            _taskRunnning = false;


        }

        public static Skill FlyAb()
        {


            var skill = new Skill
            {
                Name = "Fly",
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
                Syntax = "cast fly <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Makes you hover, reducing movement costs, and avoiding some skills such as bash",
                DateUpdated = "17/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}