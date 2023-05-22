using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals
{
    //public class RingParticle : Particle
    //{
    //    public static int NewRingParticle(Vector2 position, float duration = 100, Vector2? velocity = null, float opacity = 1, float scale = 1, Color? color = null, float rotation = 0, float angularVelocity = 0)
    //    {
    //        int index = FindOpenIndex();
    //        Particle particle = new RingParticle();
    //        return particle.SetDefaults(index, position, duration, velocity, opacity, scale, color, rotation, angularVelocity);
    //    }
    //    public override void Update()
    //    {
    //        base.Update();
    //    }
    //    public override void Draw()
    //    {
    //        Main.instance.LoadProjectile(ProjectileID.PrincessWeapon);
    //        float extraScale = MathF.Cos(Main.GlobalTimeWrappedHourly * 10) * 0.25f + 1;
    //        Texture2D ringTexture = TextureAssets.Projectile[ProjectileID.PrincessWeapon].Value;
    //        for (float i = 0; i < 8; i++)
    //        {
    //            Main.EntitySpriteDraw(ringTexture, Position - Main.screenPosition, null, Color * 0.125f * Opacity, i * 0.125f * MathHelper.PiOver2, ringTexture.Size() / 2, Scale + extraScale, SpriteEffects.None, 0);
    //        }
    //    }
    //}
    public partial class Particle
    {
        public static void NewRingParticle(Vector2 position,int ownerIndex, float duration = 40, Vector2? velocity = null, float opacity = 1, Vector2? scale = null, Color? color = null, float rotation = 0, float angularVelocity = 0)
        {
            Particle particle = GetFreeParticleAndSetValues(position, ownerIndex, duration, velocity, opacity, scale, color, rotation, angularVelocity);
            particle.drawFunction = particle.DrawRing;
        }
        public static void NewMotionRingParticle(Vector2 position, int ownerIndex, Vector2 velocity, float duration = 40, float opacity = 1, Color? color = null)
        {       
            Particle particle = GetFreeParticleAndSetValues(position, ownerIndex, duration, velocity, opacity, new Vector2(0.5f,1), color, velocity.ToRotation());
            particle.drawFunction = particle.DrawRing;
            particle.updateFunction = particle.UpdateMotionRing;
        }
        public void UpdateMotionRing()
        {
            Velocity *= 0.8f;
            Scale += (1 - 1 / TimeLeft) *  new Vector2(Velocity.Length() * 0.2f) * Vector2.Normalize(Scale);
        }        
        public void DrawRing()
        {
            Main.instance.LoadProjectile(ProjectileID.PrincessWeapon);
            Texture2D ringTexture = TextureAssets.Projectile[ProjectileID.PrincessWeapon].Value;           
            Main.EntitySpriteDraw(ringTexture, Position - Main.screenPosition, null, Color * Opacity * 0.5f, Rotation, ringTexture.Size() / 2, Scale * 0.1f, SpriteEffects.None, 0);            
        }
    }
    class TestItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PlatinumShortsword;
        public override void SetDefaults()
        {
           
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 10;
            Item.Size = new Vector2(30);
            Item.shoot = ProjectileID.FireArrow;
            Item.shootSpeed = 10;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 10; i++)
            {
                float hue = i / 10f;
                Particle.NewSparkleParticleBothAxes(Main.MouseWorld,player.ToParticleOwnerIndex(), 60, Main.rand.NextVector2CircularEdge(1, 1) * (1 + Main.rand.NextFloat() * 30), 1, new Vector2(3), Main.hslToRgb(hue, 1, 0.65f), Main.rand.NextFloat() * MathF.Tau, Main.rand.NextFloat() * 0.1f - 0.05f);
            }
            return false;
        }
    }
}
