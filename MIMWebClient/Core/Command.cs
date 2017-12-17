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
        public static Dictionary<string, Action> Commands(string commandOptions, string commandKey, PlayerSetup.Player playerData, Room.Room room)
        {
            var context = HubContext.Instance;

            var commandList = new Dictionary<String, Action>
            {
                {"north", () => Movement.Move(playerData, room, "North")},
                {"south", () => Movement.Move(playerData, room, "South")},
                {"east", () => Movement.Move(playerData, room, "East")},
                {"west", () => Movement.Move(playerData, room, "West")},
                {"down", () => Movement.Move(playerData, room, "Down")},
                {"up", () => Movement.Move(playerData, room, "Up")},
                {"look", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look")},
                {"look at", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look")},
                {"l at", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look")},
                {"l in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in")},
                {"look in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in")},
                {"search", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look")},
                {"search in", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "look in")},
                {"examine", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "examine")},
                {"touch", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "touch")},
                {"smell", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "smell")},
                {"taste", () => LoadRoom.ReturnRoom(playerData, room, commandOptions, "taste")},
                {"score", () => Score.ReturnScore(playerData)},
                {"inventory", () => Inventory.ReturnInventory(playerData.Inventory, playerData)},
                {"eq", () => Equipment.ShowEquipment(playerData)},
                {"equip", () => Equipment.WearItem(playerData, commandOptions)},
                {"equipment", () => Equipment.ShowEquipment(playerData)},
                {"garb", () => Equipment.ShowEquipment(playerData)},
                {"loot", () => ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item")},
                {"pick up", () => ManipulateObject.GetItem(room, playerData, commandOptions, commandKey, "item")},
                {"plunder", () => ManipulateObject.GetItem(room, playerData, "all " + commandOptions, commandKey, "item")},
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
                {"yes", () => Communicate.Say("Yes", playerData, room)},
                {"no", () => Communicate.Say("No", playerData, room)},
                {"say", () => Communicate.Say(commandOptions, playerData, room)},
                {"sayto", () => Communicate.SayTo(commandOptions, room, playerData)},
                {">", () => Communicate.SayTo(commandOptions, room, playerData)},
                {"yell", () => Communicate.Yell(commandOptions, room, playerData)},
                {"talkto", () => Talk.TalkTo(commandOptions, room, playerData)},
                {"emote", () => Emote.EmoteActionToRoom(commandOptions, playerData)},
                {"use", () => Equipment.WearItem(playerData, commandOptions)},
                {"wear", () => Equipment.WearItem(playerData, commandOptions)},
                {"remove", () => Equipment.RemoveItem(playerData, commandOptions)},
                {"doff", () => Equipment.RemoveItem(playerData, commandOptions)},
                {"wield", () => Equipment.WearItem(playerData, commandOptions, true)},
                {"unwield", () => Equipment.RemoveItem(playerData, commandOptions, false, true)},
                {"hit", () => Fight2.PerpareToFight(playerData, room, commandOptions)},
                {"kill", () => Fight2.PerpareToFight(playerData, room, commandOptions)},
                {"attack", () => Fight2.PerpareToFight(playerData, room, commandOptions)},
                {"flee", () => Flee.fleeCombat(playerData, room)},
                {"sacrifice", () => Harvest.Body(playerData, room, commandOptions)},
                {"harvest", () => Harvest.Body(playerData, room, commandOptions)},
                {"peak", () => Peak.DoPeak(context, playerData, room, commandOptions)},
                {"steal", () => Steal.DoSteal(context, playerData, room, commandOptions)},

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

                //skills angle, line, trawl, lure, bob
                {"forage", () =>
                {
                    var foraging = new Foraging();

                    foraging.StartForaging(playerData, room);
                }},

                {"fish", () =>
                {
                    var fishing = new Fishing();

                    fishing.StartFishing(playerData, room);
                }},
                {"angle", () =>
                {
                    var fishing = new Fishing();

                    fishing.StartFishing(playerData, room);
                }},
                {"line", () =>
                {
                    var fishing = new Fishing();

                    fishing.StartFishing(playerData, room);
                }},
                {"trawl", () =>
                {
                    var fishing = new Fishing();

                    fishing.StartFishing(playerData, room);
                }},
                {"lure", () =>
                {
                    var fishing = new Fishing();

                    fishing.StartFishing(playerData, room);
                }},
                {"reel", () =>Fishing.GetFish(playerData, room)},
                {"dirt kick", () =>
                {
                    var dirtKicking = new DirtKick();

                    dirtKicking.StartDirtKick(context, playerData, room, commandOptions);
                }},
                {"bash", () =>
                {
                    var bash = new Bash();

                    bash.StartBash(context, playerData, room, commandOptions);
                }},
                 {"Shield bash", () =>
                {
                    var sbash = new ShieldBash();

                    sbash.StartBash(context, playerData, room, commandOptions);
                }},
                {"punch", () => Punch.StartPunch(playerData, room)},
                {"kick", () =>
                {
                    var kick = new Kick();
                    kick.StartKick(context, playerData, room, commandOptions);
                }},
                {"spin kick", () =>
                {
                    var skick = new SpinKick();
                    skick.StartKick(context, playerData, room, commandOptions);
                }},
                {"rescue", () =>
                {
                    var rescue = new Rescue();
                    rescue.StartRescue(context, playerData, room, commandOptions);
                }},
                 {"lunge", () =>
                {
                    var lunge = new Lunge();
                    lunge.StartLunge(context, playerData, room, commandOptions);
                }},
                {"disarm", () =>
                {
                    var disarm = new Disarm();
                    disarm.StartDisarm(context, playerData, room);
                }},
                {"ride", () =>  Mount.StartMount(playerData, room, commandOptions)},
                {"mount", () => Mount.StartMount(playerData, room, commandOptions)},
                {"dismount", () => Mount.Dismount(playerData, room, commandOptions)},
                { "trip", () => {new Trip().StartTrip(context, playerData, room, commandOptions); }},
                {"sneak", () => Sneak.DoSneak(context, playerData)},
                {"hide", () => Hide.DoHide(context, playerData)},
                {"lore", () => Lore.DoLore(context, playerData, commandOptions)},

                //
                {"unlock", () => ManipulateObject.UnlockItem(room, playerData, commandOptions, commandKey)},
                {"lock", () => ManipulateObject.LockItem(room, playerData, commandOptions, commandKey)},
                {"open", () => ManipulateObject.Open(room, playerData, commandOptions, commandKey)},
                {"close", () => ManipulateObject.Close(room, playerData, commandOptions, commandKey)},
                {"drink", () => ManipulateObject.Drink(room, playerData, commandOptions, commandKey)},
                {"help", () => Help.ShowHelp(commandOptions, playerData)},
                {"commands", () => Help.ShowHelp(commandOptions, playerData)},
                {"/help", () => Help.ShowHelp(commandOptions, playerData)},
                {"?", () => Help.ShowHelp(commandOptions, playerData)},
                {"time", Update.Time.ShowTime},
                {"clock", Update.Time.ShowTime},
                {"skills", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions)},
                {"spells", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions)},
                {"skills all", () => ShowSkills.ShowPlayerSkills(playerData, commandOptions)},
                {"practice", () => Trainer.Practice(playerData, room, commandOptions)},
                {"list", () => Shop.listItems(playerData, room)},
                {"buy", () => Shop.buyItems(playerData, room, commandOptions)},
                {"sell", () => Shop.sellItems(playerData, room, commandOptions)},
                {"quest log", () => Quest.QuestLog(playerData)},
                {"qlog", () => Quest.QuestLog(playerData)},
                {"wake", () => Status.WakePlayer(context, playerData, room)},
                {"sleep", () => Status.SleepPlayer(context, playerData, room)},
                {"rest", () => Status.RestPlayer(context, playerData, room)},
                {"sit", () => Status.RestPlayer(context, playerData, room)},
                {"stand", () => Status.StandPlayer(context, playerData, room)},
                {"greet", () => Greet.GreetMob(playerData, room, commandOptions)},
                {"who", () => Who.Connected(playerData)},
                {"affects", () => Effect.Show(playerData)},
                {"follow", () => Follow.FollowThing(playerData, room, commandOptions) },
                {"nofollow", () => Follow.FollowThing(playerData, room, "noFollow") },
                {"quit", () => HubContext.Instance.Quit(playerData.HubGuid, room)},
                {"craft", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Craft)},
                {"chop", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Chop)},
                {"cook", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Cook)},
                {"brew", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Brew)},
                {"forge", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Forge)},
                {"carve", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Carve)},
                {"knit", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Knitting)},
                {"make", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Craft)},
                {"build", () => Craft.CraftItem(playerData, room, commandOptions, CraftType.Craft)},
                {"show crafts", () => Craft.CraftList(playerData)},
                {"craftlist", () => Craft.CraftList(playerData)},
                {"set up camp", () => Camp.SetUpCamp(playerData, room)},
                {"repair", () => Events.Repair.RepairItem(playerData, room, commandOptions)}, 

                //admin
                {"/debug", () => PlayerSetup.Player.DebugPlayer(playerData) },
                {"/setGold", () => PlayerSetup.Player.SetGold(playerData, commandOptions) },
                {"/setAc", () => PlayerSetup.Player.SetAC(playerData, commandOptions) },
                {"/godmode", () => SigmaMap.DrawMap(playerData.HubGuid) },
                {"/map", () => SigmaMap.DrawMap(playerData.HubGuid) }
            };


            return commandList;
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
                        enteredCommand.Substring(enteredCommand.IndexOf(commands[0], StringComparison.Ordinal)).Trim();

                }
                else
                {

                    commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(' ', 1)).Trim();

                }

            }

            //TODO: do this only once
            var command = Command.Commands(commandOptions, commandKey, playerData, room);

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
                    ErrorMessage = commandKey + " " + commandOptions,
                    MethodName = "Wrong command"
                };

                Save.LogError(log);

                HubContext.Instance.SendToClient("Sorry you can't do that.", playerData.HubGuid);
            }

            playerData.LastCommandTime = DateTime.Now;

            Score.UpdateUiPrompt(playerData);

            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine(stopwatch.ElapsedMilliseconds);
        }

    }
}