using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Armors.Magic.Gaia.Items.PickUps
{
    public class NaturaPower : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 18;
        }
        
        public override bool OnPickup(Player player)
        {
            player.AddBuff(ModContent.BuffType<NatureBoost>() , 60*5);
            SoundEngine.PlaySound(SoundID.Grab , player.Center);
            return false;
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange = 8;
        }
        
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale,
            int whoAmI)
        {
            Lighting.AddLight(Item.Center , TorchID.Green);
        }
        
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0;
        }

        public class NatureBoost : ModBuff
        {
            public override void Update(Player player, ref int buffIndex)
            {
                player.GetCritChance(DamageClass.Magic) += 10f;
            }
        }
        
    }
}