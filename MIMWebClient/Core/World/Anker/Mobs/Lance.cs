using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.World.Anker.Mobs
{
    /// <summary>
    /// The village Elder
    /// </summary>
    public class Lance
    {

        public static PlayerSetup.Player VillageElderLance()
        {

            #region NPC setup
            var Lance = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Lance",
                NPCLongName = "Lance",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The elder of the village anker",
                Strength = 60,
                Dexterity = 60,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 3000,
                HitPoints = 3000,
                Level = 51,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Trainer = true,
                Greet = true,
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>(),
                MobTalkOnEnter = true


            };

            #endregion


            var welcomePlayers = new DialogTree
            {
                Id = "lance1",
                Message = "Greetings and welcome to Anker, I am the village Elder and I can help you if you need it.",
                PossibleResponse = new List<Responses>()
            };

            var lance1a = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1a",
                Response = "What is this place?"
            };

            var lance1b = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1b",
                Response = "What is there to do?"
            };

            var lance1c = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1c",
                Response = "Help I am stuck and confused"
            };

            var lance1d = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1d",
                Response = "It looks great, nice to meet you."
            };

            var lance1e = new Responses()
            {
                Keyword = new List<string>(),
                QuestionId = "lance1",
                AnswerId = "lance1e",
                Response = "Anything I can help you with?",
                QuestId = 1

            };

            var lance1aAnswer = new DialogTree()
            {
                Id = "lance1a",
                MatchPhrase = lance1a.Response,
                Message = "This is Anker a small fishing village, the 1st of many interesting places built on this world. It will take time but this place will feel alive soon. I am talking aien't I?",
                PossibleResponse = new List<Responses>()

            };

            lance1aAnswer.PossibleResponse.Add(lance1a);
            lance1aAnswer.PossibleResponse.Add(lance1b);
            lance1aAnswer.PossibleResponse.Add(lance1c);
            lance1aAnswer.PossibleResponse.Add(lance1d);
            lance1aAnswer.PossibleResponse.Add(lance1e);

            var lance1bAnswer = new DialogTree()
            {
                Id = "lance1b",
                MatchPhrase = lance1b.Response,
                Message = "Hmmm... The world is young $playerName but you can go north to the inn, Modo and Dyten may surprise you. North East houses the General shoppe and the blacksmith. Other than that you can walk around square walk. More changes happen everyday...",
                PossibleResponse = new List<Responses>()

            };

            lance1bAnswer.PossibleResponse.Add(lance1a);
            lance1bAnswer.PossibleResponse.Add(lance1b);
            lance1bAnswer.PossibleResponse.Add(lance1c);


            var lance1cAnswer = new DialogTree()
            {
                Id = "lance1c",
                MatchPhrase = lance1c.Response,
                Message = "Stuck? Oh, This can be a daunting place for newcommers. You know how to move right? you got here anyway. Move by typing the exit typically north, east, south etc can be shorten to just n,e,s,w. You can get or drop items. Wield or wear equipment and kill things. Stick to the cats.",
                PossibleResponse = new List<Responses>()

            };


            lance1cAnswer.PossibleResponse.Add(lance1a);
            lance1cAnswer.PossibleResponse.Add(lance1b);
            lance1cAnswer.PossibleResponse.Add(lance1c);

            var lance1dAnswer = new DialogTree()
            {
                Id = "lance1d",
                MatchPhrase = lance1d.Response,
                Message = "Thankyou $playerName, I am glad you like it. Run along now and enjoy your time here. The Gods will appreciate any feedback. Just send a prayer to liam.kenneth@hotmail.co.uk",
                PossibleResponse = new List<Responses>()

            };

            var lance1eAnswer = new DialogTree()
            {
                Id = "lance1d",
                MatchPhrase = lance1e.Response,
                Message = "Yes $playerName, I would like you to buy me a beer from Modo please. You may need some gold.",
                PossibleResponse = new List<Responses>(),
                GivePrerequisiteItem = true,
                GiveQuest = true,
                QuestId = 1


            };

            var whosModo = new DialogTree
            {
                Id = "lance2",
                Message = "He is the owner of the Red Lion, best beer in Anker I might add.",
                PossibleResponse = new List<Responses>()
            };







            Lance.DialogueTree.Add(welcomePlayers);
            Lance.DialogueTree.Add(lance1aAnswer);
            Lance.DialogueTree.Add(lance1cAnswer);
            Lance.DialogueTree.Add(lance1bAnswer);
            Lance.DialogueTree.Add(lance1dAnswer);
            Lance.DialogueTree.Add(lance1eAnswer);



            welcomePlayers.PossibleResponse.Add(lance1a);
            welcomePlayers.PossibleResponse.Add(lance1b);
            welcomePlayers.PossibleResponse.Add(lance1c);
            welcomePlayers.PossibleResponse.Add(lance1e);


            var findLance = new DialogTree
            {
                Id = "lance1",
                Message = "Yes I am Lance, you must be $playerName, well met. You look much better than when I last saw you. " +
                          "Do you remember much?",
                PossibleResponse = new List<Responses>(),
                ShowIfOnQuest = "Find and greet Lance",
                
                
            };

            var findLanceA = new Responses()
            {
                Response = "Yes I was with someone called Wilhelm, Did you find anyone else?",

            };

            var findLanceB = new Responses()
            {
                Response = "Know what happend to me?",

            };

            var findLance1aAnswer = new DialogTree()
            {
                Id = "findLanceA",
                MatchPhrase = findLanceA.Response,
                Message = "Sorry we only found you, there is a good chance that if he was captured they may of taken him to the caves east of here in the woods" +
                          " it is there we believe the goblins have been coming from",
                PossibleResponse = new List<Responses>(),
                GivePrerequisiteItem = true,
                GiveQuest = true,
                QuestId = 2
            };


            var findLance1bAnswer = new DialogTree()
            {
                Id = "findLanceB",
                MatchPhrase = findLanceA.Response,
                Message = "You had a nasty hit to the head when we found you, we suspected it was goblins. They have been raiding us for some time now. We need an adventure to find the source and stop them. Are you able to help us?",
                PossibleResponse = new List<Responses>(),
                GivePrerequisiteItem = true,
                GiveQuest = true,
                QuestId = 2
            };
            findLance.PossibleResponse.Add(findLanceA);
            findLance.PossibleResponse.Add(findLanceB);

            Lance.DialogueTree.Add(findLance1aAnswer);
            Lance.DialogueTree.Add(findLance1bAnswer);
            Lance.DialogueTree.Add(findLance);


            //quest


            var getStarted = new Quest()
            {
                Id = 2,
                Description = "Investigate the origins of the Goblins",
                Name = "Find and remove the source of the Goblins",
                Type = Quest.QuestType.Kill,
                QuestFindMob = "Shaman",
                RewardXp = 1000,
                RewardGold = 500,
                PrerequisiteItem = new List<Item.Item>()
                {
                    new Item.Item()
                {
                    name = "Gold",
                    type = Item.Item.ItemType.Gold,
                    Gold = 120
                }
                },
                QuestGiver = Lance.Name,
                PrerequisiteItemEmote = "puts his hands in his money pounch and hands you some gold. \"Use this to get yourself a weapon and some armor from the shops north east of Anker.\" ",
                AlreadyOnQuestMessage = "The goblins raid us everyday, have you found them yet?",
                RewardDialog = new DialogTree()
                {
                    Message = "You are our hero",
                    PossibleResponse = new List<Responses>()
                }



            };


            var getBeer = new Quest()
            {
                Id = 1,
                Description = "I would like some beer from Modo please",
                Name = "Buy beer from modo and give to lance",
                Type = Quest.QuestType.FindItem,
                QuestItem = new List<Item.Item>()
                {
                  new Item.Item() { 
                    name = "Light Beer",
                    type = Item.Item.ItemType.Drink,
                    Gold = 3,
                    description = new Description
                    {
                        look = "A weak looking flat beer bubbles in a bottle",
                        room = "A beer has been left on the floor"
                    },
                    slot = Item.Item.EqSlot.Hands
}
                },
                RewardXp = 200,
                QuestGiver = Lance.Name,
                PrerequisiteItemEmote = "puts his hands in his money pounch and hands you some gold.",
                AlreadyOnQuestMessage = "Yeah I told you to get me some beer from Modo.",
                RewardDialog = new DialogTree()
                {
                    Message = "Ah Thank you so much!",
                    PossibleResponse = new List<Responses>()
                }



            };

            Lance.Quest.Add(getBeer);
            Lance.Quest.Add(getStarted);
            return Lance;
        }
    }
}