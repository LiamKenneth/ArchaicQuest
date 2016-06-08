using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Player
{
    public class Experience
    {

        //Red:     Mob Level >= Char Level + 5
        //Orange:  Mob Level = Char Level + (3 or 4)
        //Yellow:  Mob Level = Char Level ± 2
        //Green:   Mob Level <= Char Level - 3, but above Gray Level
        //Gray:    Mob Level <= Gray Level

//        XP = (Char Level * 5) + 45, where Char Level = Mob Level, for mobs in Azeroth
//XP = (Char Level* 5) + 235, where Char Level = Mob Level, for mobs in Outland
//XP = (Char Level* 5) + 580, where Char Level = Mob Level, for mobs in Northrend
//XP = (Char Level* 5) + 1878, where Char Level = Mob Level, for mobs in Cataclysm

        private int CheckMobLevelBonus(PlayerSetup.Player player, PlayerSetup.Player mob)
        {
            int mobLevel = mob.Level;
            int playerLevel = player.Level;

            if (playerLevel + 5 <= mobLevel)
            {
                if (playerLevel + 10 <= mobLevel)
                {
                    return 5;
                }
                return 4;
            }

            switch (mobLevel - playerLevel)
            {
                case 3:
                    return 3;
                case 2:
                case 1:
                case 0:
                case -1:
                case -2:
                    return 2;
                default:

                    if (playerLevel <= 5)
                    {
                        return 1;
                    }

                    if (playerLevel <= 39)
                    {
                        if (mobLevel <= (playerLevel - 5 - Math.Floor((double)playerLevel / 10)))
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                    //player over lvl 39:
                    if (mobLevel <= (playerLevel - 1 - Math.Floor((double)playerLevel / 5)))
                    {
                        return 0;
                    }

                    return 1;
            }
        }

        private int XPLevelModifier(PlayerSetup.Player player)
        {
            if (player.Level < 10)
            {
                return 45;
            }

            if (player.Level < 20)
            {
                return 175;
            }

            if (player.Level < 30)
            {
                return 235;
            }

            if (player.Level < 40)
            {
                return 460;
            }

            if (player.Level < 50)
            {
                return 785;
            }

            return 935;

        }

        //The highest mob level that gives you no experience is the Gray Level. It varies based on your character level as follows: 
        //Char level  1-5:  Gray level = 0(all mobs give XP)
        //Char level  6-49: Gray level = Char level - FLOOR(Char level/10) - 5
        //Char level    50: Gray level = Char level - 10 = 40
        //Char level 51-59: Gray level = Char level - FLOOR(Char level/5) - 1
        //Char level 60-70: Gray level = Char level - 9 = 51 @ 60, 61 @ 70

        //The amount of experience you will get for a solo kill on a mob whose level is equal to your level is:

        //XP = (Char Level * 5) + 45, where Char Level = Mob Level, for mobs in Azeroth
        //XP = (Char Level* 5) + 235, where Char Level = Mob Level, for mobs in Outland
        //XP = (Char Level* 5) + 580, where Char Level = Mob Level, for mobs in Northrend
        //XP = (Char Level* 5) + 1878, where Char Level = Mob Level, for mobs in Cataclysm

        // XP = (Base XP) * (1 + 0.05 * (Mob Level - Char Level) ), where Mob Level > Char Level
        // This is the amount of experience you will get for a solo kill on a mob whose level is higher
        // than your level.This is known to be valid for up to Mob Level = Char Level + 4
        // (Orange and high-Yellow Mobs). Red mobs cap out at the same experience 
        // as orange.Higher elite mobs may also cap at the same amount(ie are not doubled).

        //lower level
        //For a given character level, the amount of XP given by lower-level mobs
        //is a linear function of the Mob Level.The amount of experience reaches
        //zero when the difference between the Char Level and Mob Level reaches
        //a certain point.This is called the Zero Difference value, and is given by:
        //ZD = 5, when Char Level = 1 - 7
        //ZD = 6, when Char Level = 8 - 9
        //ZD = 7, when Char Level = 10 - 11
        //ZD = 8, when Char Level = 12 - 15
        //ZD = 9, when Char Level = 16 - 19
        //ZD = 11, when Char Level = 20 - 29
        //ZD = 12, when Char Level = 30 - 39
        //ZD = 13, when Char Level = 40 - 44
        //ZD = 14, when Char Level = 45 - 49
        //ZD = 15, when Char Level = 50 - 54
        //ZD = 16, when Char Level = 55 - 59
        //ZD = 17, when Char Level = 60 - 79

        //Using the ZD values above, the formula for Mob XP for any mob of level lower than your character is:

        //XP = (Base XP) * (1 - (Char Level - Mob Level)/ZD )
        //where Mob Level<Char Level, and Mob Level> Gray Level

        //example

        //    using Char Level = 20.
        //so Gray Level = 13, by the table above.
        //killing any mob level 13 or lower will give 0 XP.
        //Basic XP is (20 * 5 + 45) = 145. Killing a level 20 mob will give 145 XP.
        //For a level 21 mob, we have XP = 145 * (1 + 0.05 * 1) = 152.2 rounded to 152 XP.
        //ZD is 11, from the table above.
        //For a level 18 mob, we have XP = 145 * (1 - 2 / 11) = 118.6 rounded to 119 XP.
        //For a level 16 mob, we have XP = 145 * (1 - 4 / 11) = 92.3 rounded to  92 XP.
        //For a level 14 mob, we have XP = 145 * (1 - 6 / 11) = 65.91 rounded to  66 XP.

        //Mobs that are flagged as elite will give twice the amount of experience as a normal mob for the same level.

        //Elite XP = 2 * XP

        public int MobXP(PlayerSetup.Player player, PlayerSetup.Player mob)
        {
            int xpGained = 0;
            int mobLvlBonus = CheckMobLevelBonus(player, mob);
            int levelModifier = XPLevelModifier(player);

            /// XP = (Base XP) * (1 + 0.05 * (Mob Level - Char Level) ), where Mob Level > Char Level
            /// 
            if (mob.Level > player.Level)
            {
                xpGained = (int)Math.Floor((player.Level * mobLvlBonus) * (1 + 0.10 * (mob.Level - player.Level)) + levelModifier);
            }
            else
            {
                xpGained = (int)Math.Floor((player.Level * mobLvlBonus) * 1 + 0.05 + levelModifier);
            }

            return xpGained;
        }
    }
}
