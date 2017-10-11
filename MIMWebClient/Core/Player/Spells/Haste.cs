using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;

namespace MIMWebClient.Core.Player.Skills
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Haste : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill HasteSkill { get; set; }

        public static void StartHaste(Player player, Room room, string target = "")
        {

            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, HasteAb().Name);

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

            player.Status = Player.PlayerStatus.Busy;

            _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }


            if (!_taskRunnning && _target != null)
            {

                if (player.ManaPoints < HasteAb().ManaCost)
                {
                    HubContext.Instance.SendToClient("You fail to concentrate due to lack of mana.", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= HasteAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.Instance.SendToClient("You utter citus multo festina.", player.HubGuid);

 

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
 
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters citus multo festina.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoHaste(player, room));

            }
            else
            {
                if (_target == null)
                {


                    if (player.ManaPoints < HasteAb().ManaCost)
                    {
                        HubContext.Instance.SendToClient("You attempt to draw energy but fail", player.HubGuid);

                        return;
                    }

                    //TODO REfactor
                    player.ManaPoints -= HasteAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    HubContext.Instance.SendToClient("You utter citus multo festina.", player.HubGuid);


                    foreach (var character in room.players)
                    {
                        if (character != player)
                        {
                            var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters citus multo festina.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    Task.Run(() => DoHaste(player, room));
                     
                }

                 

            }

        }

        private static async Task DoHaste(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            var hasteAff = new Effect
            {
                Name = "Haste",
                Duration = attacker.Level + 5,
                AffectLossMessagePlayer = "Your body begins to slow down.",
                AffectLossMessageRoom = $" begins to slow down."
            };


            await Task.Delay(500);

            try
            {

                if (_target == null)
                {
                    var castingTextAttacker = "You begin to move much faster.";

                    HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);

                    foreach (var character in room.players.ToList())
                    {
                        if (character != attacker)
                        {
                            var roomMessage =
                                $"{Helpers.ReturnName(attacker, character, string.Empty)} starts to blur as they move faster.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }


                    if (attacker.Effects == null)
                    {
                        attacker.Effects = new List<Effect> {hasteAff};

                    }
                    else
                    {
                        attacker.Effects.Add(hasteAff);
                    }

                    Score.ReturnScoreUI(attacker);
                    Score.UpdateUiAffects(attacker);



                }
                else
                {
                    var castingTextAttacker = Helpers.ReturnName(_target, attacker, null) +
                                              " starts to blur as they move faster.";

                    var castingTextDefender = "You begin to move much faster.";

                    HubContext.Instance.SendToClient(castingTextAttacker, attacker.HubGuid);
                    HubContext.Instance.SendToClient(castingTextDefender, _target.HubGuid);

                    foreach (var character in room.players.ToList())
                    {
                        if (character != attacker || character != _target)
                        {
                            var roomMessage =
                                $"{Helpers.ReturnName(_target, character, string.Empty)} starts to blur as they move faster.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    if (_target.Effects == null)
                    {
                        _target.Effects = new List<Effect> {hasteAff};

                    }
                    else
                    {
                        _target.Effects.Add(hasteAff);
                    }
                    Score.UpdateUiAffects(_target);
                    Score.ReturnScoreUI(_target);

                }

                Player.SetState(attacker);


            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.Message.ToString(),
                    MethodName = "haste"
                };

                Save.LogError(log);
            }

            _target = null;
            _taskRunnning = false;
     

        }

        public static Skill HasteAb()
        {


            var skill = new Skill
            {
                Name = "Haste",
                SpellGroup = SpellGroupType.Transmutation,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 2,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast haste / cast haste <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Increases your dexterity and speed in performing actions",
                DateUpdated = "20/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
