using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace Crystals.Core.Systems.ParticleSystemAttempt
{
    public partial class Particle
    {
        public static void NewSparkleParticle(Vector2 position, int ownerIndex, float duration = 100, Vector2? velocity = null, float opacity = 1, Vector2? scale = null, Color? color = null, float rotation = 0, float angularVelocity = 0, Vector2? fatness = null)
        {
            scale ??= Vector2.One;
            fatness ??= Vector2.One;
            Vector2 scaleX = new Vector2(fatness.Value.X * 0.5f, scale.Value.X);
            Vector2 scaleY = new Vector2(fatness.Value.Y * 0.5f, scale.Value.Y);
            Particle particle = GetFreeParticleAndSetValues(position, ownerIndex, duration, velocity, opacity, scaleY, color, rotation, angularVelocity, 15);
            particle.drawFunction = particle.SparkleDrawSingleAxis;
            particle.updateFunction = particle.SparkleUpdate;
            particle = GetFreeParticleAndSetValues(position, ownerIndex, duration, velocity, opacity, scaleX, color, rotation + MathF.PI * 0.5f, angularVelocity, 15);
        }
        public static void NewSparkleParticleBothAxes(Vector2 position, int ownerIndex, float duration = 100, Vector2? velocity = null, float opacity = 1, Vector2? scale = null, Color? color = null, float rotation = 0, float angularVelocity = 0)
        {
            scale ??= Vector2.One;
            Particle particle = GetFreeParticleAndSetValues(position, ownerIndex, duration, velocity, opacity, scale, color, rotation, angularVelocity, 15);
            particle.drawFunction = particle.SparkleDrawBothAxes;
            particle.updateFunction = particle.SparkleUpdate;
        }
        public void SparkleDrawSingleAxis()
        {
            Vector2 drawpos = Position - Main.screenPosition;
            Texture2D texture = TextureAssets.Extra[98].Value;
            Color outerColor = Color * Opacity * 0.75f;
            Vector2 origin = texture.Size() / 2f;
            Color innerColor = new Color(200, 200, 200, 0) * Opacity;
            float intensityMultiplier = MathF.Cos(TimeLeft * 0.4f) * 0.25f + 0.75f;
            outerColor *= intensityMultiplier;
            innerColor *= intensityMultiplier;
            Main.EntitySpriteDraw(texture, drawpos, null, outerColor, MathF.PI * 0.5f + Rotation, origin, Scale * intensityMultiplier, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, MathF.PI * 0.5f + Rotation, origin, Scale * intensityMultiplier * 0.6f, SpriteEffects.None, 0);
        }
        public void SparkleDrawBothAxes()
        {
            Vector2 drawpos = Position - Main.screenPosition;
            Texture2D texture = TextureAssets.Extra[98].Value;
            Color outerColor = Color;
            if (!Main.dayTime)
                outerColor.A = 0;
            else outerColor.A = 180;
            outerColor *= Opacity * 0.75f;
            Vector2 origin = texture.Size() / 2f;
            Color innerColor = new Color(200, 200, 200, 0) * Opacity;
            float intensityMultiplier = MathF.Cos(TimeLeft * 0.4f) * 0.25f + 0.75f;
            outerColor *= intensityMultiplier;
            innerColor *= intensityMultiplier;
            Vector2 scaleX = new Vector2(0.5f * Scale.X, Scale.X);
            Vector2 scaleY = new Vector2(0.5f * Scale.Y, Scale.Y);       
            Main.EntitySpriteDraw(texture, drawpos, null, outerColor, MathF.PI * 0.5f + Rotation, origin,scaleX * intensityMultiplier, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, outerColor, Rotation, origin, scaleX * intensityMultiplier, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, Rotation, origin, scaleY * intensityMultiplier * 0.6f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, MathF.PI * 0.5f + Rotation, origin, scaleY * intensityMultiplier * 0.6f, SpriteEffects.None, 0);
        }
        public void SparkleUpdate()
        {
            Velocity *= 0.9f;
        }
    }
    public static class ParticleExtensionMethods
    {
        public static int ToParticleOwnerIndex(this Player player) => player.whoAmI + 1200;
        public static int ToParticleOwnerIndex(this NPC npc) => npc.whoAmI + 1000;
        public static int ToProjectileOwnerIndex(this Projectile projectile) => projectile.whoAmI;
    }
}
