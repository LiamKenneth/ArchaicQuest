using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Head
{
    public static class ClothingHead
    {


        public static Item.Item WoolenClothHelmet()
        {

            return new Item.Item
            {
                name = "Woolen Cloth Helmet",
                armourType = Item.Item.ArmourType.Cloth,
                eqSlot = Item.Item.EqSlot.Head,
                description = new Description()
                {
                    look = "This is a simple woolen helmet which provides basic protection and warmth.",
                },
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Head,
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