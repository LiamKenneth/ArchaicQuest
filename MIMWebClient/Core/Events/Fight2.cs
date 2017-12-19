using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Ajax.Utilities;
using MIMWebClient.Core.Loging;
using MIMWebClient.Core.Player.Skills;


namespace MIMWebClient.Core.Events
{
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using Player;
    using System.Data.Entity.Infrastructure.Pluralization;

    public class Fight2
    {
        /// <summary>
        /// Starts a fight between two players or mobs
        /// The defender can only fight it's original target 
        /// so cant fight back at multiple targets
        /// </summary>
        /// <param name="attacker">The attacker</param>
        /// <param name="room">The room</param>
        /// <param name="defenderName">The defenders Name for now</param>
        /// <returns></returns>
        public static void PerpareToFight(Player attacker, Room room, string defenderName, bool busy = false)
        {


            //fixes issue where fight task fails to run as the attacker fighting is still set to true
            //unsure how this happens
            if (attacker.ActiveFighting == true && attacker.Target == null)
            {
                attacker.ActiveFighting = false;
            }


            if (attacker == null)
            {
                return;
            }

            if (attacker.Target != null)
            {
                HubContext.Instance.SendToClient("You are already fighting!!.", attacker.HubGuid);
                return;
            }

            /* player can only attack one target
           * if player gets attacked by something else they cannot fight back until
           * they have ended the fight they are already in.
           * player defence should be divided by the number of people they are being attacked by.
           * 
           */

            //automated Combat rounds for melee attacks

            //find defender 
            Player defender = FindValidTarget(room, defenderName, attacker);


            if (defender == null)
            {
                HubContext.Instance.SendToClient("No one here by that name.", attacker.HubGuid);
                return;
            }



            defender.Status = Player.PlayerStatus.Fighting;

            attacker.Status = !busy ? Player.PlayerStatus.Fighting : Player.PlayerStatus.Busy;


            AddFightersIdtoRoom(attacker, defender, room);

            StartFight(attacker, defender, room);

        }

        /// <summary>
        /// Starts a fight between two players or mobs
        /// The defender can only fight it's original target 
        /// so cant fight back at multiple targets
        /// </summary>
        /// <param name="attacker">The attacker</param>
        /// <param name="room">The room</param>
        /// <param name="defenderName">The defenders Name for now</param>
        /// <returns></returns>
        public static void PerpareToFightBack(Player attacker, Room room, string defenderName, bool busy = false)
        {


            Player defender = FindValidTarget(room, defenderName, attacker);


            if (defender == null)
            {
                HubContext.Instance.SendToClient("No one here by that name.", attacker.HubGuid);
                return;
            }

            if (defender.Status != Player.PlayerStatus.Stunned)
            {
                defender.Status = Player.PlayerStatus.Fighting;
            }

            if (IsAlive(defender, attacker))
            {
                if (attacker.Target == null)
                {
                    attacker.Target = defender;
                }
            }

            StartFight(defender, attacker, room);

        }

        public static void AddFightersIdtoRoom(Player attacker, Player defender, Room room, bool includeAttacker = true)
        {

            if (room.fighting == null)
            {
                room.fighting = new List<string>();
            }

            if (attacker.HubGuid != null)
            {
                room.fighting.Add(attacker.HubGuid);
            }

            if (defender.HubGuid != null)
            {
                room.fighting.Add(defender.HubGuid);
            }



        }


        public static void StartFight(Player attacker, Player defender, Room room)
        {



            if (defender.Target == null || defender.Target == attacker)
            {
                defender.Target = attacker;

                if (!defender.ActiveFighting)
                {
                    defender.ActiveFighting = true;
                    Task.Run(() => HitTarget(defender, attacker, room, 2000));

                }

            }

            if (attacker.Target == null || attacker.Target == defender)
            {
                attacker.Target = defender;

                if (!attacker.ActiveFighting)
                {
                    if (attacker.Status == Player.PlayerStatus.Fighting)
                    {

                        double offense = Offense(attacker);
                        double evasion = Evasion(defender);

                        double toHit = (offense / evasion) * 100;

                        if (Effect.HasEffect(defender, Effect.Blindness(defender).Name))
                        {
                            toHit += 25;
                        }

                        if (Effect.HasEffect(attacker, Effect.Blindness(defender).Name))
                        {
                            toHit -= 35;
                        }

                        int chance = D100();


                        ShowAttack(attacker, defender, room, toHit, chance, null, true);

                    }


                    attacker.ActiveFighting = true;
                    Task.Run(() => HitTarget(attacker, defender, room, 2000));
                }

            }
            //3000, 3 second timer needs to be a method taking in players dexterity, condition and spells to determine speed.



        }

