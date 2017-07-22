using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Update
{
    public static class Time
    {
        private static int tickCount = 0;

        private static int ticksInDay = 48;

        private static int ticksElapsedSinceSave = 0;

        private static int autoSaveTick = 300000;

        private static int sunrise = 11;

        private static int morning = 14;

        private static int midDay = 24;

        private static int afternoon = 30;

        private static int sunset = 36;

        private static int moonRise = 40;

        private static int night = 42;

        private static int midNight = 48;

        private static int twilight = 8;

        private static int hour = 0;

        private static int minute = 0;


        public static void UpdateTIme()
        {
            ticksElapsedSinceSave += 1;
            tickCount += 1;

            if (tickCount == 48)
            {
                tickCount = 0;
            }

            if (tickCount != 0)
            {

                if (minute == 30)
                {
                    hour += 1;

                    if (hour == 24)
                    {
                        hour = 0;
                    }
                }

                minute += 30;

                if (minute == 60)
                {
                    minute = 0;
                }

            }

            if (tickCount <= 35)
            {

                switch (tickCount)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage(
                            "The moon is slowly moving west across the sky.");
                        break;
                    case 9:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage("The moon slowly sets in the west.");
                        break;
                    case 11:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage("The sun slowly rises from the east.");
                        break;
                    case 13:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage(
                            "The sun has risen from the east, the day has begun.");
                        break;
                    case 24:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage("The sun is high in the sky.");
                        break;
                    default:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage(
                            "The sun is slowly moving west across the sky.");
                        break;
                }


            }
            else
            {
                switch (tickCount)
                {
                    case 36:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage("The sun slowly sets in the west.");
                        break;
                    case 40:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage("The moon slowly rises in the west.");
                        break;
                    case 43:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage(
                            "The moon has risen from the east, the night has begun.");
                        break;
                    default:
                        HubContext.getHubContext.Clients.All.addNewMessageToPage("The moon is high in the sky.");
                        break;


                }
            }


            if (tickCount == ticksInDay)
            {
                //New day reset
                tickCount = 0;
            }


            if (autoSaveTick == ticksElapsedSinceSave)
            {
                //SAve world/ players
            }

         

            RestoreVitals.UpdatePlayers();
            //heal players / mobs


        }

        public static bool isDay()
        {

            if (hour >= sunrise && hour <= moonRise)
            {
                return true;
            }
            return false;
        }

        public static void ShowTime()
        {

            HubContext.getHubContext.Clients.All.addNewMessageToPage("The time is " + AddZero(hour) + ":" + AddZero(minute));

        }

        private static string AddZero(int time)
        {
            if (time.ToString().Length == 1)
                {
                    return "0" + time;
                }

            return time.ToString();
        }
    }
}