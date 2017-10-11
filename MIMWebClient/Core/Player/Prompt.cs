using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player
{
    using MIMWebClient.Core.PlayerSetup;

    public class Prompt
    {
        public static void ShowPrompt(Player player)
        {
            int hp = PlayerStats.GetHp(player);
            int maxHP = PlayerStats.GetMaxHp(player);
            int mana = PlayerStats.GetMana(player);
            int maxMana = PlayerStats.GetMaxMana(player);
            int move = PlayerStats.GetMove(player);
            int maxMove= PlayerStats.GetMaxMove(player);

            var prompt = new StringBuilder();

            prompt.Append("<")
                .Append(hp)
                .Append("/")
                .Append(maxHP)
                .Append("HP ")
                .Append(mana)
                .Append("/")
                .Append(maxMana)
                .Append("MP ")
                .Append(move)
                .Append("/")
                .Append(maxMove)
                .Append("Mvs")
                .Append(">");
               
             
            HubContext.Instance.SendToClient(prompt.ToString(), player.HubGuid);
        }
    }
}
