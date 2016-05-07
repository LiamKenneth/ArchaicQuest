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
   public class LoadRoom
    {
       public string Region { get; set; }
        public string Area { get; set; }
        public int id { get; set; }


        public JObject LoadRoomFile()
        {
            JObject roomJson = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/World/Area/" + this.Region + "/" + this.Area + ".json"));

            return roomJson;
        }


        public string DisplayRoom(JObject room)
        {

            var roomJson = room;

          string roomTitle = (string)roomJson["title"];
          string roomDescription = (string)roomJson["description"];

            string displayRoom = "You look around " + "\r\n" + roomTitle + "\r\n" + roomDescription;

            return displayRoom;

        }
    }
}
