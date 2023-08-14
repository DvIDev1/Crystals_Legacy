using Crystals.Core;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Other.Items.Vanity.Maid
{
    public class Maid
    {
        [AutoloadEquip(EquipType.Head)]
        private class MaidHat : ModItem
        {

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Dragon Maid Bonnet");
                // Tooltip.SetDefault("It has Horns");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }

            public override void SetDefaults()
            {
                Item.width = 24;
                Item.height = 24;
                Item.vanity = true;
                Item.rare = ItemRarityID.Red;
            }
        }

        [AutoloadEquip(EquipType.Body)]
        private class MaidDress : ModItem
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Dragon Maid Dress");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }

            public override void SetDefaults()
            {
                Item.width = 30;
                Item.height = 30;
                Item.vanity = true;
                Item.rare = ItemRarityID.Red;
            }
        }

        [AutoloadEquip(EquipType.Legs)]
        private class MaidSkirt : ModItem
        {

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Dragon Maid Skirt");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }

            public override void SetDefaults()
            {
                Item.width = 32;
                Item.height = 32;
                Item.vanity = true;
                Item.rare = ItemRarityID.Red;
            }
        }
    }
}