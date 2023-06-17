using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items;

public class CrusoliumFragment : ModItem
{
    
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
    }
        
    public override void SetDefaults()
    {
        Item.maxStack = 1;
        Item.width = 26;
        Item.height = 22;
        Item.rare = ItemRarityID.Green;
        Item.material = true;
        Item.value = ValueHelper.GetCoinValue(0 ,0 , 1 , 25);
    }
    
    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
        float rotation, float scale, int whoAmI)
    {
        Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow",
            AssetRequestMode.ImmediateLoad).Value;
        spriteBatch.Draw
        (
            texture,
            new Vector2
            (
                Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
            ),
            new Rectangle(0, 0, texture.Width, texture.Height),
            Color.White,
            rotation,
            texture.Size() * 0.5f,
            scale,
            SpriteEffects.None,
            0f
        );
    }
    
}