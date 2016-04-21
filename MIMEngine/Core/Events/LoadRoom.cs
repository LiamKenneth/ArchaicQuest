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
        public string DisplayRoom()
        {
            JObject roomJson = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory +"bin/World/Area/Valston/Town.json"));

          string roomTitle = (string)roomJson["title"];
          string roomDescription = (string)roomJson["description"];

            string displayRoom = roomTitle + "/r/n" + roomDescription;

            return displayRoom;

        }
    }
}
