using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public int GainXp(PlayerSetup.Player player, PlayerSetup.Player victim)
        {
            const int maxXpGain = 10000;

            var exp = Math.Min(maxXpGain, VictimDifficulty(player, victim));

            if (victim.Type == PlayerSetup.Player.PlayerTypes.Mob)
            {
                exp += Math.Max(0, exp*Math.Min(4, victim.Level - player.Level))/8;
            }
            else
            {
                exp += Math.Max(0, exp*Math.Min(8, victim.Level - player.Level))/8;
            }

            exp = Math.Max(exp, 1);

            //XP gain should happen here
            // add to totalxp here
            //what about quest xp?/ explore xp / other types of xp success
            return exp;
        }
 

        public bool HasGainedLevel(PlayerSetup.Player player)
        {         
            return player.Experience >= player.ExperienceToNextLevel;
        }

        public void GainLevel(PlayerSetup.Player player)
        {
            if (HasGainedLevel(player))
            {
                player.Level += 1;
                var carryOverExcessXp = Math.Max(player.Experience - player.ExperienceToNextLevel, 0);
                player.Experience = carryOverExcessXp;
                player.ExperienceToNextLevel = GetTNL(player);

                var selectedClass = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.ToLower().StartsWith(player.SelectedClass, StringComparison.CurrentCultureIgnoreCase));
                var dice = new Helpers();

                var hpGain = dice.dice(1, selectedClass.Value.MinHpGain, selectedClass.Value.MaxHpGain);
                var manaGain = dice.dice(1, selectedClass.Value.MinHpGain, selectedClass.Value.MaxHpGain);
                var enduranceGain = dice.dice(1, selectedClass.Value.MinHpGain, selectedClass.Value.MaxHpGain);
                //tell user they have gained a level

                player.MaxHitPoints += hpGain;
                player.MaxManaPoints += manaGain;
                player.MaxMovePoints += enduranceGain;

                //tell user how much HP / mana / mvs / practices / trains they have gained

                HubContext.Instance.SendToClient("Congratulations, you are now level " + player.Level + ". You have gained. HP: " + hpGain + " Mana: " + manaGain + " End: " + enduranceGain, player.HubGuid);

                Save.SavePlayer(player);             
                //save player

                //check player hasn't leveled again
                GainLevel(player);
            }
        }

        public int GetTNL(PlayerSetup.Player player)
        {

            //find race add them all also
           // var selectedRace = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.ToLower().StartsWith(player.SelectedClass, StringComparison.CurrentCultureIgnoreCase));
           //create the other classes
            var selectedClass = PlayerClass.ClassList().FirstOrDefault(x => x.Value.Name.ToLower().StartsWith(player.SelectedClass, StringComparison.CurrentCultureIgnoreCase));
            var tnl = player.Level * player.Level * player.Level * 2 * selectedClass.Value.ExperienceModifier;

            return tnl;
        }
    }
}
