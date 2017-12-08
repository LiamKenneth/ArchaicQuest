using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Feet
{
    public static class ClothingFeet
    {

        public static Item.Item WoolenClothBoots()
        {

            return new Item.Item
            {
                name = "Woolen Cloth Boots",
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Feet,
                description = new Description()
                {
                    look = "This is a simple woolen boots which provides basic protection and warmth.",
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Feet,
                type = Item.Item.ItemType.Armour,
                ArmorRating = new ArmourRating()
                {
                    Armour = 3,
                    Magic = 0
                },
                Weight = 2,
                equipable = true,
                stats = new Stats()
                {
                    minUsageLevel = 5
                }

            };

        }

    }
}