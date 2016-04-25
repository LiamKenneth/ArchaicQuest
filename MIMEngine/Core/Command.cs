using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core
{
    public class Command
    {

        public static void parseCommand(string input)
        {

            if (!string.IsNullOrEmpty(input))
            {
              var command = generateCommands().FirstOrDefault(x => x.Key.StartsWith(input));

              
            }
        }

        public static Dictionary<string, Action> generateCommands()
        {
            Dictionary<string, Action> commands = new Dictionary<string, Action>();


            commands.Add("n", () => { Console.WriteLine("n"); });
            commands.Add("north", () => { Console.WriteLine("n"); });
            commands.Add("e", () => { Console.WriteLine("e"); });
            commands.Add("east", () => { Console.WriteLine("e"); });
            commands.Add("s", () => { Console.WriteLine("s"); });
            commands.Add("south", () => { Console.WriteLine("s"); });
            commands.Add("w", () => { Console.WriteLine("w"); });

            return commands;

        }
    }
}