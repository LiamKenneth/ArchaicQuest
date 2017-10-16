using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class Ferron
    {

        public static PlayerSetup.Player MetalMedleyBlacksmith()
        {

            #region NPC setup
            var ferron = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Ferron",
                NPCLongName = "Ferron",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The black smith",
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
                Shop = true,
                itemsToSell = new List<Item.Item>(),
                sellerMessage = "Why of course, here is what I can sell you.",
                GreetMessage = "Hello there!",
                DialogueTree = new List<DialogTree>(),
                Dialogue = new List<Responses>(),
                Quest = new List<Quest>()


            };
 
            #endregion

            for (int i = 0; i < 20; i++)
            {

                ferron.itemsToSell.Add(ShortSwordBasic.Saber());
                ferron.itemsToSell.Add(LongSwordBasic.BastardSword());
                ferron.itemsToSell.Add(SpearBasic.BoarSpear());
                ferron.itemsToSell.Add(AxeBasic.DoubleAxe());
                ferron.itemsToSell.Add(HammerBasic.GreatHammer());
                ferron.itemsToSell.Add(LongSwordBasic.Katana());
                ferron.itemsToSell.Add(DaggerBasic.HuntingKnife());
                ferron.itemsToSell.Add(DaggerBasic.HiddenBlade());

                //armor scale mail

                ferron.itemsToSell.Add(ScalemailHead.ScalemailHelm());
                ferron.itemsToSell.Add(ScalemailBody.ScalemailBreastPlate());
                ferron.itemsToSell.Add(ScalemailArms.ScalemailSleeves());
                ferron.itemsToSell.Add(ScalemailLegs.ScalemailGreaves());
                ferron.itemsToSell.Add(ScalemailHands.ScalemailGauntlets());
                ferron.itemsToSell.Add(ScalemailFeet.ScalemailBoots());

                //armour bronze - bronze has crappy stats, make better?
                ferron.itemsToSell.Add(FullPlateHelm.BronzeHelm());
                ferron.itemsToSell.Add(FullPlateBody.BronzeBreastPlate());
                ferron.itemsToSell.Add(FullPlateSleeves.BronzeSleeves());
                ferron.itemsToSell.Add(FullPlateGreaves.BronzeGreaves());
                ferron.itemsToSell.Add(FullPlateGauntlet.BronzeGauntlets());
                ferron.itemsToSell.Add(FullPlateBoots.BronzeBoots());
            }


            return ferron;
        }
    }
}