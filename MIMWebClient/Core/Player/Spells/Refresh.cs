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

    public class Refresh : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill RefreshSkill { get; set; }

        public static void StartRefresh(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, RefreshAb().Name);

            if (hasSpell == false)
            {
                HubContext.SendToClient("You don't know that spell.", player.HubGuid);
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


                if (player.ManaPoints < RefreshAb().ManaCost)
                {
                    HubContext.SendToClient("You fail to concerntrate due to lack of mana.", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= RefreshAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("You utter et refocillabatur.", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                   
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters et refocillabatur.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoRefresh(player, room));

            }
            else
            {

                player.ManaPoints -= RefreshAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("You utter et refocillabatur.", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters volito autem.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoRefresh(player, room));

            }

        }

        private static async Task DoRefresh(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            if (attacker.ManaPoints < RefreshAb().ManaCost)
            {
                HubContext.SendToClient("You attempt to draw energy but fail", attacker.HubGuid);

                return;
            }



            if (_target == null)
            {
                var castingTextAttacker = "You feel refreshed.";

                if (attacker.MovePoints >= attacker.MaxMovePoints)
                {
                    HubContext.SendToClient("You are already fully refreshed.", attacker.HubGuid);
                }
                else
                {
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
                                $"{Helpers.ReturnName(attacker, character, string.Empty)} looks refreshed.";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                }

                attacker.MovePoints += Helpers.Rand(10, attacker.Level * 5 + 20);

                if (attacker.MovePoints > attacker.MaxMovePoints)
                {
                    attacker.MovePoints = attacker.MaxMovePoints;
                }

                Score.ReturnScoreUI(attacker);
            }
            else
            {

                if (_target.MovePoints >= _target.MaxMovePoints)
                {
                    HubContext.SendToClient("They are already fully refreshed.", attacker.HubGuid);
                }
                else
                {
                    var castingTextAttacker =
                        Helpers.ReturnName(_target, attacker, null) + " looks refreshed.";

                    var castingTextDefender = "You feel refreshed.";

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
                                $"{Helpers.ReturnName(attacker, character, string.Empty)} looks refreshed.";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                }

                _target.MovePoints += Helpers.Rand(10, _target.Level * 5 + 20);

                if (_target.MovePoints > _target.MaxMovePoints)
                {
                    _target.MovePoints = _target.MaxMovePoints;
                }

                Score.ReturnScoreUI(_target);
            }

            Score.ReturnScoreUI(attacker);
            Player.SetState(attacker);

            _target = null;
            _taskRunnning = false;


        }

        public static Skill RefreshAb()
        {


            var skill = new Skill
            {
                Name = "Refresh",
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
                Syntax = "cast refresh <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Restores movement costs",
                DateUpdated = "19/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}