using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
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
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "The black smith",
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
 
            #endregion

            ferron.itemsToSell.Add(ShortSwordBasic.Saber());
            ferron.itemsToSell.Add(LongSwordBasic.BastardSword());
            ferron.itemsToSell.Add(SpearBasic.BoarSpear());
            ferron.itemsToSell.Add(AxeBasic.DoubleAxe());
            ferron.itemsToSell.Add(HammerBasic.GreatHammer());
            ferron.itemsToSell.Add(LongSwordBasic.Katana());
            ferron.itemsToSell.Add(DaggerBasic.HuntingKnife());
            ferron.itemsToSell.Add(DaggerBasic.HiddenBlade());
            return ferron;
        }
    }
}