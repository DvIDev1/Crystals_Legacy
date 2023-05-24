using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace Crystals.Core.Systems.ParticleSystem
{
    internal class TelegraphEffects
    {
        /// <summary>
        /// draws a cross shaped star, used in many vanilla projectiles. 
        /// This is literally a clone of the vanilla method :thumb_up:
        /// </summary>
        /// <param name="drawColor">the inside color. Most of the time this should be white</param>
        /// <param name="shineColor">the outside color</param>
        /// <param name="flareCounter"> a timer. Use MathF.Sin or MathF.Cos </param>
        /// <param name="fadeInStart">honestly these params area bit confusing</param>
        /// <param name="fadeInEnd"></param>
        /// <param name="fadeOutStart"></param>
        /// <param name="fadeOutEnd"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="fatness">idk how to explain this one but you can stretch the effect using this</param>
        public static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D texture = TextureAssets.Extra[98].Value;
            Color color = shineColor * opacity * 0.5f;
            Vector2 origin = texture.Size() / 2f;
            Color innerColor = drawColor * 0.5f;
            float intensityMultiplier = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, true);
            Vector2 scaleX = new Vector2(fatness.X * 0.5f, scale.X) * intensityMultiplier;
            Vector2 scaleY = new Vector2(fatness.Y * 0.5f, scale.Y) * intensityMultiplier;
            color *= intensityMultiplier;
            innerColor *= intensityMultiplier;
            Main.EntitySpriteDraw(texture, drawpos, null, color, MathF.PI / 2f + rotation, origin, scaleX, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, color, 0f + rotation, origin, scaleY, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, MathF.PI / 2f + rotation, origin, scaleX * 0.6f, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, 0f + rotation, origin, scaleY * 0.6f, dir, 0);
        }
        /// <summary>
        /// draws a convering ring. A ring that gets smaller as the timer approaches the end
        /// </summary>
        public static void DrawTelegraphRing(float timerVar, float start, float end, float startScale, Vector2 ringCenter, Color ringColor, byte ringAlpha = 0)
        {
            float duration = end - start;
            float progress = (timerVar - start) / duration;
            Main.instance.LoadProjectile(ProjectileID.PrincessWeapon);
            Texture2D ringTexture = TextureAssets.Projectile[ProjectileID.PrincessWeapon].Value;
            ringColor.A = ringAlpha;
            float colorMult = 1 - MathF.Pow(1 - progress, 2);
            colorMult *= 0.125f;
            for (float i = 0; i < 8; i++)
            {
                Main.EntitySpriteDraw(ringTexture, ringCenter - Main.screenPosition, null, ringColor * colorMult, i / 8f * MathHelper.PiOver2, ringTexture.Size() / 2, (1 - progress) * startScale, SpriteEffects.None, 0);
            }
        }
        /// <summary>
        /// TEST THIS LATER NOT SURE IF IT WORKS
        /// </summary>
        /// <param name="timerVar">variable you're using as a timer</param>
        /// <param name="start">when it starts on your timer</param>
        /// <param name="end">when it ends on your timer</param>
        /// <param name="lineWidth">width of the line</param>
        /// <param name="lineLength"> length of the line in pixels</param>
        /// <param name="lineColor">the color of the line</param>
        /// <param name="lineStartPos"> where the line starts</param>
        /// <param name="lineRotation"> rotation of the line</param>
        public static void DrawTelegraphLine(float timerVar, float end, float lineWidth, float lineLength, Color lineColor, Vector2 lineStartPos, float lineRotation, float start = 0)
        {
            //0,001953125
            Texture2D line = TextureAssets.Extra[ExtrasID.FairyQueenLance].Value;
            float duration = end - start;
            float progress = (timerVar - start) / duration;
            lineWidth = (1 - progress) * lineWidth * 0.001953125f;//1/512 the texture is 512 pixels wide
            lineColor.A = 0;
            Main.EntitySpriteDraw(line, lineStartPos - Main.screenPosition, null, lineColor, lineRotation, new Vector2(0, line.Height * 0.5f), new Vector2(lineLength, lineWidth), SpriteEffects.None);
        }
    }
}
