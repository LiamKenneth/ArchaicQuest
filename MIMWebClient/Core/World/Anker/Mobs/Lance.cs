using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.World.Anker.Mobs
{
    public class Lance
    {

        public static PlayerSetup.Player VillageElderLance()
        {

            #region NPC setup
            var Lance = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Lance",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The elder of the village anker",
                Strength = 15,
                Dexterity = 16,
                Constitution = 16,
                Intelligence = 12,
                Wisdom = 16,
                Charisma = 18,
                MaxHitPoints = 350,
                HitPoints = 350,
                Level = 20,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
                Trainer = true,
                Greet = true,
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()


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
                Message = "Hmmm... The world is young $playerName but you can go north to the inn, Modo and Dysten may surprise you. North East houses the General shoppe and the blacksmith. Other than that you can walk around square walk. More changes happen everyday...",
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
                Message = "Yes I am Lance, well met $playerName",
                PossibleResponse = new List<Responses>(),
                ShowIfOnQuest = "Find and greet Lance",
                
            };

            var findLanceA = new Responses()
            {
                Response = "Well wtf do I do now?",

            };

            findLance.PossibleResponse.Add(findLanceA);

            Lance.DialogueTree.Add(findLance);


            //quest

            var getBeer = new Quest()
            {
                Id = 1,
                Description = "I would like some beer from Modo please",
                Name = "Buy beer from modo and give to lance",
                Type = Quest.QuestType.FindItem,
                QuestItem = new Item.Item
                {
                    name = "Light Beer",
                    type = Item.Item.ItemType.Drink,
                    Gold = 3,
                    description = new Description
                    {
                        look = "A weak looking flat beer bubbles in a bottle",
                        room = "A beer has been left on the floor"
                    },
                    slot = Item.Item.EqSlot.Hands
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

            return Lance;
        }
    }
}