        public static Player FindValidTarget(Room room, string defender, Player attacker)
        {
  
            var findObject = Events.FindNth.Findnth(defender);
            int nth = findObject.Key;
            string itemToFind = findObject.Value;

            Player defendingPlayer = null;

            //BUG: Can't find correct player of same name. e.g bill, billy. if billy want to kill bill we need to remove billy from the list so they don't find themseleves 1st.
           

            if (nth == -1)
            {
                defendingPlayer =
                  room.players.Where(x => x.HitPoints > 0).ToList().FirstOrDefault(
                      x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                  ?? room.mobs.Where(x => x.HitPoints > 0).ToList().FirstOrDefault(x => x.Name.ToLower().Contains(defender.ToLower()));
            }
            else
            {
                defendingPlayer =
                 room.players.Skip(nth - 1).FirstOrDefault(x => x.Name.StartsWith(itemToFind, StringComparison.CurrentCultureIgnoreCase)) ?? room.mobs.Skip(nth - 1).FirstOrDefault(x => x.Name.ToLower().Contains(itemToFind.ToLower()));
            }


            if (defendingPlayer == null)
            {
                HubContext.Instance.SendToClient("No one here", attacker.HubGuid);
                attacker.ActiveFighting = false;
                attacker.Target = null;
                attacker.Status = Player.PlayerStatus.Standing;

                return null;
            }

            if (attacker.Name.Equals(defendingPlayer.Name))
            {
                HubContext.Instance.SendToClient("You can't kill yourself", attacker.HubGuid);
                return null;
            }


            if (attacker.HitPoints <= 0)
            {
                HubContext.Instance.SendToClient("You cannot attack anything while dead", attacker.HubGuid);
                attacker.Status = Player.PlayerStatus.Standing;
                attacker.ActiveFighting = false;
                attacker.Target = null;
                return null;
            }

            if (defendingPlayer.HitPoints <= 0)
            {
                HubContext.Instance.SendToClient("They are already dead.", attacker.HubGuid);
                attacker.Status = Player.PlayerStatus.Standing;
                attacker.ActiveFighting = false;
                attacker.Target = null;
                return null;
            }

            return defendingPlayer;
        }

        private static async Task HitTarget(Player attacker, Player defender, Room room, int delay)
        {

            try
            {

                if (attacker.Target == null)
                {
                    return;
                }

                while (attacker.HitPoints > 0 && defender.HitPoints > 0)
                {
                    if (attacker.Status == Player.PlayerStatus.Stunned)
                    {

                        await Task.Delay(attacker.StunDuration);
                        attacker.StunDuration = 0;
                        attacker.Status = Player.PlayerStatus.Fighting;
                        continue;

                    }

                    //if (!attacker.ActiveFighting || !defender.ActiveFighting)
                    //{
                    //    return;
                    //}



                    //check still here
                    Player target = FindValidTarget(room, defender.Name, attacker);


                    if (target == null)
                    {
                        HubContext.Instance.SendToClient("No one here by that name", attacker.HubGuid);
                        return;
                    }

                    bool canAttack = CanAttack(attacker, defender);

                    if (canAttack)
                    {
                        bool alive = IsAlive(attacker, defender);


                        if (attacker.Target == null)
                        {
                            attacker.Status = Player.PlayerStatus.Standing;
                            attacker.ActiveFighting = false;
                            return;
                        }

                        if (alive && attacker.Status != Player.PlayerStatus.Busy)
                        {

                            await Task.Delay(delay);

                            if (attacker.Target == null)
                            {
                                attacker.Status = Player.PlayerStatus.Standing;
                                attacker.ActiveFighting = false;
                                return;
                            }

                            double offense = Offense(attacker);
                            double evasion = Evasion(defender);

                            double toHit = (offense / evasion) * 100;
                            int chance = D100();

                            Player checkAttackerAgainAfterDelay = FindValidTarget(room, defender.Name, attacker);
                            Player checkDefenderAgainAfterDelay = FindValidTarget(room, attacker.Name, defender);

                            if (checkAttackerAgainAfterDelay == null || checkDefenderAgainAfterDelay == null)
                            {
                                //Fixes bug where fleeing you would still get hit and hit back while in another room.
                                if (attacker.Target.HitPoints <= 0)
                                {

                                    attacker.Status = Player.PlayerStatus.Standing;
                                    attacker.ActiveFighting = false;
                                    attacker.Target = null;

                                }

                                return;
                            }




                            ShowAttack(attacker, defender, room, toHit, chance, null, true);






                            if (attacker.Type == Player.PlayerTypes.Player)
                            {
                                Score.UpdateUiPrompt(attacker);
                            }
                            if (defender.Type == Player.PlayerTypes.Player)
                            {
                                Score.UpdateUiPrompt(defender);
                            }
                        }
                        else
                        {
                            if (attacker.Target.HitPoints <= 0)
                            {
                                attacker.Status = Player.PlayerStatus.Standing;
                                attacker.ActiveFighting = false;
                                attacker.Target = null;
                            }
                        }



                    }
                    else
                    {
                        if (attacker.Target.HitPoints <= 0)
                        {
                            attacker.Status = Player.PlayerStatus.Standing;
                            attacker.ActiveFighting = false;
                            attacker.Target = null;
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "fight"
                };

                Save.LogError(log);

            }

        }


        /// <summary>
        /// Finds the defending player or NPC
        /// </summary>
        /// <param name="room">Current room</param>
        /// <param name="defender">Player or NPC name</param>
        /// <returns></returns>
        public static Player FindTarget(Room room, string defender)
        {
            //BUG: Can't find correct player of same name. e.g bill, billy. if billy want to kill bill we need to remove billy from the list so they don't find themseleves 1st.
            //BUG: no nth search here 2.billy
            Player target =
                room.players.Where(x => x.HitPoints > 0).ToList().FirstOrDefault(
                    x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                ?? room.mobs.Where(x => x.HitPoints > 0).ToList().FirstOrDefault(x => x.Name.ToLower().Contains(defender.ToLower()));

            return target;
        }

        public static bool CanAttack(Player attacker, Player defender)
        {

            bool canAttack = attacker.Target.Name == defender.Name;

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Resting)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Incapitated)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Dead)
            {
                canAttack = false;
            }


            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Ghost)
            {
                canAttack = false;
            }


            //using skill, casting. 
            // probably should set busy when picking up items(disarmed weapon)
            // wielding an item 
            // getting potion
            // quaffing
            // disallow changing armour while in combat
            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Busy)
            {
                canAttack = false;
            }

