using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Mob.Events;

namespace MIMWebClient.Core
{
    using System.Collections.Concurrent;

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.Room;

    using Newtonsoft.Json.Linq;
    using System.Threading;

    using MIMWebClient.Core.Player.Skills;

    public class Command
    {

        public static List<string> _Buffer = new List<string>();
 


        //public static Dictionary<string, Action> commandList { get; set; }
        public static Dictionary<string, Action> Commands(string commandOptions,string commandKey,PlayerSetup.Player playerData,Room.Room room)
        {

            var commandList = new Dictionary<String, Action>(); 
            commandList.Add("north", () => Movement.Move(playerData, room, "North"));
            commandList.Add("south", () => Movement.Move(playerData, room, "South"));
            commandList.Add("east", () => Movement.Move(playerData, room, "East"));
            commandList.Add("west", () => Movement.Move(playerData, room, "West"));
            commandList.Add("down", () => Movement.Move(playerData, room, "Down"));
            commandList.Add("up", () => Movement.Move(playerData, room, "Up"));
            //commandList.Add("look at", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look"));
            commandList.Add("look", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look"));
            commandList.Add("l in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in"));
            commandList.Add("look in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in"));
            commandList.Add("examine", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "examine"));
            commandList.Add("touch", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "touch"));
            commandList.Add("smell", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "smell"));
            commandList.Add("taste", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "taste"));
            commandList.Add("score", () => Score.ReturnScore(playerData));
            commandList.Add("inventory", () => Inventory.ReturnInventory(playerData.Inventory, playerData));
            commandList.Add("equipment", () => Equipment.ShowEquipment(playerData));
            commandList.Add("garb", () => Equipment.ShowEquipment(playerData));
            commandList.Add("get", () => ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item"));
            commandList.Add("take", () => ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item"));
            commandList.Add("drop", () => ManipulateObject.DropItem(room, playerData, commandOptions, commandKey));
            commandList.Add("put", () => ManipulateObject.DropItem(room, playerData, commandOptions, commandKey));
            commandList.Add("save", () =>  Save.UpdatePlayer(playerData));
            commandList.Add("'", () => Communicate.Say(commandOptions, playerData, room));
            commandList.Add("newbie", () => Communicate.NewbieChannel(commandOptions, playerData));
            commandList.Add("gossip", () => Communicate.GossipChannel(commandOptions, playerData));
            commandList.Add("ooc", () => Communicate.OocChannel(commandOptions, playerData));
            commandList.Add("say", ()=> Communicate.Say(commandOptions, playerData, room));
            commandList.Add("sayto", () => Communicate.SayTo(commandOptions, room, playerData));
            commandList.Add(">", () => Communicate.SayTo(commandOptions, room, playerData));
            commandList.Add("emote", () => Emote.EmoteActionToRoom(commandOptions, playerData));
            commandList.Add("quit", () => HubContext.Quit(playerData.HubGuid, room));
            commandList.Add("wear", () => Equipment.WearItem(playerData, commandOptions));
            commandList.Add("remove", () => Equipment.RemoveItem(playerData, commandOptions));
            commandList.Add("wield", () => Equipment.WearItem(playerData, commandOptions, true));
            commandList.Add("unwield", () => Equipment.RemoveItem(playerData, commandOptions, false, true));
            commandList.Add("kill",  () =>  Fight2.PerpareToFight(playerData, room, commandOptions));
            commandList.Add("flee", () => Flee.fleeCombat(playerData, room));
            commandList.Add("unlock", () => ManipulateObject.UnlockItem(room, playerData, commandOptions, commandKey));
            commandList.Add("lock", () => ManipulateObject.LockItem(room, playerData, commandOptions, commandKey));
            commandList.Add("open", () => ManipulateObject.Open(room, playerData, commandOptions, commandKey));
            commandList.Add("close", () => ManipulateObject.Close(room, playerData, commandOptions, commandKey));
            commandList.Add("punch", () =>  Punch.StartPunch(playerData, room));
            commandList.Add("help", () => Help.ShowHelp(commandOptions, playerData));
            commandList.Add("time", Update.Time.ShowTime);
            commandList.Add("clock", Update.Time.ShowTime);
            commandList.Add("skills", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions));
            commandList.Add("skills all", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions));
            commandList.Add("practice", () =>  Trainer.Practice(playerData, room, commandOptions));
            commandList.Add("map", () => Map.GenerateGrid(playerData));

            return commandList;
        }

 

        public static  void ParseCommand(string input, PlayerSetup.Player playerData, Room.Room room = null)
        {

            //testing
            string enteredCommand = input;
            string[] commands = enteredCommand.Split(' ');
            string commandKey = commands[0];

          
            string commandOptions = string.Empty;
            // testing

           
 
            if (commands.Length >= 2)
            {
           
                if ((commands[1].Equals("in", StringComparison.InvariantCultureIgnoreCase) || commands[1].Equals("at", StringComparison.InvariantCultureIgnoreCase)))
                {
                    commandKey = commands[0] + " " + commands[1];
                    commandOptions =  enteredCommand.Substring(enteredCommand.IndexOf(commands[2], StringComparison.Ordinal)).Trim();  
                }
                else
                {
                    commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(' ', 1)).Trim();
                }
               
            }
 
             //TODO: do this only once
            var command =  Command.Commands(commandOptions, commandKey, playerData, room);
 
            var fire = command.FirstOrDefault(x => x.Key.StartsWith(commandKey, StringComparison.InvariantCultureIgnoreCase));
 
            if (fire.Value != null)
            {
                 fire.Value();

            

            }
            else
            {

                HubContext.SendToClient("Sorry you can't do that.", playerData.HubGuid);
            }

            //  Prompt.ShowPrompt(playerData);
            Score.UpdateUiPrompt(playerData);
           
        }

        //public static void CommandBuffer(string input, PlayerSetup.Player playerData, Room.Room room = null)
        //{
        //    _Buffer.Add(input);

        //    ProcessBuffer(playerData, room);


        //}

        //public static void ProcessBuffer(PlayerSetup.Player playerData, Room.Room room = null)
        //{
        //   var playerInput = _Buffer.FirstOrDefault();

        //    ParseCommand(playerInput, playerData, room);

        //}
    }
}