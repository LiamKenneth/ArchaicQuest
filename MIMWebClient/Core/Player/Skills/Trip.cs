using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Core.Player.Skills
{

    public class Trip: Skill
    {

        public static Skill TripSkill { get; set; }
        public static Skill TripAb()
        {
                  
            if (TripSkill != null)
            {
               return TripSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Trip",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "trip",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "trip help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                TripSkill = skill;
            }

            return TripSkill;
            
        }

        public void StartTrip(PlayerSetup.Player player, Room.Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, TripAb().Name);

            if (hasSkill == false)
            {
                HubContext.Instance.SendToClient("You don't know that skill.", player.HubGuid);
                return;
            }

            var canDoSkill = Skill.CanDoSkill(player);

            if (!canDoSkill)
            {
                return;
            }

            if (string.IsNullOrEmpty(target) && player.Target != null)
            {
                target = player.Target.Name;
            }

            var _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }

            if (player.ActiveSkill != null)
            {

                HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = TripAb();
            }




            if (_target != null)
            {


                if (player.MovePoints < TripAb().MovesCost)
                {


                    HubContext.Instance.SendToClient("You are too tired to use trip.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                //TODO REfactor

                player.MovePoints -= TripAb().MovesCost;

                Score.UpdateUiPrompt(player);

                Task.Run(() => DoTrip(player, _target, room));

            }
            else if (_target == null)
            {
                HubContext.Instance.SendToClient("You can't trip yourself", player.HubGuid);
            }


        }

        private async Task DoTrip(PlayerSetup.Player attacker, PlayerSetup.Player target, Room.Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < TripAb().MovesCost)
            {
                HubContext.Instance.SendToClient("You are too tired to use trip.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }

            var die = new PlayerStats();

            var dam = die.dice(5, 6);

            var toHit = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(TripAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss
            int chance = die.dice(1, 100);

            Fight2.ShowAttack(attacker, target, room, toHit, chance, TripAb(), dam);

            //trip / stun player


            Score.ReturnScoreUI(target);


            PlayerSetup.Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);




            attacker.ActiveSkill = null;

        }

    }
}
