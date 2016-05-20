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
    }
}
