using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace Crystals.Core.Systems.ParticleSystemAttempt
{
    public partial class Particle
    {
        public static void NewBlurParticle(Vector2 position, int ownerIndex, float duration = 100, Vector2? velocity = null, float opacity = 1, Vector2? scale = null, Color? color = null, float rotation = 0, float angularVelocity = 0)
        {
            scale ??= Vector2.One;
            Particle particle = GetFreeParticleAndSetValues(position, ownerIndex, duration, velocity, opacity, scale, color, rotation, angularVelocity, 15);
            particle.drawFunction = particle.DrawBlur;
            particle.updateFunction = particle.UpdateBlur;
        }
        public void UpdateBlur()
        {
            Velocity *= 0.8f;
            if (TimeLeft <= FadeOutStart)
                Scale -= Vector2.Clamp(new Vector2(1f / FadeOutStart), Vector2.Zero, new Vector2(1000000000));
        }
        /// <summary>
        /// UNTESTED, MIGHT LOOK BAD WITH 0 ALPHA
        /// </summary>
        public void DrawBlur()
        {
            Vector2 drawpos = Position - Main.screenPosition;
            Texture2D texture = TextureAssets.Extra[59].Value;
            Color drawColor = Color * Opacity;
            drawColor.A = 0;
            Main.EntitySpriteDraw(texture, drawpos, null, drawColor, MathF.PI * 0.5f + Rotation, texture.Size() / 2, Scale, SpriteEffects.None, 0);
        }
    }
}
