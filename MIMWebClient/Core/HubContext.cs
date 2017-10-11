using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using MIMWebClient.Core.Loging;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core
{
    using System.Security.Cryptography.X509Certificates;

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player;
    using MIMWebClient.Hubs;
    using Room;

    // This could perhaps be broken down to smaller interfaces
    public interface IHubContext
    {
        void SendToClient(string message, string playerId, string recipientId = null, bool sendToAll = false, bool excludeCaller = false, bool excludeRecipient = false);

        void SendToAllExcept(string message, List<string> excludeThesePlayers, List<PlayerSetup.Player> players);

        void Quit(string playerId, Room.Room room);

        void Quit(string playerId);

        void UpdateStat(string playerGuid, int stat, int maxStat, string statType);

        void AddNewMessageToPage(string playerGuid, string message);

        void AddNewMessageToPage(string message);

        void UpdateUiDescription(string playerGuid, string description);

        void UpdateInventory(string playerGuid, IEnumerable<string> items);

        void BroadcastToRoom(string message, List<PlayerSetup.Player> players, string playerId, bool excludeCaller = false);

        void UpdateExits(string playerGuid, List<string> exits);

        void UpdateScore(PlayerSetup.Player player);

        void UpdateEquipment(string playerGuid, string equipment);

        void UpdateQuestLog(string playerGuid, string log);

        void UpdateEffects(string playerGuid, string message);

        void UpdateUIRoom(string playerGuid, string room);

        void CharacterPasswordError(string hubguid, string message);

        void CharacterNameLoginError(string hubguid, string message);

        void UpdateUiChannels(string playerGuid, string text, string className);

        void UpdateUiMap(string playerGuid, int roomId, string area, string region, int zindex);

        void GetMap(string playerGuid, SigmaMapJSON json);
    }


    public class HubContext: IHubContext
    {
        private Microsoft.AspNet.SignalR.IHubContext _hubContext;

        static HubContext() {}

        private HubContext() {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<MIMHub>();
        }

        private static readonly HubContext instance = new HubContext();

        /// <summary>
        /// gets the SignalR Hub connection
        /// </summary>
        public static HubContext Instance { get { return instance; } }


        /// <summary>
        /// Send a message to connected clients
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="player">The active player</param>
        /// <param name="sendToAll">toggle to send to all or to caller</param>
        /// <param name="excludeCaller">toggle to send to all but exclude caller</param>
        public void SendToClient(string message, string playerId, string recipientId = null, bool sendToAll = false, bool excludeCaller = false, bool excludeRecipient = false)
        {
            if (message != null)
            {

                //Send a message to all users
                if (sendToAll && excludeCaller == false)
                {
                    _hubContext.Clients.All.addNewMessageToPage(message);
                }

                if (playerId != null)
                {
                    //send a message to all but caller
                    // x hits you - Fight message is being sent to all instead of person who's being hit
                    if (excludeCaller && sendToAll == false && excludeRecipient == false)
                    {
                        _hubContext.Clients.AllExcept(playerId).addNewMessageToPage(message);
                    }

                    //send a message to all but recipient
                    if (excludeRecipient && recipientId != null)
                    {
                        _hubContext.Clients.Client(recipientId).addNewMessageToPage(message);
                    }

                    //send only to caller
                    if (sendToAll == false && excludeCaller == false)
                    {
                        _hubContext.Clients.Client(playerId).addNewMessageToPage(message);
                    }


                }
            }
        }

        /// <summary>
        /// Sends a message to all except those specified in the exclude list 
        /// which contains the hubguid of the player
        /// </summary>
        /// <param name="message">message to send to all</param>
        /// <param name="excludeThesePlayers">list of HubGuid's to exclude</param>
        /// <param name="players">players to send message to</param>
        [System.Obsolete("SendToAllExcept is deprecated, please use SendToClient in a forEach instead and exclude the players there.")]
        public void SendToAllExcept(string message, List<string> excludeThesePlayers, List<PlayerSetup.Player> players)
        {
            if (message == null)
            {
                return;
            }

            foreach (var player in players)
            {

                if (player != null && player.HubGuid != excludeThesePlayers?.FirstOrDefault(x => x.Equals(player.HubGuid)))
                {
                    _hubContext.Clients.Client(player.HubGuid).addNewMessageToPage(message);
                }

            }

        }
        [System.Obsolete("broadcastToRoom is deprecated, please use SendToClient in a forEach instead and exclude the player there.")]
        public void BroadcastToRoom(string message, List<PlayerSetup.Player> players, string playerId, bool excludeCaller = false)
        {
            int playerCount = players.Count;

            if (excludeCaller)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    if (playerId != players[i].HubGuid)
                    {
                        _hubContext.Clients.Client(players[i].HubGuid).addNewMessageToPage(message);
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerCount; i++)
                {
                    _hubContext.Clients.Client(players[i].HubGuid).addNewMessageToPage(message);
                }
            }


        }

        public void Quit(string playerId)
        {
            _hubContext.Clients.Client(playerId).quit();
        }

        public void Quit(string playerId, Room.Room room)
        {
            //remove player from room and player cache

            try
            {

                var Player = Cache.getPlayer(playerId);

                if (Player?.Target != null)
                {
                    SendToClient("You can't quit during combat.", playerId);
                    return;
                }     
              
                if (Player != null)
                {

                    using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
                    {
                        var col = db.GetCollection<QuitLocation>("QuitLocation");

                        var quitLoc = new QuitLocation
                        {
                            PlayerName = Player.Name,
                            RoomName = room.title,
                            RoomId = room.areaId
                        };

                        col.Insert(Guid.NewGuid(), quitLoc);
                    }

                    PlayerManager.RemovePlayerFromRoom(room, Player);

                    Save.SavePlayer(Player);

                    PlayerSetup.Player removedChar = null;

                    MIMHub._PlayerCache.TryRemove(Player.HubGuid, out removedChar);

                    SendToClient("Gods take note of your progress", playerId);
                    SendToClient("See you soon!", playerId);
                    BroadcastToRoom(Player.Name + " has left the realm", room.players, playerId, true);

                    try
                    {
                        _hubContext.Clients.Client(playerId).quit();
                    }
                    catch (Exception ex)
                    {
                        var log = new Error.Error
                        {
                            Date = DateTime.Now,
                            ErrorMessage = ex.InnerException.ToString(),
                            MethodName = "Quit"
                        };

                        Save.LogError(log);
                    }

                }

            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "Quit"
                };

                Save.LogError(log);
            }
        }

        public void UpdateStat(string playerGuid, int stat, int maxStat, string statType)
        {
            _hubContext.Clients.Client(playerGuid)
                .updateStat(stat, maxStat, "hp");
        }

        public void AddNewMessageToPage(string message)
        {
            _hubContext.Clients.All.addNewMessageToPage(message);
        }

        public void AddNewMessageToPage(string playerGuid, string message)
        {
            _hubContext.Clients.Client(playerGuid).addNewMessageToPage(message);
        }

        public void UpdateUiDescription(string playerGuid, string description)
        {
            _hubContext.Clients.Client(playerGuid).UpdateUiDescription(description);
        }

        public void UpdateInventory(string playerGuid, IEnumerable<string> items)
        {
            _hubContext.Clients.Client(playerGuid).updateInventory(items);
        }

        public void UpdateExits(string playerGuid, List<string> exits)
        {
            _hubContext.Clients.Client(playerGuid).updateExits(exits);
        }

        public void UpdateScore(PlayerSetup.Player player)
        {
            _hubContext.Clients.Client(player.HubGuid).updateScore(player);
        }

        public void UpdateEquipment(string playerGuid, string equipment)
        {
            _hubContext.Clients.Client(playerGuid).updateEquipment(equipment);
        }

        public void UpdateQuestLog(string playerGuid, string log)
        {
            _hubContext.Clients.Client(playerGuid).updateQuestLog(log);
        }

        public void UpdateEffects(string playerGuid, string message)
        {
            _hubContext.Clients.Client(playerGuid).updateAffects(message);
        }

        public void UpdateUIRoom(string playerGuid, string room)
        {
            _hubContext.Clients.Client(playerGuid).UpdateUiRoom(room);
        }

        public void CharacterPasswordError(string hubguid, string message)
        {
            _hubContext.Clients.Client(hubguid).characterPasswordError(message);
        }

        public void CharacterNameLoginError(string hubguid, string message)
        {
            _hubContext.Clients.Client(hubguid).characterNameLoginError(message);
        }

        public void UpdateUiChannels(string playerGuid, string text, string className)
        {
            _hubContext.Clients.Client(playerGuid).UpdateUiChannels(text, className);
        }

        public void UpdateUiMap(string playerGuid, int roomId, string area, string region, int zindex)
        {
            _hubContext.Clients.Client(playerGuid).UpdateUiMap(roomId, area, region, zindex);
        }

        public void GetMap(string playerGuid, SigmaMapJSON json)
        {
            _hubContext.Clients.Client(playerGuid).getMap(json);
        }
    }
}
