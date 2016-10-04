using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player.Skills
{
    using System.Runtime.CompilerServices;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Kick: Skill
    {

        public static Skill KickSkill { get; set; }
        public static void KickAb(Player attacker, Player defender, Room room)
        {
           
            Skill kick = null;

            if (KickSkill != null)
            {
                kick = KickSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Kick",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Passive = false,
                    Proficiency = 75,
                    UsableFromStatus = "Standing",
                    Syntax = "Kick <Target>"
                };


                skill.HelpText = skill.Syntax + " Some help text about kick";

                KickSkill = skill;
            }

            if (attacker.Level >= kick.LevelObtained)
            {
                if (attacker.Target != null)
                {
                    if (attacker.Status == Player.PlayerStatus.Standing ||  attacker.Status == Player.PlayerStatus.Fighting)
                    {
                        return;
                    }

                    var die = new PlayerStats();
                     var dam = die.dice(1, attacker.Strength);

                    if (defender.HitPoints > 0)
                    {
                        defender.HitPoints -= dam;
                    }

                    if (defender.HitPoints <= 0)
                    {
                        defender.HitPoints = 0;
                      
                    }


                    HubContext.SendToClient("You kick " + defender.Name, attacker.HubGuid);
                        HubContext.SendToClient(attacker.Name + " kicks you", defender.HubGuid);
                    
                }
                else
                {
                    HubContext.SendToClient("You are not fighting anyone ", attacker.HubGuid);
                }

            }

            
        }

    }
}
