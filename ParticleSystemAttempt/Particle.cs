using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals
{
    public partial class Particle
    {
        public delegate void UpdateFunction();
        public delegate void DrawFunction();
        public UpdateFunction updateFunction;
        public DrawFunction drawFunction;
        public Particle()
        {
            TimeLeft = 0;
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Opacity = 0;
            Scale = Vector2.One;
            Color = Color.White;
            Rotation = 0;
            AngularVelocity = 0;
            OwnerIndex = 0;
        }
        public static Particle GetFreeParticleAndSetValues(Vector2 position, int ownerIndex, float duration = 100, Vector2? velocity = null, float opacity = 1, Vector2? scale = null, Color? color = null, float rotation = 0, float angularVelocity = 0, float fadeOutStart = 15)
        {
            int index = 1000;
            for (int i = 0; i < ParticleSystem.MaxParticles; i++)
            {
                if (ParticleSystem.particles[i].Active)
                    continue;
                index = i;
                break;
            }
            Particle particle = ParticleSystem.particles[index];
            particle.WhoAmI = index;
            velocity ??= Vector2.Zero;
            color ??= Color.White;
            scale ??= Vector2.One;
            particle.TimeLeft = duration;
            particle.Position = position;
            particle.Velocity = velocity.Value;
            particle.Opacity = opacity;
            particle.Scale = scale.Value;
            particle.Color = color.Value;
            particle.Rotation = rotation;
            particle.AngularVelocity = angularVelocity;
            particle.OwnerIndex = (short)ownerIndex;
            particle.FadeOutStart = fadeOutStart;
            return particle;    
        }
        /// <summary>
        /// from 0-1456.
        /// if it's a number between0 and 999, it's a projectile, if it's a number between 1000 and 1200, it's an npc, and if it's above that, it's a player.
        /// ONLY SET THIS WHEN SPAWNING THE PARTICLE!!! USE THE EXTENSION METHODS!!! USE THE GETTERS FOR THE NPC, PLAYER AND PROJECTILE OWNER INSTANCES.
        ///If you are only using the position, size and velocity data, it's advised to use entity instead as that will make functionality implemented with it  compatible with both players, npcs and projectiles
        /// </summary>
        public short OwnerIndex { get; set; }
        public Player PlayerOwner { get => (OwnerIndex >= 1200) ? Main.player[OwnerIndex - 1200] : Main.player[255]; }
        public NPC NPCOwner { get => (OwnerIndex >= 1000 && OwnerIndex < 1200) ? Main.npc[OwnerIndex - 1000] : Main.npc[200]; }
        public Projectile ProjectileOwner { get => (OwnerIndex < 1000 && OwnerIndex >= 0) ? Main.projectile[OwnerIndex] : Main.projectile[1000]; }
        public Entity OwnerEntity { get => OwnerIndex < 1000 ? ProjectileOwner : OwnerIndex < 1200 ? NPCOwner : PlayerOwner; }
        public int WhoAmI { get; set; }
        public float FadeOutStart { get; set; }
        public bool Active { get => TimeLeft > 0; set => TimeLeft = value ? TimeLeft : 0; }
        public float TimeLeft { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Friction { get; set; }
        public float AngularVelocity { get; set; }
        public float Rotation { get; set; }
        public float Opacity { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; }

        public void Draw()
        {
            if (drawFunction != null)
                drawFunction.Invoke();
        }
     
        public void Update()
        {
            UpdatePhysics();
            if (updateFunction != null)
            updateFunction.Invoke();
        }

        public void UpdatePhysics()
        {
            TimeLeft--;
            Position += Velocity;
            Rotation += AngularVelocity;
            if (TimeLeft <= FadeOutStart)
                Opacity -= 1f / FadeOutStart;
        }
    }
}