            return canAttack;
        }

        public static int CalculateDamageReduction(Player defender, int damage)
        {
            int ArRating = ArmourRating(defender);
            var armourReduction = ArRating / (double)damage;

            if (armourReduction > 4)
            {
                armourReduction = 4;
            }
            else if (armourReduction <= 0)
            {
                armourReduction = 1;
            }

            return (int)armourReduction;
        }

        public static Item GetAttackerWepon(Player attacker)
        {
            //currently everyone is right handed :-O
            var wielded = attacker.Equipment.Wielded;

            if (wielded == "Nothing")
            {
                return null;
            }

            var weapon = attacker.Inventory.FirstOrDefault(x => x.name.ToLower().Equals(wielded.ToLower()) && x.eqSlot.Equals(Item.EqSlot.Wielded));

            return weapon;
        }

        public static int HandToHandDamage(Player attacker)
        {
            var die = new PlayerStats();
            var handToHandDam = die.dice(1, 6);


            double handToHandSkill = attacker.Skills.Find(x => x.Name.Equals("HandToHand"))?.Proficiency ?? 0;

            if (attacker.Type == Player.PlayerTypes.Mob)
            {
                handToHandSkill = 1;
            }

            var damage = CalculateSkillBonus(handToHandSkill, handToHandDam);

            return damage;
        }

        /// <summary>
        /// Get the damage based on the skill proficiency
        /// </summary>
        /// <param name="skillProficiency">decimal of the skill Proficiency 0.1 - 1</param>
        /// <param name="damage">the dice roll for the skill or damage</param>
        /// <returns>int of damage</returns>
        public static int CalculateSkillBonus(double skillProficiency, int damage)
        {
            var skillModifier = (skillProficiency / damage) * 100;


            return (int)((skillModifier + damage) * 1);
        }

        /// <summary>
        ///  (Weapon Damage * Strength Modifier * Condition Modifier * Critical Hit Modifier) / Armor Reduction.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public static int Damage(Player attacker, Player defender, int criticalHit)
        {
            // (Weapon Damage * Strength Modifier * Condition Modifier * Critical Hit Modifier) / Armor Reduction.

            int damage;
            var helper = new Helpers();
            var weapon = GetAttackerWepon(attacker);

            if (weapon != null)
            {

                damage = helper.dice(1, weapon.stats.damMin, weapon.stats.damMax);
            }
            else
            {
                //Unarmed so use hand to hand
                damage = helper.dice(1, 1, 6);
            }

            if (attacker.Type == Player.PlayerTypes.Mob && attacker.MobAttackType != Player.MobAttackTypes.Punch)
            {
                damage = helper.dice(1, attacker.MobAttackStats.damMin, attacker.MobAttackStats.damMax);
            }

            if (Skill.CheckPlayerHasSkill(attacker, EnhancedDamage.EnhancedDamageAb().Name))
            {
                var chanceOfSuccess = Helpers.Rand(1, 100);
                var enhancedDamageSkill =attacker.Skills.FirstOrDefault(x => x.Name.Equals(EnhancedDamage.EnhancedDamageAb().Name));

                if (enhancedDamageSkill !=  null && enhancedDamageSkill.Proficiency > chanceOfSuccess)
                {
                    damage = damage + enhancedDamageSkill.Proficiency / damage * 10;
                }
            }

            var armourReduction = CalculateDamageReduction(defender, damage);

            var strengthMod = 0.5 + attacker.Strength / (double)100;
            var levelDif = attacker.Level - defender.Level <= 0 ? 1 : attacker.Level - defender.Level;
            var levelMod = levelDif / 2 <= 0 ? 1 : levelDif / 2;
            var enduranceMod = attacker.MovePoints / (double)attacker.MaxMovePoints;

            if (defender.Status == Player.PlayerStatus.Sleeping || defender.Status == Player.PlayerStatus.Stunned || defender.Status == Player.PlayerStatus.Resting)
            {
                criticalHit = 2;
            }


            int totalDamage = (int)(damage * strengthMod * criticalHit * levelMod);

            if (armourReduction > 0)
            {
                totalDamage = totalDamage / armourReduction;
            }

            if (totalDamage <= 0)
            {
                totalDamage = 1;
            }


            return totalDamage;
        }

        public static int ArmourRating(Player defender)
        {
            return 1 + defender.ArmorRating;
        }

