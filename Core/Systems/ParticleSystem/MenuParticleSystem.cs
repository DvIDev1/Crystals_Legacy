using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Crystals.Core.Systems.ParticleSystemAttempt;
using Terraria.Graphics.Renderers;

namespace Crystals.Core.Systems.ParticleSystem
{
    public class MenuParticleSystem
    {
        public delegate void Update(MenuParticle particle);
        private readonly List<MenuParticle> particles = new();
        private Texture2D Tex;
        private readonly Update UpdatePaticles;

        public MenuParticleSystem(string Texture, Update update)
        {
            Tex = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;


            UpdatePaticles = update;
        }



        public void Draw(SpriteBatch sprite)
        {
            
            for (int k = 0; k < particles.Count; k++)
            {
                MenuParticle particle = particles[k];
                particle.TimeLeft--;
                if (particle is null)
                    continue;

                if (!Main.gameInactive)
                {
                    UpdatePaticles(particle);
                }

                

                sprite.Draw(Tex, particle.Position, particle.Frame == new Rectangle() ? Tex.Bounds : particle.Frame, particle.Color * particle.Alpha, particle.Angle, particle.Frame.Size() / 2, particle.Size, 0, 0);

            }
            particles.RemoveAll(n => n is null || n.TimeLeft <= 0);

        }

        public void GenerateParticle(MenuParticle particle)
        {
            if (!Main.gameInactive)
            {
                particles.Add(particle);
            }
        }
        public void ClearParticles()
        {
            particles.Clear();
        }

    }

    public class MenuParticle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Rectangle Frame;
        public float Angle;
        public Color Color;
        public float Size;
        public int TimeLeft;
        public bool Activate = false;
        public int MaxParticles;
        internal float Alpha;


        public MenuParticle(Vector2 position, Vector2 velocity, float angle, Color color, float size, int timeLeft, bool activate, int maxParticles, float alpha = 1, Rectangle frame = new Rectangle())
        {
            Position = position;
            Velocity = velocity;
            Angle = angle;
            Color = color;
            Size = size;
            TimeLeft = timeLeft;
            Activate = activate;
            MaxParticles = maxParticles;
            Frame = frame;
            Alpha = alpha;
            
        }


    }
}