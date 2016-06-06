using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core
{
   public class Helpers
    {

        public static Random diceRoll = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Rolls dice for comabt, spells, skills etc
        /// </summary>
        /// <param name="number">Number of dice to roll</param>
        /// <param name="minSize">Min size of dice</param>
        /// <param name="maxSize">Max Size of Dice</param>
        /// <returns></returns>
        public int dice(int number, int minSize, int maxSize)
        {
            int sum = 0;

            for (int i = 0; i < number; i++)
            {
                sum += diceRoll.Next(minSize, maxSize);
            }

            return sum;
        }
    }
}
