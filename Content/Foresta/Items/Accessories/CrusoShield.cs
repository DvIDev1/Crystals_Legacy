using System.Drawing;
using Crystals.Core;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Crystals.Content.Foresta.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class CrusoShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crusolium Shield");
            // Tooltip.SetDefault("Increased life regen by 2 \nIncreased Damage Reduction by 5%");
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.defense = 5;
            Item.value = ValueHelper.GetCoinValue(0, 4, 18, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.05f;
            player.lifeRegen += 2;
        }



    }
}