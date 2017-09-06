using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.World.Items.MiscEQ.Light;

namespace MIMWebClient.Core.Player.Skills
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Invis : Skill
    {
        private static bool _taskRunnning = false;
        private static Item.Item _target = new Item.Item();

        public static Skill nvisSkill { get; set; }

        public static void StartInvis (Player player, Room room, string commandOptions = "")
        {

            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, InvisAb().Name);

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

            #region refactor

            string[] options = commandOptions.Split(' ');
            int nth = -1;
            string getNth = string.Empty;
            string objectToFind = String.Empty;
 

            if (options.Length == 3)
            {
                objectToFind = options[2];

                if (objectToFind.IndexOf('.') != -1)
                {
                    getNth = objectToFind.Substring(0, objectToFind.IndexOf('.'));
                    int.TryParse(getNth, out nth);
                }


            }


            #endregion

            if (nth == 0) {  nth = -1;  }

            if (!string.IsNullOrEmpty(objectToFind))
            {
                _target = FindItem.Item(player.Inventory, nth, objectToFind, Item.Item.ItemLocation.Inventory);

                if (_target == null)
                {
                    var findPlayer = FindItem.Player(room.players, nth, objectToFind);
                    var findMob = FindItem.Player(room.mobs, nth, objectToFind);

                    if (findPlayer != null || findMob != null)
                    {
                        HubContext.SendToClient("You can only cast invis on yourself or items in your inventory.", player.HubGuid);

                        return;
                    } else
                    {
                        HubContext.SendToClient("You don't have that to turn invisible.", player.HubGuid);
                        return;
                    }

                }

            }
            else
            {
                _target = null;
            }

            if (!_taskRunnning && _target != null)
            {

                if (player.ManaPoints < InvisAb().ManaCost)
                {
                    HubContext.SendToClient("You attempt to draw energy but fail", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= InvisAb().ManaCost;

                Score.UpdateUiPrompt(player);

                if (_target.itemFlags == null)
                {
                    _target.itemFlags = new List<Item.Item.ItemFlags>();
                }

                if (_target.itemFlags.Contains(Item.Item.ItemFlags.invis))
                {
                    HubContext.SendToClient("This item is already invisible", player.HubGuid);
                    return;
                }


                var result = AvsAnLib.AvsAn.Query(_target.name);
                string article = result.Article;


                HubContext.SendToClient($"You take hold of the {_target.name} between your hands which starts to fade in and out of existence", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                        var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} takes hold of the {_target.name} between {hisOrHer} hands which starts to fade in and out of existence.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoInvis(player, room));

            }
            else
            {
                if (_target == null)
                {

                    if (player.ManaPoints < InvisAb().ManaCost)
                    {
                        HubContext.SendToClient("You attempt to draw energy but fail", player.HubGuid);

                        return;
                    }

                    //TODO REfactor
                    player.ManaPoints -= InvisAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    var hasFaerieFire = player.Affects?.FirstOrDefault(
                                            x => x.Name.Equals("Faerie Fire", StringComparison.CurrentCultureIgnoreCase)) !=
                                        null;

                    if (!hasFaerieFire)
                    {
                        HubContext.SendToClient($"You start to fade in and out of existence.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (character != player)
                            {
                                var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} starts to fade in and out of existence";

                                HubContext.SendToClient(roomMessage, character.HubGuid);
                            }
                        }


                        Task.Run(() => DoInvis(player, room));

                    }
                    else
                    {
                        HubContext.SendToClient($"You fail to turn invisible due to the glow of Faerie fire surrounding you.", player.HubGuid);
                    }
                }



            }

        }

        private static async Task DoInvis(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            if (_target == null)
            {
                var castingTextAttacker =  $"You fade out of existence";
 
                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);

 
                foreach (var character in room.players)
                {
                    if (character != attacker)
                    {
                        var roomMessage = $"{ Helpers.ReturnName(attacker, character, string.Empty)} fades out of existence.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                attacker.invis = true;
                var invisAffect = new Affect
                {
                    Name = "Invis",
                    Duration = InvisAb().Duration * attacker.Level,
                    AffectLossMessagePlayer = "You are no longer invisible.",
                    AffectLossMessageRoom = "fades into existence."
                };
                 

                if (attacker.Affects != null)
                {
                    var checkExisting = attacker.Affects.FirstOrDefault(x => x.Name.Equals("Invis"));

                    if (checkExisting == null)
                    {
                        attacker.Affects.Add(invisAffect);
                    }
                    else
                    {
                        checkExisting.Duration = InvisAb().Duration;
                    }
                   
                }
                else
                {
                    attacker.Affects = new List<Affect>
                    {
                        invisAffect
                    };



                }

                Score.UpdateUiAffects(attacker);

            }
            else
            {
                var castingTextAttacker =  $"The {_target.name} fades out of existence.";

                var castingTextRoom = $"The {_target.name} fades out of existence.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.broadcastToRoom(castingTextRoom, room.players, attacker.HubGuid, true);

                _target.itemFlags.Add(Item.Item.ItemFlags.invis);          

            }

            Player.SetState(attacker);

            _target = null;
            _taskRunnning = false;
     

        }

       
        public static Skill InvisAb()
        {


            var skill = new Skill
            {
                Name = "Invis",
                SpellGroup = SpellGroupType.Illusion,
                SkillType = Type.Spell,
                CoolDown = 0,
                Duration = 5,
                Delay = 0,
                LevelObtained = 2,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = " cast invis <target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "The invis spell makes the character invisible. " +
                "If a target is given (player or items) it becomes invisible. Invisible creatures become visible again during combat." +
                "T become visible again, simply type visible.",
                DateUpdated = "06/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
