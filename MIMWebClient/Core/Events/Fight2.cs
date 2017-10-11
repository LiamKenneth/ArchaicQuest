using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MIMWebClient.Core.Loging;


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

            defender.Status = Player.PlayerStatus.Fighting;

            AddFightersIdtoRoom(attacker, defender, room, false);

            if (IsAlive(defender, attacker))
            {
                if (attacker.Target == null)
                {
                    attacker.Target = defender;
                    attacker.Status = Player.PlayerStatus.Fighting;
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


                if (attacker.Status == Player.PlayerStatus.Fighting)
                {


                    double offense = Offense(attacker);
                    double evasion = Evasion(defender);

                    double toHit = (offense / evasion) * 100;
                    int chance = D100();


                        ShowAttack(attacker, defender, room, toHit, chance, null);

                }

                if (!attacker.ActiveFighting)
                {
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

            if (room.mobs.Count <= 0)
            {
                HubContext.Instance.SendToClient("You don't see anything to kill that matches that name", attacker.HubGuid);
                attacker.ActiveFighting = false;
                attacker.Target = null;
                attacker.Status = Player.PlayerStatus.Standing;
            }

            if (room.players.Count <= 0)
            {
                HubContext.Instance.SendToClient("You don't see anything to kill that matches that name", attacker.HubGuid);
                attacker.ActiveFighting = false;
                attacker.Target = null;
                attacker.Status = Player.PlayerStatus.Standing;
            }

            if (nth == -1)
            {
                defendingPlayer =
                  room.players.Where(x => x.HitPoints > 0).ToList().FirstOrDefault(
                      x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                  ?? room.mobs.Where(x => x.HitPoints > 0).ToList().First(x => x.Name.ToLower().Contains(defender.ToLower()));
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



                while (attacker.HitPoints > 0 && defender.HitPoints > 0)
                {
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



                        if (alive && attacker.Status != Player.PlayerStatus.Busy)
                        {

                            await Task.Delay(delay);

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




                            ShowAttack(attacker, defender, room, toHit, chance, null);


                            attacker.MovePoints = attacker.MovePoints - Helpers.Rand(1, 5);

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

            Player target =
                room.players.Where(x => x.HitPoints > 0).ToList().FirstOrDefault(
                    x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                ?? room.mobs.Where(x => x.HitPoints > 0).ToList().First(x => x.Name.ToLower().Contains(defender.ToLower()));

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

        public static int CalculateDamageReduction(Player attacker, Player defender, int damage)
        {
            int ArRating = ArmourRating(defender);
            int armourReduction = ArRating / damage;

            if (armourReduction > 4)
            {
                armourReduction = 4;
            }
            else if (armourReduction <= 0)
            {
                armourReduction = 1;
            }

            return armourReduction;
        }

        public static Item GetAttackerWepon(Player attacker)
        {
            //currently everyone is right handed :-O
            var wielded = attacker.Equipment.Wielded;

            if (wielded == "Nothing")
            {
                return null;
            }

            var weapon = attacker.Inventory.FirstOrDefault(x => x.name.Equals(wielded) && x.location.Equals(Item.ItemLocation.Worn));

            return weapon;
        }

        public static int HandToHandDamage(Player attacker)
        {
            var die = new PlayerStats();
            var handToHandDam = die.dice(1, 6);

            double handToHandSkill = attacker.Skills.Find(x => x.Name.Equals("HandToHand"))?.Proficiency ?? 0;

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

            var weapon = GetAttackerWepon(attacker);

            if (weapon != null)
            {
                var helper = new Helpers();
                damage = helper.dice(1, weapon.stats.damMin, weapon.stats.damMax);
            }
            else
            {
                //Unarmed so use hand to hand
                damage = HandToHandDamage(attacker);
            }

            var armourReduction = CalculateDamageReduction(attacker, defender, damage);


            int totalDamage = (damage / armourReduction) * criticalHit;

            return totalDamage;
        }

        public static int ArmourRating(Player defender)
        {
            return 1 + defender.ArmorRating;
        }

        public static KeyValuePair<string, string> WeaponAttackName(Player attacker, Skill skillUsed)
        {
            if (skillUsed == null)
            {

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

                    return new KeyValuePair<string, string>(weapon.name, weapon.name);
                }
            
                    return new KeyValuePair<string, string>("hit", "hit");
                
                /// weapon.attackType = Item.AttackTypes.Slash;
                //add attack string to weapons

            }

            //  var checkPlural = new EnglishPluralizationService();


            //Skill / spell name here
            return new KeyValuePair<string, string>(skillUsed.Name, skillUsed.Name);

        }

        public static int CriticalHit(double toHit, int chance)
        {
            var die = new PlayerStats();
            var chanceMod = die.dice(1, 4);
            var critical = chance * chanceMod;

            if (toHit >= chance)
            {
                return 2;
            }
            return 1;
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
        public static void ShowAttack(Player attacker, Player defender, Room room, double toHit, int chance, Skill skillUsed, int damage = 0)
        {

            var numberOfAttacks = 1;
            if (attacker.Skills != null)
            {
                var secondAttack = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Second Attack"));
                var thirdAttack = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Third Attack"));

                if (secondAttack != null)
                {
                    if (Helpers.SkillSuccess(secondAttack.Proficiency))
                    {
                        numberOfAttacks += 1;
                    }

                    if (thirdAttack != null)
                    {
                        //higher chance of fail if second attack is not fully trained
                        if (Helpers.SkillSuccess(thirdAttack.Proficiency - (95 - secondAttack.Proficiency)))
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
                        
                        var dam = damage > 0 ? damage : Damage(attacker, defender, IsCritical);

                        var damageText = DamageText(dam);

                        if (IsAlive(attacker, defender))
                        {

                            HubContext.Instance.SendToClient(
                                "Your " + WeaponAttackName(attacker, skillUsed).Key + " " + damageText.Value.ToLower() +
                                " " + Helpers.ReturnName(defender, attacker, null) + " [" + dam + "]", attacker.HubGuid);

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, defender, null) + "'s " +
                                WeaponAttackName(attacker, skillUsed).Value + " " + damageText.Value.ToLower() +
                                " you [" + dam + "]", defender.HubGuid);

                            foreach (var player in room.players)
                            {
                                if (player != attacker && player != defender)
                                {
                                    HubContext.Instance.SendToClient(
                                        Helpers.ReturnName(attacker, defender, null) + "'s " +
                                        WeaponAttackName(attacker, skillUsed).Value + " " + damageText.Value.ToLower() +
                                        " " + Helpers.ReturnName(defender, attacker, null), player.HubGuid);
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

                                observerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>dodges</span>" + Helpers.ReturnName(defender, attacker, null) + "'s" + WeaponAttackName(attacker, skillUsed).Key;
                            }
                            else
                            {
                                attackerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>parries</span> your " +
                                          WeaponAttackName(attacker, skillUsed).Key;

                                defenderMessage = "You <span style='color:olive'>parry</span> " + Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Key;

                                observerMessage = Helpers.ReturnName(defender, attacker, null) + " <span style='color:olive'>parries</span>" + Helpers.ReturnName(defender, attacker, null) + "'s" + WeaponAttackName(attacker, skillUsed).Key;
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
                    return new KeyValuePair<string, string>("<span style='color:green'>scratch</span>", "<span style='color:green'>scratches</span>");
                case 5:
                case 6:
                case 7:
                case 8:
                    return new KeyValuePair<string, string>("<span style='color:green'>graze</span>", "<span style='color:green'>grazes</span>");
                case 9:
                case 10:
                case 11:
                case 12:
                    return new KeyValuePair<string, string>("<span style='color:green'>hit</span>", "<span style='color:green'>hits</span>");
                case 13:
                case 14:
                case 15:
                case 16:
                    return new KeyValuePair<string, string>("<span style='color:green'>injure</span>", "<span style='color:green'>injures</span>");
                case 17:
                case 18:
                case 19:
                case 20:
                    return new KeyValuePair<string, string>("<span style='color:green'>wound</span>", "<span style='color:green'>wounds</span>");
                case 21:
                case 22:
                case 23:
                case 24:
                    return new KeyValuePair<string, string>("<span style='color:green'>maul</span>", "<span style='color:green'>mauls</span>");
                case 25:
                case 26:
                case 27:
                case 28:
                    return new KeyValuePair<string, string>("<span style='color:yellow'>decimate</span>", "<span style='color:yellow'decimates</span>");
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

                if (!string.IsNullOrEmpty(defender.EventDeath))
                {
                    CheckEvent.FindEvent(CheckEvent.EventType.Death, attacker, "death");
                }

                foreach (var player in room.players)
                {
                    if (player != defender)
                    {
                        HubContext.Instance.SendToClient(defender.Name + " dies ", player.HubGuid);
                    }
                    else
                    {
                        HubContext.Instance.SendToClient("You die", defender.HubGuid);
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
                        KilledBy = defender.Target.Name,
                        Id = Guid.NewGuid(),
                        Type = defender.Type == Player.PlayerTypes.Mob ? Player.PlayerTypes.Mob.ToString() : Player.PlayerTypes.Player.ToString()
                    };


                    col.Insert(Guid.NewGuid(), mobDeath);
                }



                defender.Target = null;
                defender.ActiveFighting = false;
                defender.Status = Player.PlayerStatus.Ghost;


                //Turn corpse into room item
                var defenderCorpse = new Item
                {
                    type = Item.ItemType.Container,
                    equipable = false,
                    name = "The corpse of " + defender.Name,
                    container = true,
                    containerItems = new ItemContainer(),
                    description = new Description { look = "The slain corpse of " + defender.Name + " is here.", room = "The slain corpse of " + defender.Name }
                };

                foreach (var invItem in defender.Inventory)
                {
                    invItem.location = Item.ItemLocation.Room;
                    defenderCorpse.containerItems.Add(invItem);

                }



                var oldRoom = room;
                room.items.Add(defenderCorpse);
                room.corpses.Add(defender);

                if (defender.Type == Player.PlayerTypes.Mob || string.IsNullOrEmpty(defender.HubGuid))
                {
                    room.mobs.Remove(defender);
                }
                else
                {
                    //room.players.Remove(defender);
                    //Add slain player to recall
                }



                defender.Status = defender.Type == Player.PlayerTypes.Player ? PlayerSetup.Player.PlayerStatus.Ghost : PlayerSetup.Player.PlayerStatus.Dead;

                Cache.updateRoom(room, oldRoom);

                var xp = new Experience();

                int xpGain = xp.GainXp(attacker, defender);
                attacker.Experience += xpGain;
                attacker.TotalExperience += xpGain;
                HubContext.Instance.SendToClient(xpGain + "XP", attacker.HubGuid);

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
            double weaponSkill = 0;

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


            int dexterity = player.Dexterity;
            int strength = player.Strength;


            //(Weapon Skill + (Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue);

            double off = weaponSkill + (strength / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

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

            double blockSkill = string.IsNullOrEmpty(player.Equipment.Shield) ? 0 : block?.Proficiency / 95 ?? 0;
            double parrySkill = parry?.Proficiency / 95 ?? 0;
            double dodgeSkill = dodge?.Proficiency / 95 ?? 0;  
            int dexterity = player.Dexterity;


            var hasHaste = player.Effects?.FirstOrDefault(
                             x => x.Name.Equals("Haste", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (hasHaste)
            {
                dexterity += 5;
            }

            //((Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue)

            double evade =  blockSkill + parrySkill + dodgeSkill + (dexterity / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

            return evade;
        }


    }
}
