using MIMWebClient.Core.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core
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

            try
            {
                //Hack - something in update is passing in 0 for max size
                if (minSize == maxSize)
                {
                    maxSize += 1;
                }

                if (maxSize == 0)
                {
                    maxSize = minSize + 1;
                }
                //Hack

                for (int i = 0; i < number; i++)
                {
                    sum += diceRoll.Next(minSize, maxSize);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return sum;

        }

        /// <summary>
        /// Returns a random number
        /// </summary>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static int Rand(int minSize, int maxSize)
        {
            
            return diceRoll.Next(minSize, maxSize);

        }

        /// <summary>
        /// Returns percentage of maximum value
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public static int GetPercentage(int currentValue, int maximumValue)
        {
            var percent = (int)Math.Round(currentValue * 100M / maximumValue);

 
            return percent <= 0 ? 1 : percent;

        }

        /// <summary>
        /// Returns a random string.
        /// </summary>
        /// <param name="possible"></param>
        /// <returns></returns>
        public static String RandomString(params String[] possible)
        {
            return possible[diceRoll.Next(0, possible.Count())];
        }

        /// <summary>
        /// Find player? not used will be deleted after 30 days of no use
        /// </summary>
        /// <param name="Room"></param>
        /// <param name="player"></param>
        /// <returns></returns>

        public static PlayerSetup.Player FindPlayer(Room.Room Room, PlayerSetup.Player player)
        {
            var playerFound = Room.players.FirstOrDefault(x => x.Name.StartsWith(player.Name, StringComparison.CurrentCultureIgnoreCase))
                             ?? Room.mobs.FirstOrDefault(x => x.Name.ToLower().Contains(player.Name.ToLower()));

            return playerFound;

        }

        /// <summary>
        /// Returns a or an if name starts with a Vowel 
        /// but if the object has a name it returns that.
        /// 
        /// e.g an apple / Liam / 
        /// </summary>
        /// <param name="objName"></param>
        ///  /// <param name="itemName"></param>
        /// <returns></returns>
        public static string ReturnName(PlayerSetup.Player player, PlayerSetup.Player target, string itemName)
        {

            if (player == null && target == null)
            {

                var itemResult = AvsAnLib.AvsAn.Query(itemName).Article;


                return FirstLetterToUpper(itemResult + " " + itemName);
            }

            //TODO: ah a bug, if you have detects it will always say someone
            if (player.invis == true && target.DetectInvis == false || player.hidden == true && target.DetectHidden == false)
            {
                return "Someone";
            }


            string name;

            if (player.KnownByName)
                {
                    name = player.Name;
                }
                else
                {
                var result = AvsAnLib.AvsAn.Query(player.Name).Article;
               
                name = result + " " + player.Name;
                }


                return name;

           


        }
 
        /// <summary>
        /// Returns captalised string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ReturnHisOrHers(PlayerSetup.Player player, PlayerSetup.Player target, bool plural = true)
        {
            if (player.invis == true && target.DetectInvis == false || player.hidden == true && target.DetectHidden == false)
            {
                return "their";
            }

            if (plural)
            {
                return player.Gender == "male" ? "his" : "hers";
            }

            return player.Gender == "male" ? "his" : "her";

        }

    }
}
