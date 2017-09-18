using System;
using System.Collections.Generic;
using System.Linq;
using MIMWebClient.Core.Mob.Events;

namespace MIMWebClient.Core
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.Room;
    using MIMWebClient.Core.Player.Skills;

    public class Command
    {

        /// <summary>
        /// Command list for the game
        /// </summary>
        /// <param name="commandOptions">Everything after the 1st occurance of a space</param>
        /// <param name="commandKey">The string before the 1st occurance of a space</param>
        /// <param name="playerData">Player Data</param>
        /// <param name="room">Current room</param>
        /// <returns>Returns Dictionary of commands</returns>
        public static Dictionary<string, Action> Commands(string commandOptions,string commandKey,PlayerSetup.Player playerData,Room.Room room)
        {

            var commandList = new Dictionary<String, Action>
            {
                {"north", () => Movement.Move(playerData, room, "North")},
                {"south", () => Movement.Move(playerData, room, "South")},
                {"east", () => Movement.Move(playerData, room, "East")},
                {"west", () => Movement.Move(playerData, room, "West")},
                {"down", () => Movement.Move(playerData, room, "Down")},
                {"up", () => Movement.Move(playerData, room, "Up")},               
                {"look", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look")},
                {"l in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in")},
                {"look in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in")},
                {"examine", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "examine")},
                {"touch", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "touch")},
                {"smell", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "smell")},
                {"taste", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "taste")},
                {"score", () => Score.ReturnScore(playerData)},
                {"inventory", () => Inventory.ReturnInventory(playerData.Inventory, playerData)},
                {"equipment", () => Equipment.ShowEquipment(playerData)},
                {"garb", () => Equipment.ShowEquipment(playerData)},
                {"get", () => ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item")},
                {"take", () => ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item")},
                {"drop", () => ManipulateObject.DropItem(room, playerData, commandOptions, commandKey)},
                {"give", () => ManipulateObject.GiveItem(room, playerData, commandOptions, commandKey, "killable")},
                {"put", () => ManipulateObject.DropItem(room, playerData, commandOptions, commandKey)},
                {"save", () => Save.SavePlayer(playerData)},
                {"'", () => Communicate.Say(commandOptions, playerData, room)},
                {"newbie", () => Communicate.NewbieChannel(commandOptions, playerData)},
                {"gossip", () => Communicate.GossipChannel(commandOptions, playerData)},
                {"ooc", () => Communicate.OocChannel(commandOptions, playerData)},
                {"say", () => Communicate.Say(commandOptions, playerData, room)},
                {"sayto", () => Communicate.SayTo(commandOptions, room, playerData)},
                {">", () => Communicate.SayTo(commandOptions, room, playerData)},
                {"talkto", () => Talk.TalkTo(commandOptions, room, playerData)},
                {"emote", () => Emote.EmoteActionToRoom(commandOptions, playerData)},
                {"wear", () => Equipment.WearItem(playerData, commandOptions)},
                {"remove", () => Equipment.RemoveItem(playerData, commandOptions)},
                {"doff", () => Equipment.RemoveItem(playerData, commandOptions)},
                {"wield", () => Equipment.WearItem(playerData, commandOptions, true)},
                {"unwield", () => Equipment.RemoveItem(playerData, commandOptions, false, true)},
                {"kill", () => Fight2.PerpareToFight(playerData, room, commandOptions)},
                {"flee", () => Flee.fleeCombat(playerData, room)},

                //spells
                {"c magic missile", () =>  MagicMissile.StartMagicMissile(playerData, room, commandOptions)},
                {"cast magic missile", () => MagicMissile.StartMagicMissile(playerData, room, commandOptions)},

                { "c armour", () => Armour.StartArmour(playerData, room, commandOptions)},
                {"cast armour", () => Armour.StartArmour(playerData, room, commandOptions)},

                { "c armor", () => Armour.StartArmour(playerData, room, commandOptions)},
                {"cast armor", () => Armour.StartArmour(playerData, room, commandOptions)},

                { "c continual light", () => ContinualLight.StarContinualLight(playerData, room, commandOptions)},
                {"cast continual light", () => ContinualLight.StarContinualLight(playerData, room, commandOptions)},

                { "c invis", () => Invis.StartInvis(playerData, room, commandOptions)},
                {"cast invis", () => Invis.StartInvis(playerData, room, commandOptions)},

                { "c weaken", () => Weaken.StartWeaken(playerData, room, commandOptions)},
                {"cast weaken", () => Weaken.StartWeaken(playerData, room, commandOptions)},

                { "c chill touch", () => ChillTouch.StartChillTouch(playerData, room, commandOptions)},
                {"cast chill touch", () => ChillTouch.StartChillTouch(playerData, room, commandOptions)},

                { "c fly", () => Fly.StartFly(playerData, room, commandOptions)},
                {"cast fly", () => Fly.StartFly(playerData, room, commandOptions)},

                { "c refresh", () => Refresh.StartRefresh(playerData, room, commandOptions)},
                {"cast refresh", () => Refresh.StartRefresh(playerData, room, commandOptions)},

                { "c faerie fire", () => FaerieFire.StartFaerieFire(playerData, room, commandOptions)},
                {"cast faerie fire", () => FaerieFire.StartFaerieFire(playerData, room, commandOptions)},

                { "c teleport", () => Teleport.StartTeleport(playerData, room)},
                {"cast teleport", () => Teleport.StartTeleport(playerData, room)},

                { "c blindness", () => Blindness.StartBlind(playerData, room, commandOptions)},
                {"cast blindess", () => Blindness.StartBlind(playerData, room, commandOptions)},

                { "c haste", () => Haste.StartHaste(playerData, room, commandOptions)},
                {"cast haste", () => Haste.StartHaste(playerData, room, commandOptions)},

                 { "c create spring", () => CreateSpring.StartCreateSpring(playerData, room)},
                {"cast create spring", () => CreateSpring.StartCreateSpring(playerData, room)},

                { "c shocking grasp", () =>
                {
                    var shockingGRasp = new ShockingGrasp();

                    shockingGRasp.StartShockingGrasp(playerData, room, commandOptions);
                }},
                {"cast shocking grasp", () => 
                {
                        var shockingGRasp = new ShockingGrasp();

                        shockingGRasp.StartShockingGrasp(playerData, room, commandOptions);
                    }},

                //skills
                {"punch", () => Punch.StartPunch(playerData, room)},
                {"kick", () => Kick.StartKick(playerData, room)},

                //
                {"unlock", () => ManipulateObject.UnlockItem(room, playerData, commandOptions, commandKey)},
                {"lock", () => ManipulateObject.LockItem(room, playerData, commandOptions, commandKey)},
                {"open", () => ManipulateObject.Open(room, playerData, commandOptions, commandKey)},
                {"close", () => ManipulateObject.Close(room, playerData, commandOptions, commandKey)},
                {"drink", () => ManipulateObject.Drink(room, playerData, commandOptions, commandKey)},
                {"help", () => Help.ShowHelp(commandOptions, playerData)},
                {"time", Update.Time.ShowTime},
                {"clock", Update.Time.ShowTime},
                {"skills", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions)},
                {"skills all", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions)},
                {"practice", () => Trainer.Practice(playerData, room, commandOptions)},
                {"list", () => Shop.listItems(playerData, room)},
                {"buy", () => Shop.buyItems(playerData, room, commandOptions)},
                {"quest log", () => Quest.QuestLog(playerData)},
                {"qlog", () => Quest.QuestLog(playerData)},
                {"wake", () => Status.WakePlayer(playerData, room)},
                {"sleep", () => Status.SleepPlayer(playerData, room)},
                {"greet", () => Greet.GreetMob(playerData, room, commandOptions)},
                {"who", () => Who.Connected(playerData)},
                {"affects", () => Affect.Show(playerData)},
                {"follow", () => Follow.FollowThing(playerData, room, commandOptions) },
                 {"nofollow", () => Follow.FollowThing(playerData, room, "noFollow") },
                  {"quit", () => HubContext.Quit(playerData.HubGuid, room)},
                //admin
                {"/debug", () => PlayerSetup.Player.DebugPlayer(playerData) }
            };
 

            return commandList;
        }

 
        /// <summary>
        /// Handles matching and calling the commands
        /// </summary>
        /// <param name="input">What the player typed in</param>
        /// <param name="playerData">Player Data</param>
        /// <param name="room">Current Room</param>
        public static  void ParseCommand(string input, PlayerSetup.Player playerData, Room.Room room = null)
        {

            if (string.IsNullOrEmpty(input.Trim()))
            {
                HubContext.SendToClient("You need to enter a command, type help if you need it.", playerData.HubGuid);
                return;
            }

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
                else if (commandKey.Equals("c", StringComparison.InvariantCultureIgnoreCase) || commandKey.Equals("cast", StringComparison.InvariantCultureIgnoreCase) && commands.Length > 1)
                {
                    commandKey = commands[0] + " " + commands[1];

                    commandOptions =
                        enteredCommand.Substring(enteredCommand.IndexOf(commands[0], StringComparison.Ordinal)).Trim();

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
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = commandOptions + " " + commandKey,
                    MethodName = "Wrong command"
                };

                Save.LogError(log);

                HubContext.SendToClient("Sorry you can't do that.", playerData.HubGuid);
            }

            playerData.LastCommandTime = DateTime.UtcNow;

            Score.UpdateUiPrompt(playerData);
           
        }

    }
}