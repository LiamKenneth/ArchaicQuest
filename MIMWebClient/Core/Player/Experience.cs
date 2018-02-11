using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Controllers;
using MIMWebClient.Core.Player.Classes;

namespace MIMWebClient.Core.Player
{
    using MIMWebClient.Core.Events;

    public class Experience
    {


        public int VictimDifficulty(PlayerSetup.Player player, PlayerSetup.Player victim)
        {
            var difficulty = victim.Level * victim.Level * 2  + (victim.Strength + victim.Constitution + victim.Dexterity + victim.Intelligence + victim.Wisdom + victim.Charisma) * 10 + 
                         (victim.DamRoll + victim.HitRoll) * 2 + (victim.MaxHitPoints + victim.MaxManaPoints + victim.MaxMovePoints) * 2;


            if (player.Level * 3 < victim.Level)
            {
                difficulty = difficulty * 3;
            }
            else if (player.Level * 2 < victim.Level)
            {
                difficulty = difficulty * 2;
            }


            if (player.Level > victim.Level * 3)
            {
                difficulty = difficulty / 3;
            }
            else if (player.Level >=victim.Level * 2)
            {
                difficulty = difficulty / 2;
            }

            if (player.Level * 5 > victim.Level)
            {
                difficulty = difficulty/4;
            }




            return difficulty;
        }

        /// <summary>
        /// figures out XP gained = (Mob HP x Mob Level) / (Player HP x Player Level) x 50
        /// </summary>
        /// <param name="player">Player obj</param>
        /// <param name="victim">Viction obj</param>
        /// <returns>Xp gained</returns>
        public int GainXp(PlayerSetup.Player player, PlayerSetup.Player victim)
        {
            const int maxXpGain = 10000;

            var exp = (int)((victim.MaxHitPoints * victim.Level) / (double)(player.MaxHitPoints * player.Level) * 500);

            if (exp > maxXpGain)
            {
                exp = maxXpGain;
            }


            return exp;
        }
 

        public bool HasGainedLevel(PlayerSetup.Player player)
        {         
            return player.Experience >= player.ExperienceToNextLevel;
        }

        public void GainLevel(PlayerSetup.Player player)
        {
            if (player.Type == PlayerSetup.Player.PlayerTypes.Mob)
            {
                return;
            }

            if (HasGainedLevel(player))
            {
                player.Level += 1;
                var carryOverExcessXp = Math.Max(player.Experience - player.ExperienceToNextLevel, 0);
                player.Experience = carryOverExcessXp;
                player.ExperienceToNextLevel = GetTNL(player);

                var selectedClass = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.ToLower().StartsWith(player.SelectedClass, StringComparison.CurrentCultureIgnoreCase));
                var dice = new Helpers();
                var maxHPGain = selectedClass.Value.MaxHpGain;
                var maxEnduranceGain = selectedClass.Value.MaxEnduranceGain;

                if (Skill.CheckPlayerHasSkill(player, "Toughness"))
                {
                    var chanceOfSuccess = Helpers.Rand(1, 100);

                    var hasSkill = player.Skills.FirstOrDefault(x => x.Name.Equals("Toughness"));
                    if (hasSkill != null && hasSkill.Proficiency >= chanceOfSuccess)
                    {
                        maxHPGain *= 3;
                        maxEnduranceGain *= 3;
                    }
                }

                var hpGain = dice.dice(1, selectedClass.Value.MinHpGain, maxHPGain);
                var manaGain = dice.dice(1, selectedClass.Value.MinManaGain, selectedClass.Value.MaxManaGain);
                var enduranceGain = dice.dice(1, selectedClass.Value.MinEnduranceGain, maxEnduranceGain);
                //tell user they have gained a level

                player.MaxHitPoints += hpGain;
                player.MaxManaPoints += manaGain;
                player.MaxMovePoints += enduranceGain;

                //tell user how much HP / mana / mvs / practices / trains they have gained

                HubContext.Instance.SendToClient("<span style='color:yellow'>Congratulations, you are now level " + player.Level + ". You have gained. HP: " + hpGain + " Mana: " + manaGain + " End: " + enduranceGain + "</span>", player.HubGuid);

                Score.ReturnScoreUI(player);

                Save.SavePlayer(player);             
                //save player

                //check player hasn't leveled again
                GainLevel(player);

                var discordToSay = player.Name + " is now level " + player.Level;

                var discordBot = new HomeController();
                discordBot.PostToDiscord(discordToSay);
                discordBot.Dispose();
            }
        }

        public int GetTNL(PlayerSetup.Player player)
        {

            //find race add them all also
           // var selectedRace = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.ToLower().StartsWith(player.SelectedClass, StringComparison.CurrentCultureIgnoreCase));
           //create the other classes
            var selectedClass = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.ToLower().StartsWith(player.SelectedClass, StringComparison.CurrentCultureIgnoreCase));
            var tnl = player.Level * player.Level * 1 * selectedClass.Value.ExperienceModifier;

            return tnl;
        }
    }
}
