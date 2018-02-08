using System;
using System.Collections.Generic;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using MIMWebClient.Core;
using MIMWebClient.Core.AI;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World;

[assembly: OwinStartup("MIMWebClientConfig", typeof(MIMWebClient.Startup))]

namespace MIMWebClient
{


    public class Startup
    {
        private static List<Room> _mappedRooms;
        private static List<Room> _listOfRooms;
        private static Dictionary<string, string> _commands;

        private static Dictionary<string, string> CommandKeys()
        {
            return new Dictionary<string, string>
        {
            {"north","north"},
            {"south", "south"},
            {"east", "east"},
            {"west", "west"},
            {"down", "down"},
            {"up", "up"},
            {"look", "look"},
            {"look at", "look at"},
            {"l at", "l at"},
            {"l in", "l in"},
            {"look in", "look in"},
            {"search", "search"},
            {"search in", "search in"},
            {"examine", "examine"},
            {"touch", "touch"},
            {"smell", "smell"},
            {"taste", "taste"},
            {"score", "score"},
            {"inventory", "inventory"},
            {"eq", "eq"},
            {"equip", "equip"},
            {"equipment", "equipment"},
            {"garb", "garb"},
            {"loot", "loot"},
            {"plunder","plunder"},
            {"get", "get"},
            {"take", "take"},
            {"drop","drop"},
            {"give", "give"},
            {"put", "put"},
            {"save","save"},
            {"'", "'"},
            {"newbie","newbie"},
            {"gossip", "gossip"},
            {"ooc", "ooc"},
            {"yes", "yes"},
            {"no", "no"},
            {"say", "say"},
            {"sayto", "sayto"},
            {">", ">"},
            {"yell", "yell"},
            {"talkto", "talkto"},
            {"emote", "emote"},
            {"use", "use"},
            {"wear", "wear"},
            {"remove", "remove"},
            {"doff","doff"},
            {"wield", "wield"},
            {"unwield", "unwield"},
            {"hit", "hit"},
            {"kill", "kill"},
            {"attack", "attack"},
            {"flee", "flee"},
            {"sacrifice","sacrifice"},
            {"harvest", "harvest"},
            {"peak", "peak"},
            {"steal", "steal"},
            {"pick", "pick"},

            //spells
            {"c magic missile", "c magic missile"},
            {"cast magic missile", "cast magic missile"},
            { "c armour",  "c armour"},
            {"cast armour", "cast armour"},
            { "c armor",  "c armor"},
            {"cast armor", "cast armor"},
            { "c continual light",  "c continual light"},
            {"cast continual light", "cast continual light"},
            { "c invis",  "c invis"},
            {"cast invis", "cast invis"},
            { "c weaken", "c weaken"},
            {"cast weaken", "cast weaken"},
            { "c chill touch",  "c chill touch"},
            {"cast chill touch", "cast chill touch"},
            { "c fly", "c fly"},
            {"cast fly", "cast fly"},
            { "c refresh", "c refresh"},
            {"cast refresh", "cast refresh"},
            { "c faerie fire", "c faerie fire"},
            {"cast faerie fire", "cast faerie fire"},
            { "c teleport", "c teleport"},
            {"cast teleport", "cast teleport"},
            { "c blindness", "c blindness"},
            {"cast blindess", "cast blindess"},
            { "c haste", "c haste"},
            {"cast haste", "cast haste"},
            { "c create spring", "c create spring"},
            {"cast create spring", "cast create spring"},
            { "c shocking grasp", "c shocking grasp"},
            {"cast shocking grasp", "cast shocking grasp"},
            { "c cause light", "c cause light"},
            {"cast cause light", "cast cause light"},
            { "c cure light", "c cure light"},
            {"cast cure light", "cast cure light"},
            { "c detect invis", "c detect invis"},
            {"cast detect invis", "cast detect invis"},

            //skills angle, line, trawl, lure, bob
            {"forage", "forage"},
            {"fish", "fish"},
            {"angle", "angle"},
            {"line", "line"},
            {"trawl", "trawl"},
            {"lure", "lure"},
            {"reel", "reel"},
            {"dirt kick", "dirt kick"},
            {"bash", "bash"},
            {"Shield bash", "Shield bash"},
            {"punch", "punch"},
            {"kick", "kick"},
            {"spin kick", "spin kick"},
            {"rescue", "rescue"},
            {"lunge", "lunge"},
            {"disarm", "disarm"},
            {"backstab","backstab"},
            {"feint", "feint"},
            {"ride", "ride"},
            {"mount", "mount"},
            {"dismount", "dismount"},
            {"trip", "trip"},
            {"sneak", "sneak"},
            {"hide", "hide"},
            {"lore", "lore"},

            // Other
            {"unlock", "unlock"},
            {"lock", "lock"},
            {"open", "open"},
            {"close", "close"},
            {"drink", "drink"},
            {"help", "help"},
            {"commands", "commands"},
            {"/help", "/help"},
            {"?", "?"},
            {"time","time"},
            {"clock", "clock"},
            {"skills", "skills"},
            {"spells", "spells"},
            {"skills all", "skills all"},
            {"practice", "practice"},
            {"list", "list"},
            {"buy", "buy"},
            {"sell", "sell"},
            {"quest log", "quest log"},
            {"qlog", "qlog"},
            {"wake", "wake"},
            {"sleep", "sleep"},
            {"rest", "rest"},
            {"sit", "sit"},
            {"stand","stand"},
            {"greet", "greet"},
            {"who", "who"},
            {"affects", "affects"},
            {"follow", "follow"},
            {"nofollow", "nofollow"},
            {"quit", "quit"},
            {"craft","craft"},
            {"chop", "chop"},
            {"cook", "cook"},
            {"brew", "brew"},
            {"forge", "forge"},
            {"carve", "carve"},
            {"knit", "knit"},
            {"make", "make"},
            {"build", "build"},
            {"show crafts", "show crafts"},
            {"craftlist", "craftlist"},
            {"set up camp", "set up camp"},
            {"repair", "repair"}, 

            //admin
            {"/debug", "/debug" },
            {"/setGold", "/setGold" },
            {"/setAc", "/setAc" },
            {"/godmode","/godmode" },
            {"/map", "/map"}
        };

        }


        public static List<Room> ReturnRooms
        {
            get
            {
                if (_listOfRooms == null)
                {
                    _listOfRooms = Areas.ListOfRooms();
                }

                return _listOfRooms;
            }
        }

        public static List<Room> SetMappedRooms
        {
            get
            {
                if (_mappedRooms == null)
                {
                    var roomSetUp = new BreadthFirstSearch();
                    _mappedRooms = roomSetUp.AssignCoords("Anker", "Anker");

                }

                return _mappedRooms;
            }
        }

        public static Dictionary<string, string> CommandKey
        {
            get
            {
                if (_commands == null)
                {
                    _commands = Startup.CommandKeys();
                }

                return _commands;
            }
        }


        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}