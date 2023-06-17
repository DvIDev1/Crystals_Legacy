using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items;

public class BrokenCrusoSet
{

    public class BrokenHelmet : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.White;
            Item.material = true;
            Item.value = ValueHelper.GetCoinValue(0 ,0 , 0 , 25);
        }
        
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.06f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[
                    Terraria.Dust.NewDust(position, Item.width, Item.height, DustID.AncientLight, 0f, -2.5f, 0,
                        new Color(255, 255, 255),
                        1f)];
            }

            return true;
        }
        
    }
    
    public class BrokenChest : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 34;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.material = true;
            Item.value = ValueHelper.GetCoinValue(0 ,0 , 0 , 50);
        }
        
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.06f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[
                    Terraria.Dust.NewDust(position, Item.width, Item.height, DustID.AncientLight, 0f, -2.5f, 0,
                        new Color(255, 255, 255),
                        1f)];
            }

            return true;
        }
        
    }
    
    public class BrokenBoots : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 26;
            Item.height = 22;
            Item.rare = ItemRarityID.White;
            Item.material = true;
            Item.value = ValueHelper.GetCoinValue(0 ,0 , 0 , 50);
        }
        
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.06f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[
                    Terraria.Dust.NewDust(position, Item.width, Item.height, DustID.AncientLight, 0f, -2.5f, 0,
                        new Color(255, 255, 255),
                        1f)];
            }

            return true;
        }
        
    }
    
}