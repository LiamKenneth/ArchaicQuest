using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player
{

    /// <summary>
    /// When to check and use affects:
    /// Fighting : haste, weaken, blind
    /// 
    /// </summary>
    public class Effect
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string AffectLossMessagePlayer { get; set; }
        public string AffectLossMessageRoom { get; set; }

        public static void Show(PlayerSetup.Player player)
        {
            try
            {
                if (player.Effects != null)
                {

                    if (player.Effects.Count > 0)
                    {
                        HubContext.Instance.SendToClient("You are affected by the following affects:", player.HubGuid);

                        foreach (var affect in player.Effects)
                        {
                            HubContext.Instance.SendToClient(affect.Name + " (" + affect.Duration + ") ticks ", player.HubGuid);
                        }
                    }
                    else
                    {
                        HubContext.Instance.SendToClient("You are not affected by anything.", player.HubGuid);
                    }
                }
                else
                {
                    HubContext.Instance.SendToClient("You are not affected by anything.", player.HubGuid);
                }
            }

            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "return name"
                };

                Save.LogError(log);
            }

        }

        public static bool HasEffect(PlayerSetup.Player player, string effectName)
        {
          return player.Effects?.FirstOrDefault(
                x => x.Name.Equals(effectName, StringComparison.CurrentCultureIgnoreCase)) != null;
        }

        public static Effect Blindness(PlayerSetup.Player player, bool magical = false)
        {
          return new Effect
            {
                Name = "Blindness",
                Duration = magical ? player.Level + 5 : Helpers.Rand(1, 5),
                AffectLossMessagePlayer = magical ? "Your eyes regain the ability to see again." : "You rub the dirt from your eyes.",
                AffectLossMessageRoom = magical ? "'s regain the ability to see again." : " rubs the dirt from their eyes."
            };
        }

    }
}