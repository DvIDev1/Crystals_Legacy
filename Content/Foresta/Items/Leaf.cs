using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items
{
    public class Leaf : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Essence of The Forest");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 24;
            Item.height = 30;
            Item.rare = ItemRarityID.Green;
            Item.material = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.03f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Item.width, Item.height, 107, Main._rand.NextFloat(-2, 2), Main._rand.NextFloat(-2, 2), 0, new Color(255,255,255), 1f)];
            }
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0;
        }
        
    }
}