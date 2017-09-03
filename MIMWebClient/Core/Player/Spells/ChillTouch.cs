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

    public class ChillTouch : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill WeakenSkill { get; set; }

        public static void StartChillTouch(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, ChillTouchAb().Name);

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

            if (player.Equipment.Wielded != "Nothing")
            {

                if (player.Equipment.Wielded == "Chill Touch")
                {
                    HubContext.SendToClient("You already have chill touch active.", player.HubGuid);

                    return;
                }

                HubContext.SendToClient("You can only cast chill touch if you are not wielding a weapon.", player.HubGuid);

                return;

            }


            if (!_taskRunnning && _target != null)
            {

                HubContext.SendToClient("You can only cast chill touch on your self", player.HubGuid);


            }
            else
            {
                if (_target == null)
                {

                    if (player.ManaPoints < ChillTouchAb().ManaCost)
                    {
                        HubContext.SendToClient("You fail to concerntrate due to lack of mana.", player.HubGuid);

                        return;
                    }

                    //TODO REfactor

                    player.ManaPoints -= ChillTouchAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    HubContext.SendToClient("You utter tactus glacies.", player.HubGuid);

                    var playersInRoom = new List<Player>(room.players);

                    foreach (var character in room.players)
                    {
                        if (character != player)
                        {
                            var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters tactus glacies.";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    Task.Run(() => DoChillTouch(player, room));


                }
                else
                {
                    HubContext.SendToClient("You can only cast chill touch on your self", player.HubGuid);

                }



            }

        }

        private static async Task DoChillTouch(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);


            var castingTextAttacker = "Your hands turn translucent as they turn to ice, your finger tips now resemble icicles.";

            HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);

            foreach (var character in room.players)
            {
                if (character != attacker)
                {

                    var hisOrHer = Helpers.ReturnHisOrHers(attacker, character);
                    var roomMessage = $"{Helpers.ReturnName(_target, character, string.Empty)}'s hands turn translucent as they turn to ice, their finger tips now resemble icicles.";

                    HubContext.SendToClient(roomMessage, character.HubGuid);
                }

            }


            var ChillTouchWeapon = new Item.Item
            {
                name = "Chill Touch",
                damageType = new List<Item.Item.DamageType>
                {
                    Item.Item.DamageType.Chill
                },
                stuck = true,
                attackType = Item.Item.AttackType.Pierce,
                equipable = true,
                description = new Item.Description
                {
                    look = "Your hands are now frozen blocks of ice with your fingers as 5 razer sharp icicles."
                },
                location = Item.Item.ItemLocation.Wield,
                stats = new Item.Stats
                {
                    damMax = 6,
                    damMin = 2,

                },
                eqSlot = Item.Item.EqSlot.Wielded,
                slot = Item.Item.EqSlot.Wielded
            };

            attacker.Inventory.Add(ChillTouchWeapon);

            attacker.Equipment.Wielded = "Chill Touch";
           

            var chillTouchAff = new Affect
            {
                Name = "Chill Touch",
                Duration = attacker.Level + 3,
                AffectLossMessagePlayer = "Your hands feel warm again melting the ice that was once around your hand.",
                AffectLossMessageRoom = $" ice hands melt, revealing their true hands."
            };


            if (attacker.Affects == null)
            {
                attacker.Affects = new List<Affect>();
                attacker.Affects.Add(chillTouchAff);

            }
            else
            {
                attacker.Affects.Add(chillTouchAff);
            }

            Score.UpdateUiAffects(_target);
            Score.ReturnScoreUI(_target);

            Player.SetState(attacker);
            attacker = null;
            _taskRunnning = false;


        }

        public static Skill ChillTouchAb()
        {


            var skill = new Skill
            {
                Name = "Chill Touch",
                SpellGroup = SpellGroupType.Conjuration,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast Chill touch"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Turns your hands ice cold dealing 2 - 6 dam of cold damage",
                DateUpdated = "16/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}