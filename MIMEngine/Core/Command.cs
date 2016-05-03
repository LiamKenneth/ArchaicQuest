using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core
{
    using MIMEngine.Core.Events;

    public class Command
    {

        public static Dictionary<string, Action> Commands(string commandOptions, PlayerSetup.PlayerSetup playerData)
        {
            var commandList = new Dictionary<String, Action>(); 
            commandList.Add("score", () => Score.ReturnScore(playerData));
//            commandList.Add("hello", sayHello);
//            commandList.Add("Yo", () => sayHello(commandOptions));
//            commandList.Add("North", () => Move("North"));
//            commandList.Add("East", () => Move("East"));
//            commandList.Add("West", () => Move("West")) 
            return commandList;
        }

 

        public static void ParseCommand(string input, PlayerSetup.PlayerSetup playerData)
        {

            //testing
            string enteredCommand = input;
            string[] commands = enteredCommand.Split(' ');
            string commandKey = commands[0];
            string commandOptions = string.Empty;
            // testing
 
            if (commands.Length >= 2)
            {
                commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(' ', 1));
            }
 
 
            var command = Commands(commandOptions, playerData);
 
            var fire = command.FirstOrDefault(x => x.Key.StartsWith(commandKey));

            fire.Value();
        }
    }
}