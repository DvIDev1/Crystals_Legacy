using Crystals.Content.Other.Misc.Rarity;
using Crystals.Core;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items
{
    public class GoldenLeaf : ModItem
    {
        public override string Texture => AssetDirectory.Items + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Golden Leaf");
            // Tooltip.SetDefault("An Main Component");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 28;
            Item.height = 24;
            Item.rare = ModContent.RarityType<GoldRarity>();
            Item.material = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.buyPrice(0, 6, 20, 50);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.06f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Item.width, Item.height, 269, 0f, -10f, 0, new Color(255,255,255), 1f)];
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0;
        }
    }
}