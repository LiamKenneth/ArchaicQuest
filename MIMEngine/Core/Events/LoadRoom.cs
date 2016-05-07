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
            var roomExitObj = roomJson.Property("exits").Children();

          
            string exitList = null;
            foreach (var exit in roomExitObj)
            {
                if (exit["North"] != null)
                {
                    exitList += exit["North"]["name"];
                }

                if (exit["East"] != null)
                {
                    exitList += exit["East"]["name"];
                }

                if (exit["South"] != null)
                {
                    exitList += exit["South"]["name"];
                }

                if (exit["West"] != null)
                {
                    exitList += exit["West"]["name"];
                }

                if (exit["Up"] != null)
                {
                    exitList += exit["Up"]["name"];
                }

                if (exit["Down"] != null)
                {
                    exitList += exit["Down"]["name"];
                }

            }


            string displayRoom = "You look around " + "\r\n" + roomTitle + "\r\n" + roomDescription + "\r\n" + "Obviuse Exits:\r\n" + exitList;

            return displayRoom;

        }
    }
}
