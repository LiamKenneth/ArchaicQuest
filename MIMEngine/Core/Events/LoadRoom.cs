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

    
       
        public static JObject LoadRoomFile()
        {
            JObject roomJson = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/World/Area/Valston/Town.json"));

            return roomJson;
        }


        public string DisplayRoom()
        {

          var roomJson = LoadRoomFile();

          string roomTitle = (string)roomJson["title"];
          string roomDescription = (string)roomJson["description"];

            string displayRoom = "You look around " + "\r\n" + roomTitle + "\r\n" + roomDescription;

            return displayRoom;

        }
    }
}
