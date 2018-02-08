using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static void Commands(string commandOptions, string commandKey, PlayerSetup.Player playerData, Room.Room room)
        {
            var context = HubContext.Instance;

            switch (commandKey)
            {
                case "north":
                    Movement.Move(playerData, room, "North");
                    break;
                case "east":
                    Movement.Move(playerData, room, "East");
                    break;
                case "south":
                    Movement.Move(playerData, room, "South");
                    break;
                case "west":
                    Movement.Move(playerData, room, "West");
                    break;
                case "up":
                    Movement.Move(playerData, room, "Up");
                    break;
                case "down":
                    Movement.Move(playerData, room, "Down");
                    break;
                case "look":
                case "look at":
                case "l at":
                case "search":
                    LoadRoom.ReturnRoom(playerData, room, commandOptions, "look");
                    break;
                case "l in":
                case "search in":
                    LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in");
                    break;
                case "examine":
                    LoadRoom.ReturnRoom(playerData, room, commandOptions, "examine");
                    break;
                case "touch":
                    LoadRoom.ReturnRoom(playerData, room, commandOptions, "touch");
                    break;
                case "smell":
                    LoadRoom.ReturnRoom(playerData, room, commandOptions, "smell");
                    break;
                case "taste":
                    LoadRoom.ReturnRoom(playerData, room, commandOptions, "taste");
                    break;
                case "score":
                    Score.ReturnScore(playerData);
                    break;
                case "inventory":
                    Inventory.ReturnInventory(playerData.Inventory, playerData);
                    break;
                case "eq":
                case "equip":
                case "equipment":
                case "garb":
                    Equipment.ShowEquipment(playerData);
                    break;
                case "loot":
                case "get":
                case "take":
                    ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item");
                    break;
                case "plunder":
                    ManipulateObject.GetItem(room, playerData, "all " + commandOptions, commandKey, "item");
                    break;
                case "drop":
                case "put":
                    ManipulateObject.DropItem(room, playerData, commandOptions, commandKey);
                    break;
                case "give":
                    ManipulateObject.GiveItem(room, playerData, commandOptions, commandKey, "killable");
                    break;
                case "save":
                    Save.SavePlayer(playerData);
                    break;
                case "say":
                case "'":
                    Communicate.Say(commandOptions, playerData, room);
                    break;
                case "sayto":
                case ">":
                    Communicate.SayTo(commandOptions, room, playerData);
                    break;
                case "newbie":
                    Communicate.NewbieChannel(commandOptions, playerData);
                    break;
                case "gossip":
                    Communicate.GossipChannel(commandOptions, playerData);
                    break;
                case "ooc":
                    Communicate.OocChannel(commandOptions, playerData);
                    break;
                case "yes":
                    Communicate.Say("Yes", playerData, room);
                    break;
                case "no":
                    Communicate.Say("No", playerData, room);
                    break;
                case "yell":
                    Communicate.Yell(commandOptions, room, playerData);
                    break;
                case "talkto":
                    Talk.TalkTo(commandOptions, room, playerData);
                    break;
                case "emote":
                    Emote.EmoteActionToRoom(commandOptions, playerData);
                    break;
                case "use":
                case "wear":
                case "wield":
                    Equipment.WearItem(playerData, commandOptions);
                    break;
                case "remove":
                case "doff":
                case "unwield":
                    Equipment.RemoveItem(playerData, commandOptions);
                    break;
                case "hit":
                case "kill":
                case "attack":
                    Fight2.PerpareToFight(playerData, room, commandOptions);
                    break;
                case "flee":
                    Flee.fleeCombat(playerData, room);
                    break;
                case "sacrifice":
                case "harvest":
                    Harvest.Body(playerData, room, commandOptions);
                    break;
                case "peek":
                    Peak.DoPeak(context, playerData, room, commandOptions);
                    break;
                case "steal":
                    Steal.DoSteal(context, playerData, room, commandOptions);
                    break;
                case "pick":
                    LockPick.DoLockPick(context, playerData, room, commandOptions);
                    break;
                case "c magic missile":
                case "cast magic missile":
                    MagicMissile.StartMagicMissile(playerData, room, commandOptions);
                    break;
                case "c armour":
                case "cast armour":
                case "c armor":
                case "cast armor":
                    new Armour().StartArmour(playerData, room, commandOptions);
                    break;
                case "c continual light":
                case "cast continual light":
                    ContinualLight.StarContinualLight(playerData, room, commandOptions);
                    break;
                case "c invis":
                case "cast invis":
                    Invis.StartInvis(playerData, room, commandOptions);
                    break;
                case "c weaken":
                case "cast weaken":
                    Weaken.StartWeaken(playerData, room, commandOptions);
                    break;
                case "c chill touch":
                case "cast chill touch":
                    ChillTouch.StartChillTouch(playerData, room, commandOptions);
                    break;
                case "c fly":
                case "cast fly":
                    Fly.StartFly(playerData, room, commandOptions);
                    break;
                case "c refresh":
                case "cast refresh":
                    Refresh.StartRefresh(playerData, room, commandOptions);
                    break;
                case "c faerie fire":
                case "cast faerie fire":
                    FaerieFire.StartFaerieFire(playerData, room, commandOptions);
                    break;
                case "c teleport":
                case "cast teleport":
                    Teleport.StartTeleport(playerData, room);
                    break;
                case "c blindness":
                case "cast blindness":
                    Blindness.StartBlind(playerData, room, commandOptions);
                    break;
                case "c haste":
                case "cast haste":
                    Haste.StartHaste(playerData, room, commandOptions);
                    break;
                case "c create spring":
                case "cast create spring":
                    CreateSpring.StartCreateSpring(playerData, room);
                    break;
                case "c shocking grasp":
                case "cast shocking grasp":
                    new ShockingGrasp().StartShockingGrasp(playerData, room, commandOptions);
                    break;
                case "c cause light":
                case "cast cause light":
                    new CauseLight().StartCauseLight(context, playerData, room, commandOptions);
                    break;
                case "c cure light":
                case "cast cure light":
                    new CureLight().StartCureLight(context, playerData, room, commandOptions);
                    break;
                case "c detect invis":
                case "cast detect invis":
                    DetectInvis.DoDetectInvis(context, playerData, room);
                    break;
                case "forage":
                    new Foraging().StartForaging(playerData, room);
                    break;
                case "fish":
                case "angle":
                case "line":
                case "trawl":
                case "lure":
                    new Fishing().StartFishing(playerData, room);
                    break;
                case "reel":
                    Fishing.GetFish(playerData, room);
                    break;
                case "dirt kick":
                    new DirtKick().StartDirtKick(context, playerData, room, commandOptions);
                    break;
                case "bash":
                    new Bash().StartBash(context, playerData, room, commandOptions);
                    break;
                case "shield bash":
                    new ShieldBash().StartBash(context, playerData, room, commandOptions);
                    break;
                case "punch":
                    Punch.StartPunch(playerData, room);
                    break;
                case "kick":
                    new Kick().StartKick(context, playerData, room, commandOptions);
                    break;
                case "spin kick":
                    new SpinKick().StartKick(context, playerData, room, commandOptions);
                    break;
                case "rescue":
                    new Rescue().StartRescue(context, playerData, room, commandOptions);
                    break;
                case "lunge":
                    new Lunge().StartLunge(context, playerData, room, commandOptions);
                    break;
                case "disarm":
                    new Disarm().StartDisarm(context, playerData, room);
                    break;
                case "backstab":
                    new Backstab().StartBackstab(context, playerData, room, commandOptions);
                    break;
                case "feint":
                    new Feint().StartFeint(context, playerData, room, commandOptions);
                    break;
                case "mount":
                case "ride":
                    Mount.StartMount(playerData, room, commandOptions);
                    break;
                case "dismount":
                    Mount.Dismount(playerData, room, commandOptions);
                    break;
                case "trip":
                    new Trip().StartTrip(context, playerData, room, commandOptions);
                    break;
                case "sneak":
                    Sneak.DoSneak(context, playerData);
                    break;
                case "hide":
                    Hide.DoHide(context, playerData);
                    break;
                case "lore":
                    Lore.DoLore(context, playerData, commandOptions);
                    break;
                case "unlock":
                    ManipulateObject.UnlockItem(room, playerData, commandOptions, commandKey);
                    break;
                case "lock":
                    ManipulateObject.LockItem(room, playerData, commandOptions, commandKey);
                    break;
                case "close":
                case "shut":
                    ManipulateObject.Close(room, playerData, commandOptions, commandKey);
                    break;
                case "drink":
                    ManipulateObject.Drink(room, playerData, commandOptions, commandKey);
                    break;
                case "help":
                case "/help":
                case "?":
                case "commands":
                    Help.ShowHelp(commandOptions, playerData);
                    break;
                case "time":
                case "clock":
                    Update.Time.ShowTime();
                    break;
                case "skills":
                case "spells":
                case "skills all":
                    ShowSkills.ShowPlayerSkills(playerData, commandOptions);
                    break;
                case "practice":
                    Trainer.Practice(playerData, room, commandOptions);
                    break;
                case "list":
                    Shop.listItems(playerData, room);
                    break;
                case "buy":
                    Shop.buyItems(playerData, room, commandOptions);
                    break;
                case "sell":
                    Shop.sellItems(playerData, room, commandOptions);
                    break;
                case "quest log":
                case "qlog":
                    Quest.QuestLog(playerData);
                    break;
                case "wake":
                    Status.WakePlayer(context, playerData, room);
                    break;
                case "sleep":
                    Status.SleepPlayer(context, playerData, room);
                    break;
                case "rest":
                case "sit":
                    Status.RestPlayer(context, playerData, room);
                    break;
                case "stand":
                    Status.StandPlayer(context, playerData, room);
                    break;
                case "greet":
                    Greet.GreetMob(playerData, room, commandOptions);
                    break;
                case "who":
                    Who.Connected(playerData);
                    break;
                case "affects":
                    Effect.Show(playerData);
                    break;
                case "follow":
                    Follow.FollowThing(playerData, room, commandOptions);
                    break;
                case "nofollow":
                    Follow.FollowThing(playerData, room, "noFollow");
                    break;
                case "quit":
                    HubContext.Instance.Quit(playerData.HubGuid, room);
                    break;
                case "craft":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Craft);
                    break;
                case "chop":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Chop);
                    break;
                case "cook":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Cook);
                    break;
                case "brew":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Brew);
                    break;
                case "forge":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Forge);
                    break;
                case "carve":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Carve);
                    break;
                case "knit":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Knitting);
                    break;
                case "make":
                case "build":
                    Craft.CraftItem(playerData, room, commandOptions, CraftType.Craft);
                    break;
                case "show crafts":
                case "craftlist":
                    Craft.CraftList(playerData);
                    break;
                case "set up camp":
                    Camp.SetUpCamp(playerData, room);
                    break;
                case "repair":
                    Events.Repair.RepairItem(playerData, room, commandOptions);
                    break;
                case "/debug":
                    PlayerSetup.Player.DebugPlayer(playerData);
                    break;
                case "/setGold":
                    PlayerSetup.Player.SetGold(playerData, commandOptions);
                    break;
                case "/setAc":
                    PlayerSetup.Player.SetAC(playerData, commandOptions);
                    break;
                case "/map":
                    SigmaMap.DrawMap(playerData.HubGuid); //not what you think it does
                    break;
                default:
                    HubContext.Instance.SendToClient("Sorry you can't do that. Try help commands or ask on the discord channel.", playerData.HubGuid);
                    var log = new Error.Error
                    {
                        Date = DateTime.Now,
                        ErrorMessage = commandKey + " " + commandOptions,
                        MethodName = "Wrong command"
                    };

                    Save.LogError(log);
                    break;
            }

        }


        /// <summary>
        /// Handles matching and calling the commands
        /// </summary>
        /// <param name="input">What the player typed in</param>
        /// <param name="playerData">Player Data</param>
        /// <param name="room">Current Room</param>
        public static void ParseCommand(string input, PlayerSetup.Player playerData, Room.Room room = null)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            if (string.IsNullOrEmpty(input.Trim()))
            {
                HubContext.Instance.SendToClient("You need to enter a command, type help if you need it.", playerData.HubGuid);
                return;
            }

            if (playerData.Effects?.FirstOrDefault(x => x.Name.Equals("Hidden", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                playerData.Effects.Remove(
                    playerData.Effects?.FirstOrDefault(
                        x => x.Name.Equals("Hidden", StringComparison.CurrentCultureIgnoreCase)));

                Score.UpdateUiAffects(playerData);
            }

            //testing
            string enteredCommand = input;
            string[] commands = enteredCommand.Split(' ');
            string commandKey = commands[0];


            string commandOptions = string.Empty;
            // testing

            if (commands.Length >= 2)
            {

                if ((commands[1].Equals("in", StringComparison.InvariantCultureIgnoreCase) || commands[1].Equals("at", StringComparison.InvariantCultureIgnoreCase) || commands[1].Equals("up", StringComparison.InvariantCultureIgnoreCase)))
                {
                    commandKey = commands[0] + " " + commands[1];
                    commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(commands[2], StringComparison.Ordinal)).Trim();
                }
                else if (commandKey.Equals("c", StringComparison.InvariantCultureIgnoreCase) || commandKey.Equals("cast", StringComparison.InvariantCultureIgnoreCase) && commands.Length > 1)
                {
                    commandKey = commands[0] + " " + commands[1];

                    commandOptions =
                        enteredCommand.Substring(enteredCommand.IndexOf(commands[0], StringComparison.Ordinal))
                            .Trim();

                }
                else
                {

                    commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(' ', 1)).Trim();

                }

            }

            var command = Startup.CommandKey.FirstOrDefault(x => x.Key.StartsWith(commandKey, StringComparison.InvariantCultureIgnoreCase));

            Command.Commands(commandOptions, command.Value, playerData, room);

            playerData.LastCommandTime = DateTime.Now;

            // Score.UpdateUiPrompt(playerData);

            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("Process command " + stopwatch.Elapsed);
        }

    }
}