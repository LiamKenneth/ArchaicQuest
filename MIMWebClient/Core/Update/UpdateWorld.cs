using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIMWebClient.Core.Update
{
    public class UpdateWorld
    {
        

        public static void Init()
        {
            Task.Run(UpdateTime);
        }

        public static void CleanRoom()
        {
            Task.Run(UpdateRoom);
        }

        public static void UpdateMob()
        {
            Task.Run(MoveMob);
        }
        /// <summary>
        /// Wahoo! This works
        /// Now needs to update player/mob stats, spells, reset rooms and mobs. Move mobs around?
        /// Global Timer every 60 seconds? quicker timer for mob movement?
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateTime()
        {
            await Task.Delay(75000);
    
            Time.UpdateTIme();

            Init();
        }


        public static async Task UpdateRoom()
        {
            await Task.Delay(300000);

            HubContext.getHubContext.Clients.All.addNewMessageToPage("This is will update Rooms every 5 minutes and not block the game");

            CleanRoom();
        }

        public static async Task MoveMob()
        {
            //await Task.Delay(5000);

            //HubContext.getHubContext.Clients.All.addNewMessageToPage("This task will update Mobs every 5 seconds and not block the game");

            //UpdateMob();
        }
    }
}