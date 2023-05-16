using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Armors.Magic.Gaia.Items.PickUps
{
    public class StarNature : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
        }
        
        public override bool OnPickup(Player player)
        {
            PlayerHelper.HealMana(50 , player);
            SoundEngine.PlaySound(SoundID.Grab , player.Center);
            return false;
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange = 4;
        }
        
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale,
            int whoAmI)
        {
            Lighting.AddLight(Item.Center , TorchID.Blue);
            if (Main.rand.NextFloat() < 0.03f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, Item.width, Item.height, DustID.BlueFairy, Main._rand.NextFloat(-2, 2), Main._rand.NextFloat(-2, 2), 0, new Color(255,255,255), 1f)];
            }
        }

    }
}