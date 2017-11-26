using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Feet;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Arms;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Body;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Hands;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Padded.Legs;
using MIMWebClient.Core.World.Items.Consumables.Drinks;
using MIMWebClient.Core.World.Items.Consumables.Food;
using MIMWebClient.Core.World.Items.MiscEQ.Light;
using MIMWebClient.Core.World.Items.Weapons.Axe;
using MIMWebClient.Core.World.Items.Weapons.Blunt;
using MIMWebClient.Core.World.Items.Weapons.DaggerBasic;
using MIMWebClient.Core.World.Items.Weapons.Staff;
using MIMWebClient.Core.World.Items.Weapons.Sword.Long;
using MIMWebClient.Core.World.Items.Weapons.Sword.Short;

namespace MIMWebClient.Core.World.Anker.Mobs
{
    public class Horik
    {
        /// <summary>
        /// alice walks from her home to the shop to talk to anika before buying a pie
        /// she then walks back home and puts the pie o nthe table, before sitting at the table
        /// and eating the pie.
        /// She then goes to sleep and repeats
        /// </summary>
        /// <returns></returns>
        public static PlayerSetup.Player HorikNpc()
        {


            var horik = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Horik",
                NPCLongName = "Horik the lumberjack",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "You see nothing special about him.",
                Strength = 70,
                Dexterity = 72,
                Constitution = 60,
                Intelligence = 60,
                Wisdom = 60,
                Charisma = 60,
                MaxHitPoints = 722,
                HitPoints = 722,
                Level = 23,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new ItemContainer(),
                Trainer = false,
                Greet = true,
                Shop = false,
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>()
                {
                    new DialogTree()
                    {

                        Id = "Horik1",
                        Message = "Hi there, do you mind helping me get my Axe repaired?",
                        DontShowIfOnQuest = "Repair Horik's Axe",
                        PossibleResponse =  new List<Responses>()
                        {
                            new Responses()
                            {
                                QuestionId = "Horik1a",
                                AnswerId = "Horik1a",
                                Response = "Yes, I can help you."


                            },
                            new Responses()
                            {
                                QuestionId = "Horik1b",
                                AnswerId = "Horik1b",
                                Response = "Sorry not right now."
                            }


                        },

                    },
                    new DialogTree()
                    {

                        Id = "Horik1a",
                        Message = "Thanks, take this broken axe and speak to Ferron he will know what to do. When you come back I can show you how to chop wood as a reward.",
                        MatchPhrase = "Yes, I can help you.",
                        PossibleResponse =  new List<Responses>(),
                        GivePrerequisiteItem = true,
                        GiveQuest = true,
                        QuestId =  1,
                        


                    },
                     new DialogTree()
                    {

                        Id = "Horik1b",
                        Message = "No worries, come back when you have time.",
                          MatchPhrase = "Sorry not right now.",
                        PossibleResponse =  new List<Responses>()


                    },
                    new DialogTree()
                    {
                    Id = "Horik1c",
                    Message = "Have you repaired my axe yet?",
                   ShowIfOnQuest = "Repair Horik's Axe",
                    PossibleResponse = new List<Responses>(),

}

                },
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()
                {
                                 new Quest()
                    {
                        AlreadyOnQuestMessage = "Do you have the axe just give it to me please.",
                        Description = "Repair Horik's Axe",
                        Id = 1,
                        Name = "Repair Horik's Axe",
                        QuestGiver =  "Horik",
                        Type = Quest.QuestType.FindItem,
                        QuestItem = new List<Item.Item>() {
                            new Item.Item()
                        {
                            name = "Horik's Axe",
                            location = Item.Item.ItemLocation.Inventory,
                            QuestItem = true,
                            eqSlot = Item.Item.EqSlot.Held,
                            slot = Item.Item.EqSlot.Held,
                            description = new Description()
                            {
                                look =  "The shaft of this axe has broken in half and the blade has many chips along it's once sharp edge."
                            },
                            equipable = true,
                            weaponType = Item.Item.WeaponType.Axe

                        }},
                        RewardGold = 100,
                        RewardXp = 1500,
                        RewardDialog = new DialogTree()
                        {
                            Message = "Thank you so much $playerName. To chop wood, just wield an axe and chop the wood. Here you can actually keep the axe, I found my spare axe while you were out. Good luck!"
                        },
                        PrerequisiteItem = new List<Item.Item>()
                        {
                            new Item.Item()
                            {
                                name = "Horik's Broken Axe",
                                location = Item.Item.ItemLocation.Inventory,
                                QuestItem = true,
                                eqSlot = Item.Item.EqSlot.Held,
                                slot = Item.Item.EqSlot.Held,
                                description = new Description()
                                {
                                    look =  "The shaft of this axe has broken in half and the blade has many chips along it's once sharp edge."
                                },
                                equipable = true,
                                weaponType = Item.Item.WeaponType.Axe,
                                Condition = 60
                                
                            }
                        },
                        Completed = false,
                        RewardItem =  AxeBasic.IronHatchet(),
                        RewardSkill = Chopping.ChoppingAb()
                    }
                },
                Region = "Anker",
                Area = "Anker",
                AreaId = 28,
                ArmorRating = 280,
                Recall = new Recall()
                {
                    Region = "Anker",
                    Area = "Anker Farm",
                    AreaId = 12
                }
            };



            #region Dialogue



            #endregion

            return horik;

        }
    }
}