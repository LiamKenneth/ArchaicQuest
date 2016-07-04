using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.NPC
{
    public class NPC
    {
        /// <summary>
        /// npc ID
        /// </summary>
        Guid npcId;
        /// <summary>
        /// Collection of emotes that randomly send on tick
        /// </summary>
        List<string> Emote;
        /// <summary>
        /// Does the Npc Roam
        /// </summary>
        bool roam;
        /// <summary>
        /// Attackers Player or mob on Enter
        /// </summary>
        bool aggresive;
        /// <summary>
        /// Greets player on enter
        /// </summary>
        bool greet;
        /// <summary>
        /// NPC is a shop
        /// </summary>
        bool shop;
        /// <summary>
        /// NPC is a trainer
        /// </summary>
        bool trainer;
        /// <summary>
        /// NPC is a guard
        /// </summary>
        bool guard;



    }
}