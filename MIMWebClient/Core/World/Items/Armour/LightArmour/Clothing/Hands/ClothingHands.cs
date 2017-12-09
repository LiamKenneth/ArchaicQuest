using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Hands
{
    public static class ClothingHands
    {


        public static Item.Item WoolenClothGloves()
        {

            return new Item.Item
            {
                name = "Woolen Cloth Gloves",
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Hands,
                description = new Description()
                {
                    look = "This is a simple woolen gloves which provides basic protection and warmth.",
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Hands,
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