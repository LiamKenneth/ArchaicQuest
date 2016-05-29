using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    public class PlayerStats
    {

    
       public static Random diceRoll = new Random((int)DateTime.Now.Ticks);
    
        public int[] rollStats()
        {

            int[] test = new int[6];


            for (int i = 0; i < 6; i++)
            {
                var result = dice(3, 6) + 3;
                test[i] = result;
            }


            return test;
        }

        public int dice(int number, int size)
        {
            int sum = 0;
            
            for (int i = 0; i < number; i++)
            {
                sum += diceRoll.Next(1, size);
            }

            return sum;
        }

        public static int GetHp(Player player)
        {
            return player.HitPoints;
        }

        public static int GetMaxHp(Player player)
        {
            return player.MaxHitPoints;
        }

        public static int GetMana(Player player)
        {
            return player.ManaPoints;
        }

        public static int GetMaxMana(Player player)
        {
            return player.MaxManaPoints;
        }

        public static int GetMove(Player player)
        {
            return player.MovePoints;
        }

        public static int GetMaxMove(Player player)
        {
            return player.MaxMovePoints;
        }

    }
}