        public static void CheckWeaponCondition(Item item, Player Attacker)
        {
            var skillProf = Attacker.Skills.FirstOrDefault(x => x.WeaponType.Equals(item.weaponType));

            if (skillProf != null)
            {
                var chanceOfdamage = Helpers.Rand(1, 105);

                if (skillProf.Proficiency <= chanceOfdamage)
                {
                    item.Condition -= Helpers.Rand(1, 5);

                    if (item.Condition > 0)
                    {
                        HubContext.Instance.SendToClient("<span style='color:goldenrod'>Your weapon takes some damage.</span>", Attacker.HubGuid);

                        var chanceToLearn = Helpers.Rand(1, 4);

                        if (chanceToLearn >= 4)
                        {
                            Player.LearnFromMistake(Attacker, skillProf, 100);

                        }

                    }
                    else
                    {
                        item.name = "broken " + item.name;
                        item.Condition = 0;
                        item.location = Item.ItemLocation.Inventory;
                        Attacker.Equipment.Wielded = "Nothing";

                        HubContext.Instance.SendToClient("<span style='color:goldenrod'>Your weapon breaks.</span>", Attacker.HubGuid);

                        Score.UpdateUiInventory(Attacker);
                    }



                }
            }


        }

        public static KeyValuePair<string, string> WeaponAttackName(Player attacker, Skill skillUsed)
        {
            if (skillUsed == null)
            {

                if (attacker.Type == Player.PlayerTypes.Mob && attacker.MobAttackType != Player.MobAttackTypes.Punch)
                {
                    return new KeyValuePair<string, string>(attacker.MobAttackType.ToString().ToLower(), attacker.MobAttackType.ToString().ToLower());
                }

                var wielded = attacker.Equipment.Wielded;
                Item weapon = null;
                if (wielded == "Nothing")
                {
                    return new KeyValuePair<string, string>("punch", "punches");
                }

                //find weapon
                weapon = attacker.Inventory.Find(x => x.name.Equals(wielded) && x.eqSlot.Equals(Item.EqSlot.Wielded));

                if (weapon != null)
                {

                    return new KeyValuePair<string, string>(weapon.name.ToLower(), weapon.name.ToLower());
                }

                return new KeyValuePair<string, string>("hit", "hit");

            }



            //TODO mob attack name


            //Skill / spell name here
            return new KeyValuePair<string, string>(skillUsed.Name.ToLower(), skillUsed.Name.ToLower());

        }

        public static int CriticalHit(double toHit, int chance)
        {
            var die = new PlayerStats();
            var chanceMod = die.dice(1, 4);
            var critical = chance * chanceMod;

            if (toHit >= chance)
            {
                return 1;
            }
            return 1;
        }

        public static string ShowMobHeath(Player defender)
        {
            var percent = defender.HitPoints / (double)defender.MaxHitPoints * 100;

            switch ((int)percent)
            {
                case 100:
                    return "is in excellent condition.";
                default:
                    if (percent >= 90)
                    {
                        return "has a few scratches.";
                    }
                    else if (percent >= 75)
                    {
                        return "has some small wounds and bruises.";
                    }
                    else if (percent >= 50)
                    {
                        return "has quite a few wounds.";
                    }
                    else if (percent >= 30)
                    {
                        return "has some big nasty wounds and scratches.";
                    }
                    else if (percent >= 15)
                    {
                        return "looks pretty hurt.";
                    }
                    else if (percent >= 0)
                    {
                        return "is in awful condition.";
                    }
                    else
                    {
                        return "is bleeding awfully from big wounds.";
                    }
            }
        }

        public static string ShowStatus(Player player)
        {
            if (player.Status == Player.PlayerStatus.Stunned)
            {
                return "<span style='color:red'> [Stunned]</span>";
            }

            return string.Empty;

        }

