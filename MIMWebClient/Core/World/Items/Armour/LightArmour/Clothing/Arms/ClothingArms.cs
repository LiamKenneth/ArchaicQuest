using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Arms
{
    public static class ClothingArms
    {
        public static Item.Item WoolenClothSleeves()
        {

            return new Item.Item
            {
                name = "Woolen Cloth Sleeves",
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Arms,
                description = new Description()
                {
                    look = "This is a simple woolen sleeves which provides basic protection and warmth.",
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Arms,
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