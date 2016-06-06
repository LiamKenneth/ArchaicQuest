using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Collections.Concurrent;

    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class LoadRoom
    {
        public string Region { get; set; }
        public string Area { get; set; }
        public int id { get; set; }


        public Room LoadRoomFile()
        {
            const string ConnectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Room>("Room");

            Room room = collection.Find(x => x.areaId == this.id && x.area == Area && x.region == Region).FirstOrDefault();

            
            if (room != null)
            {
                return room;
            }

            throw new Exception("No Room found in the Database for areaId: " + id);
        }



        public static string DisplayRoom(Room room, string playerName)
        {

            string roomTitle = room.title;
            string roomDescription = room.description;

            var exitList = string.Empty;
            foreach (var exit in room.exits)
            {
                exitList += exit.name + " ";
            }

            var itemList = string.Empty;
            foreach (var item in room.items)
            {
                itemList += item.name + " ";
            }

            var playerList = string.Empty;
            foreach (var item in room.players)
            {
                if (item.Name != playerName)
                {
                    if (item.Status == "Standing")
                    {
                        playerList += item.Name + " is here\r\n";
                    }
                    else if (item.Status == "Fighting")
                    {
                        playerList += item.Name + " is fighting " + item.Target.Name +"\r\n";
                    }
                   
                }
               
            }



            string displayRoom = roomTitle + "\r\n" + roomDescription + "\r\n Exits: " + exitList + "\r\n Items: " + itemList + "\r\n " + playerList;

            return displayRoom;

        }

        public static void ReturnRoom(Player player, Room room, string commandOptions = "", string keyword = "")
        {

            Room roomData = room;

            if (string.IsNullOrEmpty(commandOptions) && keyword == "look")
            {
                var roomInfo = DisplayRoom(roomData, player.Name);
                HubProxy.MimHubServer.Invoke("SendToClient", roomInfo, player.HubGuid);
            }
            else
            {

                int n = -1;
                string item = string.Empty;

                if (commandOptions.IndexOf('.') != -1)
                {
                    n = Convert.ToInt32(commandOptions.Substring(0, commandOptions.IndexOf('.')));
                    item = commandOptions.Substring(commandOptions.LastIndexOf('.') + 1);
                }


                var roomDescription = roomData.keywords.Find(x => x.name.ToLower().Contains(commandOptions));

                var itemDescription = (n == -1)
                                          ? roomData.items.Find(x => x.name.ToLower().Contains(commandOptions))
                                          : roomData.items.FindAll(x => x.name.ToLower().Contains(item))
                                                .Skip(n - 1)
                                                .FirstOrDefault();

                var mobDescription = roomData.mobs.Find(x => x.Name.ToLower().Contains(commandOptions));

                var playerDescription = roomData.players.Find(x => x.Name.ToLower().Contains(commandOptions));


                //Returns descriptions for important objects in the room
                if (roomDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;
                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = roomDescription.look;
                        broadcastAction = " looks at a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = roomDescription.examine;
                        broadcastAction = " looks closely at a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("touch"))
                    {
                        descriptionText = roomDescription.touch;
                        broadcastAction = " touches closely a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("smell"))
                    {
                        descriptionText = roomDescription.smell;
                        broadcastAction = " sniffs a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("taste"))
                    {
                        descriptionText = roomDescription.taste;
                        broadcastAction = " licks a " + roomDescription.name;
                    }

                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText, player.HubGuid);


                        foreach (var players in room.players)
                        {
                            if (player.Name != players.Name)
                            {
                                HubProxy.MimHubServer.Invoke("SendToClient", player.Name + broadcastAction, players.HubGuid);
                            }
                        }
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + roomDescription.name, player.HubGuid);
                    }

                }
                else if (itemDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;

                    if(keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase)) {

                        if (itemDescription.actions.container == true)
                        {
                            if (itemDescription.containerItems.Count > 0)
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " and see:", player.HubGuid);

                                foreach (var containerItem in itemDescription.containerItems)
                                {
                                    HubContext.SendToClient(containerItem.name, player.HubGuid);
                                }
                            }
                            else
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " but it is empty", player.HubGuid);
                            }
                           
                            HubContext.broadcastToRoom(player.Name + " looks in a " + itemDescription.name, room.players, player.HubGuid, true);
                        }
                        else
                        {
                            HubContext.SendToClient(itemDescription.name  + " is not a container", player.HubGuid);
                        }
                    }
                   else if (keyword.StartsWith("look"))
                    {
                        descriptionText = itemDescription.description.look;
                        broadcastAction = " looks at a " + itemDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = itemDescription.description.exam;
                        broadcastAction = " looks closely at a " + itemDescription.name;
                    }

                    if (!keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (!string.IsNullOrEmpty(descriptionText))
                        {
                            HubProxy.MimHubServer.Invoke("SendToClient", descriptionText, player.HubGuid);

                            foreach (var players in room.players)
                            {
                                if (player.Name != players.Name)
                                {
                                    HubProxy.MimHubServer.Invoke(
                                        "SendToClient",
                                        player.Name + broadcastAction,
                                        players.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            HubContext.SendToClient("You can't do that to a " + itemDescription.name, player.HubGuid);
                        }
                    }
                }
                else if (mobDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = mobDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText, player.HubGuid);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + mobDescription.Name, player.HubGuid);
                    }
                }
                else if (playerDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = playerDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText, player.HubGuid);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + playerDescription.Name, player.HubGuid);
                    }
                }
                else
                {
                    HubProxy.MimHubServer.Invoke("SendToClient", "You don't see that here.", player.HubGuid);
                }

            }
        }
    }
}