        /// <summary>
        /// Shows attack and damage to player
        /// </summary>
        /// <param name="attacker">the attacker</param>
        /// <param name="defender">the defender</param>
        /// <param name="room">The rom</param>
        /// <param name="toHit">Target tohit value</param>
        /// <param name="chance">Chance to hit value</param>
        /// <param name="skillUsed">This is used for skills and spells only</param>
        /// <param name="damage">This is used for skills and spells only</param>
        public static void ShowAttack(Player attacker, Player defender, Room room, double toHit, int chance, Skill skillUsed, bool autohit = false, int damage = 0)
        {

            var numberOfAttacks = 1;

            if (attacker.Skills != null && autohit)
            {
                var secondAttack = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Second Attack"));
                var thirdAttack = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Third Attack"));

                if (secondAttack != null)
                {
                    var secondAttackProficiency = attacker.Type == Player.PlayerTypes.Mob ? 1 : secondAttack.Proficiency;

                    if (Helpers.SkillSuccess(secondAttackProficiency))
                    {
                        numberOfAttacks += 1;
                    }

                    if (thirdAttack != null)
                    {

                        var thirdAttackProficiency = attacker.Type == Player.PlayerTypes.Mob ? 1 : thirdAttack.Proficiency;

                        //higher chance of fail if second attack is not fully trained
                        if (Helpers.SkillSuccess(thirdAttackProficiency - (95 - secondAttackProficiency)))
                        {
                            numberOfAttacks += 1;
                        }
                    }
                }
            }


            for (int i = 0; i < numberOfAttacks; i++)
            {

                bool alive = IsAlive(attacker, defender);
                int IsCritical = CriticalHit(toHit, chance);

                if (alive)
                {
                    if (toHit > chance)
                    {

                        var dam = Damage(attacker, defender, IsCritical);

                        var damageText = DamageText(dam);

                        if (IsAlive(attacker, defender))
                        {

                            HubContext.Instance.SendToClient(
                                "Your " + WeaponAttackName(attacker, skillUsed).Key + " " + damageText.Value.ToLower() +
                                " " + Helpers.ReturnName(defender, attacker, null).ToLower() + " [" + dam + "]" + ShowStatus(defender), attacker.HubGuid);

                            HubContext.Instance.SendToClient("<span style=color:cyan>" + Helpers.ReturnName(defender, attacker, null) + " " + ShowMobHeath(defender) + "</span>", attacker.HubGuid);

                            if (attacker.Equipment.Wielded != "Nothing")
                            {

                                CheckWeaponCondition(
                                    attacker.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(attacker.Equipment.Wielded.ToLower())), attacker);

                            }

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, defender, null) + "'s " +
                                WeaponAttackName(attacker, skillUsed).Value + " " + damageText.Value.ToLower() +
                                " you [" + dam + "]" + ShowStatus(defender), defender.HubGuid);

                            foreach (var player in room.players)
                            {
                                if (player != attacker && player != defender)
                                {
                                    HubContext.Instance.SendToClient(
                                        Helpers.ReturnName(attacker, defender, null) + "'s " +
                                        WeaponAttackName(attacker, skillUsed).Value + " " + damageText.Value.ToLower() +
                                        " " + Helpers.ReturnName(defender, attacker, null) + ShowStatus(defender), player.HubGuid);
                                }


                            }

                            var rand = Helpers.Rand(1, 10);

                            if (rand == 1)
                            {

                                if (Skill.CheckPlayerHasSkill(defender, "Parry"))
                                {
                                    var chanceOfSuccess = Helpers.Rand(1, 100);
                                    var skill = defender.Skills.FirstOrDefault(x => x.Name.Equals("Parry"));
                                    if (skill != null && skill.Proficiency <= chanceOfSuccess)
                                    {
                                        Player.LearnFromMistake(defender, skill, 100);
                                    }
                                }
                            }
                            else if (rand == 2)
                            {
                                if (Skill.CheckPlayerHasSkill(defender, "Dodge"))
                                {
                                    var chanceOfSuccess = Helpers.Rand(1, 100);
                                    var skill = defender.Skills.FirstOrDefault(x => x.Name.Equals("Dodge"));
                                    if (skill != null && skill.Proficiency <= chanceOfSuccess)
                                    {
                                        Player.LearnFromMistake(defender, skill, 100);
                                    }
                                }
                            }
                            else if (rand == 3)
                            {
                
                                if (defender.Equipment.Shield != "Nothing" && Skill.CheckPlayerHasSkill(defender, "Shield Block"))
                                {
                                    var chanceOfSuccess = Helpers.Rand(1, 100);
                                    var skill = defender.Skills.FirstOrDefault(x => x.Name.Equals("Shield Block"));
                                    if (skill != null && skill.Proficiency <= chanceOfSuccess)
                                    {
                                        Player.LearnFromMistake(defender, skill, 100);
                                    }
                                }
                            }
                            else if (rand == 2)
                            {
                                if (Skill.CheckPlayerHasSkill(defender, "Tumble"))
                                {
                                    var chanceOfSuccess = Helpers.Rand(1, 100);
                                    var skill = defender.Skills.FirstOrDefault(x => x.Name.Equals("Tumble"));
                                    if (skill != null && skill.Proficiency <= chanceOfSuccess)
                                    {
                                        Player.LearnFromMistake(defender, skill, 100);
                                    }
                                }
                            }



                            defender.HitPoints -= dam;

                            if (defender.HitPoints < 0)
                            {
                                defender.HitPoints = 0;
                            }


                            if (!IsAlive(attacker, defender))
                            {
                                IsDead(attacker, defender, room);
                            }
                        }

                    }
                    else
                    {
                        if (IsAlive(attacker, defender))
                        {

                            //Randomly pick to output dodge, parry, miss
                            var rand = Helpers.Rand(1, 4);
                            string attackerMessage, defenderMessage, observerMessage;

                            if (rand <= 1)
                            {
                                attackerMessage = "Your " + WeaponAttackName(attacker, skillUsed).Key +
                                          " <span style='color:olive'>misses</span> " +
                                          Helpers.ReturnName(defender, attacker, null);

                                defenderMessage = Helpers.ReturnName(attacker, defender, null) + "'s " +
                                                  WeaponAttackName(attacker, skillUsed).Key +
                                                  " <span style='color:olive'>misses</span> you ";

                                observerMessage = Helpers.ReturnName(attacker, defender, null) + "'s " +
                                                  WeaponAttackName(attacker, skillUsed).Key +
                                                  " <span style='color:olive'>misses</span> " +
                                                  Helpers.ReturnName(defender, attacker, null);
                            }
                            else if (rand > 1 && rand <= 2)
                            {
                                attackerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>dodges</span> your " +
                                          WeaponAttackName(attacker, skillUsed).Key;

                                defenderMessage = "You <span style='color:olive'>dodge</span> " + Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Key;

                                observerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>dodges</span> " + Helpers.ReturnName(attacker, defender, null) + "'s" + WeaponAttackName(attacker, skillUsed).Key;


                            }
                            else
                            {
                                attackerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>parries</span> your " +
                                          WeaponAttackName(attacker, skillUsed).Key;

                                defenderMessage = "You <span style='color:olive'>parry</span> " + Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Key;

                                observerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>parries</span> " + Helpers.ReturnName(attacker, defender, null) + "'s" + WeaponAttackName(attacker, skillUsed).Key;

                            }

                            HubContext.Instance.SendToClient(attackerMessage, attacker.HubGuid);
                            HubContext.Instance.SendToClient(defenderMessage, defender.HubGuid);

                            foreach (var player in room.players)
                            {
                                if (player != attacker && player != defender)
                                {
                                    HubContext.Instance.SendToClient(
                                      observerMessage, player.HubGuid);
                                }
                            }

                        }
                    }



                }
            }


        }

        public string MissMessage(Player attacker, Player defender)
        {
            string[] missText = new[]
                                    {
                                        "You side step away from " + defender.Name + "'s attack",
                                        "You lean back out of the way of " + defender.Name + "'s attack",
                                        "You duck out of the way of " + defender.Name + "'s attack",
                                        "You weave out of the way of " + defender.Name + "'s attack"
                                    };

            return missText[Helpers.diceRoll.Next(missText.Length)];
        }


        public string HitMessage(Player attacker, Player defender)
        {
            string[] hitText = new[]
                                    {
                                        "You side step away from " + defender.Name + "'s attack",
                                        "You lean back out of the way of " + defender.Name + "'s attack",
                                        "You duck out of the way of " + defender.Name + "'s attack",
                                        "You weave out of the way of " + defender.Name + "'s attack"
                                    };

            return hitText[0];
        }

        public static KeyValuePair<string, string> DamageText(int damage)
        {
            switch (damage)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    return new KeyValuePair<string, string>("<span style='color:#2ecc71'>scratch</span>", "<span style='color:#2ecc71'>scratches</span>");
                case 5:
                case 6:
                case 7:
                case 8:
                    return new KeyValuePair<string, string>("<span style='color:#2ecc71'>graze</span>", "<span style='color:#2ecc71'>grazes</span>");
                case 9:
                case 10:
                case 11:
                case 12:
                    return new KeyValuePair<string, string>("<span style='color:#2ecc71'>hit</span>", "<span style='color:#2ecc71'>hits</span>");
                case 13:
                case 14:
                case 15:
                case 16:
                    return new KeyValuePair<string, string>("<span style='color:#2ecc71'>injure</span>", "<span style='color:#2ecc71'>injures</span>");
                case 17:
                case 18:
                case 19:
                case 20:
                    return new KeyValuePair<string, string>("<span style='color:yellow'>wound</span>", "<span style='color:yellow'>wounds</span>");
                case 21:
                case 22:
                case 23:
                case 24:
                    return new KeyValuePair<string, string>("<span style='color:yellow'>maul</span>", "<span style='color:yellow'>mauls</span>");
                case 25:
                case 26:
                case 27:
                case 28:
                    return new KeyValuePair<string, string>("<span style='color:yellow'>decimate</span>", "<span style='color:yellow'>decimates</span>");
                case 29:
                case 30:
                case 31:
                case 32:
                    return new KeyValuePair<string, string>("devastate", "devastates");
                case 33:
                case 34:
                case 35:
                case 36:
                    return new KeyValuePair<string, string>("maim", "maims");
                case 37:
                case 38:
                case 39:
                case 40:
                    return new KeyValuePair<string, string>("MUTILATE", "MUTILATES");
                case 41:
                case 42:
                case 43:
                case 44:
                    return new KeyValuePair<string, string>("DISEMBOWEL", "DISEMBOWELS");
                case 45:
                case 46:
                case 47:
                case 48:
                    return new KeyValuePair<string, string>("MASSACRE", "MASSACRES");
                case 49:
                case 50:
                case 51:
                case 52:
                    return new KeyValuePair<string, string>("*** DEMOLISH ***", "*** DEMOLISHS ***");
                default:
                    return new KeyValuePair<string, string>("*** ANNIHILATES ***", "*** ANNIHILATES ***"); ;
            }


        }

        public static void IsDead(Player attacker, Player defender, Room room)
        {
            if (defender.HitPoints <= 0)
            {

                var oldDef = defender;
                defender.Status = defender.Type == Player.PlayerTypes.Player ? PlayerSetup.Player.PlayerStatus.Ghost : PlayerSetup.Player.PlayerStatus.Dead;

                if (!string.IsNullOrEmpty(defender.EventDeath))
                {
                    CheckEvent.FindEvent(CheckEvent.EventType.Death, attacker, "death");
                }

                foreach (var player in room.players)
                {
                    if (player != defender)
                    {
                        HubContext.Instance.SendToClient("<span style='color:red'>You hear " + Helpers.ReturnName(defender, attacker, null).ToLower() + "'s death cry.</span>", player.HubGuid);
                        HubContext.Instance.SendToClient("<span style='color:red'>" + defender.Name + " is DEAD!!</span> ", player.HubGuid);
                      
                    }
                    else
                    {
                        HubContext.Instance.SendToClient("<span style='color:red'>You hear " + Helpers.ReturnName(defender, attacker, null).ToLower() + "'s death cry.</span>", player.HubGuid);
                        HubContext.Instance.SendToClient("<span style='color:red'>You are DEAD!!</span>", defender.HubGuid);
                    }
                }

 

                foreach (var exit in room.exits)
                {
                    var newRoom = Cache.ReturnRooms()
                        .FirstOrDefault(x => x.areaId == exit.areaId && x.area == exit.area && x.region == exit.region);

                    if (newRoom != null)
                    {
                        foreach (var player in newRoom.players)
                        {
                            HubContext.Instance.SendToClient("<span style='color:brown'>You hear someone's death cry.</span>", player.HubGuid);
                        }
                    }
                }

 
                using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
                {
                    var col = db.GetCollection<Deaths>("Deaths");

                    var mobDeath = new Deaths
                    {
                        RoomName = room.title,
                        Area = room.area,
                        AreaId = room.areaId,
                        Date = DateTime.UtcNow,
                        KilledBy = defender.Target != null ? defender.Target.Name : "someone",
                        Id = Guid.NewGuid(),
                        Type = defender.Type == Player.PlayerTypes.Mob ? Player.PlayerTypes.Mob.ToString() : Player.PlayerTypes.Player.ToString()
                    };


                    col.Insert(Guid.NewGuid(), mobDeath);
                }

                //  HubContext.Instance.SendToClient("accessing DB done", attacker.HubGuid);

                defender.Target = null;
                defender.ActiveFighting = false;


                //Turn corpse into room item
                var defenderCorpse = new Item
                {
                    type = Item.ItemType.Container,
                    equipable = false,
                    name = "The corpse of " + defender.Name,
                    container = true,
                    location = Item.ItemLocation.Room,
                    containerItems = new ItemContainer(),
                    description = new Description { look = "The slain corpse of " + defender.Name + " is here.", room = "The slain corpse of " + defender.Name },
                    Duration = 10
                };


                if (defender.Type == Player.PlayerTypes.Mob)
                {

                    foreach (var invItem in defender.Inventory)
                    {
                        HubContext.Instance.SendToClient(
                            "<span style='color:yellow'>You loot " + Helpers.ReturnName(null, null, invItem.name).ToLower() +
                            " from the corpse of " + Helpers.ReturnName(defender, attacker, string.Empty) + "</span>",
                            attacker.HubGuid);

                        invItem.location = Item.ItemLocation.Inventory;
 
                        Player.AddItem(attacker, invItem);

                    }

                    Score.UpdateUiInventory(attacker);
                }
                else
                {

                    foreach (var invItem in defender.Inventory)
                    {

                        invItem.location = Item.ItemLocation.Room;

                        defenderCorpse.containerItems.Add(invItem);

                    }
                }


                //reset defender
                defender.Inventory = new ItemContainer();

                defender.Equipment.Arms = "Nothing";
                defender.Equipment.Torso = "Nothing";
                defender.Equipment.Body = "Nothing";
                defender.Equipment.Face = "Nothing";
                defender.Equipment.Feet = "Nothing";
                defender.Equipment.Finger = "Nothing";
                defender.Equipment.Finger2 = "Nothing";
                defender.Equipment.Floating = "Nothing";
                defender.Equipment.Hands = "Nothing";
                defender.Equipment.Head = "Nothing";
                defender.Equipment.Held = "Nothing";
                defender.Equipment.Face = "Nothing";
                defender.Equipment.Legs = "Nothing";
                defender.Equipment.Feet = "Nothing";
                defender.Equipment.Light = "Nothing";
                defender.Equipment.Neck = "Nothing";
                defender.Equipment.Neck2 = "Nothing";
                defender.Equipment.Shield = "Nothing";
                defender.Equipment.Wielded = "Nothing";
                defender.Equipment.Waist = "Nothing";
                defender.Equipment.Wrist = "Nothing";
                defender.Equipment.Wrist2 = "Nothing";

                defender.ArmorRating = 0;

                defender.Experience = (int)(defender.Experience > 0 ? defender.Experience / 1.5 : 0);


                var oldRoom = room;
                room.items.Add(defenderCorpse);
                room.corpses.Add(defender);

                if (defender.Type == Player.PlayerTypes.Mob || string.IsNullOrEmpty(defender.HubGuid))
                {
                    room.mobs.Remove(defender);
                }
                else
                {
                    room.players.Remove(defender);
                    attacker.ActiveFighting = false;
                    attacker.Status = Player.PlayerStatus.Standing;

                    var recall = Cache.ReturnRooms().FirstOrDefault(x => x.title == "Temple of Tyr");
                    Movement.Teleport(defender, recall);
                    //Add slain player to recall
                    Score.ReturnScoreUI(defender);
                }



                Cache.updateRoom(room, oldRoom);

                var xp = new Experience();

                int xpGain = xp.GainXp(attacker, defender);
                attacker.Experience += xpGain;
                attacker.TotalExperience += xpGain;
                HubContext.Instance.SendToClient("<span style='color:#2ecc71'>You gain " + xpGain + " experience points.</span>", attacker.HubGuid);

                xp.GainLevel(attacker);
                //calc xp
                //create corpse

                foreach (var player in room.players)
                {

                    var roomdata = LoadRoom.DisplayRoom(room, player.Name);
                    Score.UpdateUiRoom(player, roomdata);
                }

                if (attacker.HubGuid != null)
                {
                    room.fighting.Remove(attacker.HubGuid);
                }

                if (defender.HubGuid != null)
                {
                    room.fighting.Remove(defender.HubGuid);
                }

                //remove followers

                if (defender.Following != null)
                {

                    if (defender.Followers.Count > 0)
                    {
                        foreach (var follower in defender.Followers)
                        {
                            if (follower.HubGuid != null)
                            {
                                HubContext.Instance.SendToClient("You stop following " + defender.Name, follower.HubGuid);
                            }

                        }
                    }

                    defender.Followers = null;
                    defender.Following = null;

                }

                // check if defender is following?
                if (attacker.Followers?.FirstOrDefault(x => x.Equals(defender)) != null)
                {
                    attacker.Followers.Remove(defender);

                    if (attacker.HubGuid != null)
                    {
                        HubContext.Instance.SendToClient(defender.Name + " stops following you", attacker.HubGuid);
                    }

                }








                attacker.Target = null;
                attacker.Status = PlayerSetup.Player.PlayerStatus.Standing;
                attacker.ActiveFighting = false;


                Save.SavePlayer(defender);
                foreach (var quest in attacker.QuestLog.Where(x => x.Completed.Equals(false)))
                {
                    if (quest.Type == Quest.QuestType.Kill)
                    {
                        if (defender.Name == quest.QuestKill.Name)
                        {
                            quest.TotalQuestKills += 1;
                        }

                        if (quest.TotalQuestKills >= quest.QuestKills)
                        {
                            quest.Completed = true;

                            HubContext.Instance.SendToClient(quest.Name + " Quest Complete!", attacker.HubGuid);
                            if (quest.QuestGiver != null)
                            {
                                HubContext.Instance.SendToClient("Go back to " + quest.QuestGiver + " for your reward.",
                                    attacker.HubGuid);
                            }
                        }
                    }
                }

            }

        }

        public static bool IsAlive(Player attacker, Player defender)
        {
            if (defender.HitPoints <= 0)
            {
                return false;
            }

            if (attacker.HitPoints <= 0)
            {
                return false;
            }

            return true;
        }

        public static int D100()
        {
            var die = new PlayerStats();
            int roll = die.dice(1, 100);

            return roll;
        }

        public static double Offense(Player player)
        {
            //need error checking
            //workout wepean type check weapon skill
            //var weapon = player.Inventory.Find(x => x.location.Equals(Item.ItemLocation.Worn.ToString() && x.eqSlot.Equals(Item.EqSlot.Hand)));

            var weapon = player.Inventory.Find(x => x.location == Item.ItemLocation.Wield);
            Item.WeaponType weaponType;
            double weaponSkill = player.Type == Player.PlayerTypes.Mob ? 1 : 0;

            if (weapon != null)
            {
                weaponType = weapon.weaponType;
                weaponSkill = player.Skills.Find(x => x.Name == weaponType.ToString())?.Proficiency ?? 0;

            }
            else
            {
                //Hand To Hand
                weaponType = Item.WeaponType.HandToHand;
                weaponSkill = player.Skills.Find(x => x.Name == "Hand to Hand")?.Proficiency ?? 0;
            }

            double off = 0;
            int dexterity = player.Dexterity;
            int strength = player.Strength;

            weaponSkill = player.Type == Player.PlayerTypes.Mob ? 1 : 0;

            off = weaponSkill + (strength / (double)5) * (0.75 + 0.5 * player.MovePoints / (double)player.MaxMovePoints);


            //Based on skill and a random number, an Offensive Force / Factor(OF) is generated.
            //This number is bonused by the user's Agility as modified by the weapon's balance.
            //    The number is compared to the effective score of the defender's combined Evasion, Parry, and Shield. From this a chance of hitting is determined. At exactly equal OF and defense, the chance to hit is 66%.
            //    There is always a chance to both hit or miss, regardless of the numbers. 
            return off;
        }

        public static double Evasion(Player player)
        {
            var dodge = player.Skills.FirstOrDefault(x => x.Name.Equals("Dodge"));
            var parry = player.Skills.FirstOrDefault(x => x.Name.Equals("Parry"));
            var block = player.Skills.FirstOrDefault(x => x.Name.Equals("Shield Block"));
            var tumble = player.Skills.FirstOrDefault(x => x.Name.Equals("Tumble"));

            double blockSkill = player.Equipment.Shield == "Nothing" ? 0 : block?.Proficiency / (double)95 * 10 ?? 0;
            double parrySkill = parry?.Proficiency / (double)95 * 10 ?? 0;
            double dodgeSkill = dodge?.Proficiency / (double)95 * 10 ?? 0;
            double tumbleSkill = tumble?.Proficiency / (double)95 * 10 ?? 0;

            if (player.Type == Player.PlayerTypes.Mob)
            {
                blockSkill = 1;
                parrySkill = 1;
                dodgeSkill = 1;
                tumbleSkill = 1;
            }



            int dexterity = player.Dexterity;


            var hasHaste = player.Effects?.FirstOrDefault(
                             x => x.Name.Equals("Haste", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (hasHaste)
            {
                dexterity += 5;
            }

            //((Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue)

            double evade = blockSkill + parrySkill + dodgeSkill + tumbleSkill +(dexterity / (double)5) * (0.75 + 0.5 * (player.MovePoints / (double)player.MaxMovePoints));

            return evade;
        }


    }
}
