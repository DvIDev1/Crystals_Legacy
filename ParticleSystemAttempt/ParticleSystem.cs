using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using ReLogic.Content;

namespace Crystals
{
	public class ParticleSystem : ModSystem
    {
        public const int MaxParticles = 1000;
        /// <summary>
        /// +1 so that I can use it as a dummy slot if every slot is full even though probably it will never be maybe
        /// </summary>
        public static Particle[] particles = new Particle[MaxParticles + 1];
        public override void Load()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new();
            }
        }
        public override void PostUpdateProjectiles()  
        {
            for (int i = 0; i < MaxParticles; i++)
            {
                if(particles[i].Active)
                    particles[i].Update();
            }
            particles[MaxParticles].Active = false;
        }
        public override void PostDrawTiles(){

            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            for (int i = 0; i < MaxParticles; i++)
            {
                if (particles[i].Active)
                    particles[i].Draw();
            }
            Main.spriteBatch.End();
        }

        public override void Unload()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = null;
            }
        }
    }
}
