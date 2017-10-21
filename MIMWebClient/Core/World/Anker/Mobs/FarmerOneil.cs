using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Arms;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Body;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Feet;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Hands;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Head;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Legs;
using MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Arms;
using MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Body;
using MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Feet;
using MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Hands;
using MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Head;
using MIMWebClient.Core.World.Items.Armour.MediumArmour.ScaleMail.Legs;
using MIMWebClient.Core.World.Items.Weapons.Axe;
using MIMWebClient.Core.World.Items.Weapons.Blunt;
using MIMWebClient.Core.World.Items.Weapons.DaggerBasic;
using MIMWebClient.Core.World.Items.Weapons.Spear;
using MIMWebClient.Core.World.Items.Weapons.Sword.Long;
using MIMWebClient.Core.World.Items.Weapons.Sword.Short;

namespace MIMWebClient.Core.World.Anker.Mobs
{
    public class FarmerOneil
    {

        public static PlayerSetup.Player Farmer()
        {

            #region NPC setup
            var farmer = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Farmer O'neil",
                NPCLongName = "Farmer O'neil",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The farmer",
                Strength = 80,
                Dexterity = 60,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 2250,
                HitPoints = 2250,
                Level = 51,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Trainer = false,
                Greet = true,
                itemsToSell = new List<Item.Item>(),
                MobTalkOnEnter = false,

                DialogueTree = new List<DialogTree>()
                {
                    new DialogTree()
                    {
                        
                        Id = "Farmer O'neil1",
                        Message = "Those pesky rabbits are eating all my carrots, I have no gold but will give you an old fishing rod if you help me?",
                        PossibleResponse =  new List<Responses>()
                        {
                            new Responses()
                            {
                                QuestionId = "Farmer O'neil1a",
                                AnswerId = "Farmer O'neil1a",
                                Response = "Ok sure, I&#x27;ll help you but I want gold too old man, everything you got.",
                                DontShowIfOnQuest = "Help Farmer O'neil with with his rabbit problem"
                               

                            },
                            new Responses()
                            {
                                QuestionId = "Farmer O'neil1b",
                                AnswerId = "Farmer O'neil1b",
                                Response = "I&#x27;ll help you out no worries.",
                                DontShowIfOnQuest =   "Help Farmer O'neil with with his rabbit problem"
                              

                            },
                            new Responses()
                            {
                                QuestionId = "Farmer O'neil1c",
                                AnswerId = "Farmer O'neil1c",
                                Response = "I have killed them all now.",
                                ShowIfOnQuest = "Help Farmer O'neil with with his rabbit problem"
                                

                            }


                        },
                        
                    },
                    new DialogTree()
                    {
                        Id = "Farmer O'neil1a",
                        Message = "Fine, if you insist. All I have is under my bed. I&#x27;ll give it to you once you have helped me",
                        MatchPhrase = "Ok sure, I'll help you but I want gold too old man, everything you got.",
                        PossibleResponse =  new List<Responses>()
                        {
                            new Responses()
                            {
                                QuestionId = "Farmer O'neil2a",
                                AnswerId =  "Farmer O'neil2a",
                                Response = "Thanks, I&#x27;ll just kill you now then and take your gold, fishing rod and eat your carrorts.",

                            },
                            new Responses()
                            {
                                QuestionId = "Farmer O'neil2b",
                                AnswerId =  "Farmer O'neil2b",
                                Response = "Consider it done.",

                            },

                        }
                    },
                    new DialogTree()
                    {
                        Id = "Farmer O'neil2b",
                        Message = "Thank you very much, you will find my farm south of here. Can&#x27;t miss it",
                        MatchPhrase = "I'll help you out no worries.",
                        QuestId =  1,
                        GiveQuest = true,
                        PossibleResponse = new List<Responses>()
                    }
                    ,
                    new DialogTree()
                    {
                        Id = "Farmer O'neil2a",
                        Message = "Please don't I just want those rabbits gone!",
                        MatchPhrase = "Thanks, I'll just kill you now then and take your gold, fishing rod and eat your carrots.",
                        QuestId =  1,
                        GiveQuest = true,
                        PossibleResponse = new List<Responses>()


                    },
                    new DialogTree()
                    {
                    Id = "Farmer O'neil1c",
                    Message = "Have you killed all the rabbits yet?",
                   ShowIfOnQuest = "Help Farmer O'neil with with his rabbit problem",
                    PossibleResponse = new List<Responses>(),
                   
                    
                }
                ,
                },


                Quest = new List<Quest>()
                {
                    new Quest()
                    {
                        AlreadyOnQuestMessage = "Have you killed all the rabits yet?",
                        Description = "Kill all 4 of the rabbits eating the crops.",
                        Id = 1,
                        Name = "Help Farmer O'neil with with his rabbit problem",
                        QuestKills = 4,
                        QuestGiver =  "Farmer O'neil",
                        QuestKill = Rabit.SmallRabbit(),
                        Type = Quest.QuestType.Kill,
                        RewardGold = 100,
                        RewardXp = 1500,
                        RewardDialog = new DialogTree()
                        {
                            Message = "Thank you so much $playerName. Here is your fishing rod"
                        },
                        Completed = false,
                        RewardItem = new  Item.Item()
                        {
                            name = "basic old fishing rod",
                            location = Item.Item.ItemLocation.Inventory,
                            slot = Item.Item.EqSlot.Held,
                            eqSlot = Item.Item.EqSlot.Held,
                            description = new Description()
                            {
                                look = "This is an old long wooden fishing rod, looks to be well used. There have been other methods for catching fish, though the use of a rod like this one is the tried and tested, and most successful, method.",
                                
                            }
                            
                        }
                    }
                }


            };

            #endregion



            return farmer;
        }
    }
}