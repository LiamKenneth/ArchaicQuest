using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.World.Items.MiscEQ.Light;

namespace MIMWebClient.Core.Player.Skills
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class ContinualLight : Skill
    {
        private static bool _taskRunnning = false;
        private static Item.Item _target = new Item.Item();
        private static string _color = "white";
        public static Skill ContinualLightSkill { get; set; }

        public static void StarContinualLight (Player player, Room room, string commandOptions = "")
        {
 
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, ContinualLightAb().Name);

            if (hasSpell == false)
            {
                HubContext.SendToClient("You don't know that spell.", player.HubGuid);
                return;
            }

            #region refactor

            string[] options = commandOptions.Split(' ');
            int nth = -1;
            string getNth = string.Empty;
            string objectToFind = String.Empty;
 

            if (options.Length == 3)
            {
                objectToFind = options[2];

                if (objectToFind.IndexOf('.') != -1)
                {
                    getNth = objectToFind.Substring(0, objectToFind.IndexOf('.'));
                    int.TryParse(getNth, out nth);
                }


            }
            else if (options.Length > 3)
            {
                objectToFind = options[2];

                if (objectToFind.IndexOf('.') != -1)
                {
                    getNth = objectToFind.Substring(0, objectToFind.IndexOf('.'));
                    int.TryParse(getNth, out nth);
                }

                _color = options[3];

            }

            #endregion

            if (nth == 0) {  nth = -1;  }


            _target = FindItem.Item(player.Inventory, nth, objectToFind, Item.Item.ItemLocation.Inventory);

            if (_target == null && options.Length == 3)
            {
                _color = options[2];
            }


            if (ReturnColor(_color) == null)
            {
                HubContext.SendToClient($"{_color} is not valid, you can choose from: Blue, Red, Green, Yellow, Purple, Orange and White", player.HubGuid);

                return;
            }
 
            if (!_taskRunnning && _target != null)
            {

                if (player.ManaPoints < ContinualLightAb().ManaCost)
                {
                    HubContext.SendToClient("You clasp your hands together attempting to draw energy between your hands but fail", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= ContinualLightAb().ManaCost;

                Score.UpdateUiPrompt(player);

                if (_target.itemFlags == null)
                {
                    _target.itemFlags = new List<Item.Item.ItemFlags>();
                }

                if (_target.itemFlags.Contains(Item.Item.ItemFlags.glow))
                {
                    HubContext.SendToClient("This item is already illuminated", player.HubGuid);
                    return;
                }


                var result = AvsAnLib.AvsAn.Query(_target.name);
                string article = result.Article;


                HubContext.SendToClient($"You grasp {article} {_target.name} between your hands which starts to shimmer a slight {_color} colour", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                HubContext.broadcastToRoom(Helpers.ReturnName(player, null) + $" grasps {article} {_target.name} between {Helpers.ReturnHisOrHers(player.Gender)} hands which starts to shimmer a slight {_color} colour", playersInRoom, player.HubGuid, true);



                Task.Run(() => DoContinualLight(player, room));

            }
            else
            {
                if (_target == null)
                {

                    //TODO REfactor
                    player.ManaPoints -= ContinualLightAb().ManaCost;

                    Score.UpdateUiPrompt(player);
                
                    HubContext.SendToClient($"You clasp your hands together forming a bright {_color} ball between them", player.HubGuid);

                    HubContext.broadcastToRoom(Helpers.ReturnName(player, null) + " 's hands start to glow as they begin chanting the Continual light spell", room.players, player.HubGuid, true);

                    Task.Run(() => DoContinualLight(player, room));
                     
                }

                 

            }

        }

        private static async Task DoContinualLight(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            if (_target == null)
            {
                var castingTextAttacker =  $"A bright {_color} ball of light is released by your hands and hovers in the air.";

                var castingTextRoom =  $"{Helpers.ReturnName(attacker, null)} releases a {_color} bright ball of light which hovers in the air.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);

                var excludePlayers = new List<string> {attacker.HubGuid};

                HubContext.SendToAllExcept(castingTextRoom, excludePlayers, room.players);

                var ballOfLight = Light.BallOfLight();
                ballOfLight.description = new Description()
                {
                    exam = $"A bright {_color} ball of light hovers here.",
                    look = $"A bright {_color} ball of light hovers here.",
                    room = $"A bright {_color} ball of light hovers here."
                };
                ballOfLight.name = $"A bright {_color} ball of light";
                ballOfLight.location = Item.Item.ItemLocation.Room;
 

                room.items.Add(ballOfLight);

            }
            else
            {
                var castingTextAttacker =  $"The {_target.name} glows a bright {_color} colour.";

                var castingTextRoom = $"The {_target.name} glows a bright {_color} colour.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.broadcastToRoom(castingTextRoom, room.players, attacker.HubGuid, true);

                _target.itemFlags.Add(Item.Item.ItemFlags.glow);

            }

            _target = null;
            _taskRunnning = false;
     

        }

        private static string ReturnColor(string color)
        {
            var allowedColours = new List<string> {"Blue", "Red", "Green", "Yellow", "Purple", "Orange", "White"};

            foreach (var allowedColor in allowedColours)
            {
                if (allowedColor.Equals(color, StringComparison.CurrentCultureIgnoreCase))
                {
                    return allowedColor;
                }
            }

            return null;
        }

        public static Skill ContinualLightAb()
        {


            var skill = new Skill
            {
                Name = "Continual light",
                SpellGroup = SpellGroupType.Abjuration,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 2,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = " cast 'continual light' <object> / <colour>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "This spell creates a white ball of light, which you can hold as a light source." +
                           " The ball of light will last indefinitely. It may also be used on an object" +
                           " to give it an enchanted glow. You may also cast a certain colour of light too. See help colour",
                DateUpdated = "05/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
