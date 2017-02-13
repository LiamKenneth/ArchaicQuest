using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
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
    public class Anika
    {

        public static PlayerSetup.Player OddsNSodsShopKeeper()
        {

            #region NPC setup
            var Anika = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Anika",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The shop keeper",
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
                Trainer = false,
                Greet = true,
                Shop = true,
                itemsToSell = new List<Item.Item>(),
                sellerMessage = "Why of course, here is what I can sell you.",
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()


            };


            var backpack = new Item.Item
            {
                name = "Leather Backpack",
                container = true,
                containerSize = 30,
                Weight = 1,
                count = 10,
                equipable = false,
                location = Item.Item.ItemLocation.Inventory,
                canOpen = true,
                Gold = 25
            };

            Anika.itemsToSell.Add(ShortSwordBasic.ShortIronSword());
            Anika.itemsToSell.Add(DaggerBasic.IronDagger());
            Anika.itemsToSell.Add(LongSwordBasic.LongIronSword());
            Anika.itemsToSell.Add(StaffBasic.WoodenQuarterstaff());
            Anika.itemsToSell.Add(AxeBasic.IronHatchet());
            Anika.itemsToSell.Add(MaceBasic.IronMace());
            Anika.itemsToSell.Add(PaddedBodyBasic.PaddedBreastPlate());
            Anika.itemsToSell.Add(PaddedSleeves.PaddedSleeve());
            Anika.itemsToSell.Add(PaddedHandsBasic.PaddedGloves());
            Anika.itemsToSell.Add(PaddedLegsBasic.PaddedGreaves());
            Anika.itemsToSell.Add(LeatherBootBasic.WornLeatherBoots());
            Anika.itemsToSell.Add(Light.WoodenTorch());
            Anika.itemsToSell.Add(drink.WaterSkin());
            Anika.itemsToSell.Add(Food.Cheese());
            Anika.itemsToSell.Add(backpack);
            #endregion


          


            return Anika;
        }
    }
}