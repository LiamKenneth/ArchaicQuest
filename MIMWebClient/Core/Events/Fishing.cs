using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.Player.Skills
{

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using System.Threading.Tasks;

    public class Fishing : Skill
    {


        public void StartFishing(Player player, Room room)
        {

            if (room.terrain != Room.Terrain.Water)
            {
                HubContext.Instance.SendToClient("You must be by water to fish.", player.HubGuid);
                return;
            }

            var rod = player.Inventory.FirstOrDefault(x => x.name.Contains("rod") &&
                                                           x.location == Item.Item.ItemLocation.Worn);
            if (rod == null)
            {
                HubContext.Instance.SendToClient("You must be holding a rod to fish.", player.HubGuid);
                return;
            }

            if (rod.HasBeenCast)
            {
                HubContext.Instance.SendToClient("Your rod has already been casted.", player.HubGuid);
                return;
            }

            var fishingAB = player.Skills.FirstOrDefault(x => x.Name.Equals("Fishing"));
            if (player.ActiveSkill != null)
            {

                HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = fishingAB;
            }


            if (player.MovePoints < 10)
            {


                HubContext.Instance.SendToClient("You are too tired to cast the rod.", player.HubGuid);
                player.ActiveSkill = null;

                return;
            }

 

            player.MovePoints -= 15;

            if (player.MovePoints < 0)
            {
                player.MovePoints = 0;
            }

            Score.UpdateUiPrompt(player);

            var getSkill = player.Skills.FirstOrDefault(x => x.Name.Equals("Fishing"));
            double getSkillProf = 0;
            if (getSkill != null)
            {
                getSkillProf = getSkill.Proficiency /  (double)95 * 100;
            }

            var successChance = Helpers.Rand(1, 100);

            if (getSkillProf >= successChance)
            {


                HubContext.Instance.SendToClient("You cast your rod out, the float plops into the water.",
                    player.HubGuid);


                rod.HasBeenCast = true;
                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} casts their rod into the water.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoFishing(player, room));

            }
            else
            {
                var failMessage = "";
                switch (Helpers.Rand(1, 4))
                {
                    case 1:
                        failMessage = "you hooked yourself Ouch!";
                        break;
                    case 2:
                    case 3:
                        failMessage = "you fail to cast correctly.";
                        break;
                    default:
                        failMessage = "you fail to cast correctly.";
                        break;
                }

                HubContext.Instance.SendToClient(failMessage,
                    player.HubGuid);

                if (getSkillProf < 95)
                {

                    HubContext.Instance.SendToClient("You learn from your mistakes and gain 100 experience points",
                        player.HubGuid);

                    var xp = new Experience();
                    player.Experience += 100;

                    xp.GainLevel(player);

                    getSkill.Proficiency += Helpers.Rand(1, 5);

                }

                Score.ReturnScoreUI(player);

                player.ActiveSkill = null;

            }


        }

        public static void GetFish(Player player, Room room)
        {
            var rod = player.Inventory.FirstOrDefault(x => x.name.Contains("rod") &&
                                                           x.location == Item.Item.ItemLocation.Worn);
            if (rod == null)
            {
                HubContext.Instance.SendToClient("You must be holding a rod before you can reel anything in.", player.HubGuid);

                return;
            }

            if (rod.HasBeenCast == false)
            {
                HubContext.Instance.SendToClient("You have to angle your rod before trying to reel in.", player.HubGuid);

                return;
            }

            var getSkill = player.Skills.FirstOrDefault(x => x.Name.Equals("Fishing"));
            double getSkillProf = 0;
            if (getSkill != null)
            {
                getSkillProf = getSkill.Proficiency / (double)95 * 100;
            }

            var successChance = Helpers.Rand(1, 100);

            if (getSkillProf >= successChance)
            {


                if (rod.HasFish)
                {


                    rod.HasFish = false;

                    Score.UpdateUiPrompt(player);

                    var caughtFish = "Perch";

                    switch (Helpers.Rand(1, 16))
                    {
                        case 1:
                            caughtFish = "bream";
                            break;
                        case 2:
                        case 3:
                            caughtFish = "Brown Trout";
                            break;
                        case 4:
                        case 5:
                            caughtFish = "Carp";
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            caughtFish = "Chub";
                            break;
                        case 11:
                            caughtFish = "Perch";
                            break;
                        case 12:
                            caughtFish = "Snapping turtle";
                            break;
                        case 13:
                            caughtFish = "Eel";
                            break;
                        case 14:
                            caughtFish = "Frog";
                            break;
                        case 15:
                        case 16:
                            caughtFish = "Water Snake";
                            break;

                    }

                    if (caughtFish == "Water Snake")
                    {


                        HubContext.Instance.SendToClient("You hooked a " + caughtFish + "!!", player.HubGuid);

                        var snake =
                            new Player()
                            {
                                Name = "Water Snake",
                                Aggro = true,
                                Strength = 40,
                                Dexterity = 60,
                                Constitution = 30,
                                Wisdom = 30,
                                Intelligence = 40,
                                Charisma = 30,
                                MobAttackType = Player.MobAttackTypes.Bite,
                                Type = Player.PlayerTypes.Mob,
                                HitPoints = 25,
                                MaxHitPoints = 25,
                                MovePoints = 50,
                                MaxMovePoints = 50,
                                MobAttackStats = new Stats()
                                {
                                    damMax = 11,
                                    damMin = 4
                                },
                                Level = 2,


                            };

                        room.mobs.Add(snake);

                        Player.MobAttack(snake, player, room);


                    }
                    else
                    {


                        HubContext.Instance.SendToClient("You reel your rod in, you have caught a " + caughtFish + ".",
                            player.HubGuid);


                        var fish = new Item.Item()
                        {
                            name = caughtFish,
                            location = Item.Item.ItemLocation.Inventory
                        };

                        player.Inventory.Add(fish);

                        Score.UpdateUiInventory(player);


                        foreach (var character in room.players)
                        {
                            if (character != player)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} reels their rod in, they have caught a " +
                                    caughtFish;

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }



                    }


                }
                else
                {

                    HubContext.Instance.SendToClient("You reel your rod in but nothing is on the hook.",
                        player.HubGuid);
                    rod.HasBeenCast = false;
                }
            }
            else
            {

                var failMessage = "";
                switch (Helpers.Rand(1, 4))
                {
                    case 1:
                        failMessage = "Your line breaks.";
                        break;
                    case 2:
                        failMessage = "You hooked a stump.";
                        break;
                    case 3:
                        failMessage = "You reel in too fast and lost the catch.";
                        break;
                    default:
                        failMessage = "The line snapped and you hit yourself in the face. Ouch!";
                        break;
                } 

                HubContext.Instance.SendToClient(failMessage,
                    player.HubGuid);

                if (getSkillProf < 95)
                {

                    HubContext.Instance.SendToClient("You learn from your mistakes and gain 100 experience points",
                        player.HubGuid);

                    var xp = new Experience();
                    player.Experience += 100;

                    xp.GainLevel(player);

                    getSkill.Proficiency += Helpers.Rand(1, 5);

                    player.ActiveSkill = null;

                }

                Score.ReturnScoreUI(player);
            }

            player.ActiveSkill = null;
        }

        private async Task DoFishing(Player player, Room room)
        {

            player.Status = Player.PlayerStatus.Busy;

            await Task.Delay(500);

            var fishingText = string.Empty;

            switch (Helpers.Rand(1, 3))
            {
                case 1:
                    fishingText = "You see the bobble dip a little";
                    break;
                case 2:
                    fishingText = "You feel your line tighten";
                    break;
                default:
                    fishingText = "You see ripples around the bobble";
                    break;
            }

            await Task.Delay(1500);


            HubContext.Instance.SendToClient(fishingText, player.HubGuid);

            await Task.Delay(1500);

            HubContext.Instance.SendToClient("You see your bobble quickly submerge.", player.HubGuid);

            var rod = player.Inventory.FirstOrDefault(x => x.name.Contains("rod") &&
                                                           x.location == Item.Item.ItemLocation.Worn);
            if (rod != null && rod.HasBeenCast) rod.HasFish = true;

            await Task.Delay(Helpers.Rand(800, 3000));

            rod.HasFish = false;
            rod.HasBeenCast = false;

        }


    }
}