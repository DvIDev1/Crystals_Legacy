using Crystals.Core.Systems.ParticleSystem;
using Crystals.Core.Systems.ParticleSystemAttempt;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Menus
{
    public class ForestaMystica : ModMenu
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Opening");

        public override string DisplayName => "Foresta Mystica";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Textures/Menus/CrystalTitle");

        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Textures/Menus/OvergrownMoon");

        public MenuParticleSystem Particles;

        protected float time = 0;

        protected int TimeLeft = 600;

        public override void Load()
        {
            Particles = new MenuParticleSystem("Crystals/Assets/Foresta/Dust/MenuDust",ParticleLogic);

        }

        public void ParticleLogic(MenuParticle particle)
        {
            particle.Position += particle.Velocity;
            particle.Angle += 0.005f;
        }


        public override void Unload()
        {
            Particles = null;
        }

        public override void OnSelected()
        {
            time = 0;
        }

        public override void Update(bool isOnTitleScreen)
        {
            base.Update(isOnTitleScreen);

        }

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale,
            ref Color drawColor)
        {

            time++;

            Particles.Draw(spriteBatch);



            Vector2 SpawnPos = logoDrawCenter;


            Vector2 RandVel = new Vector2(Main.rand.NextFloat(-1, 1) + (float)MathFunctions.SineWave(2, 0.5f, time / 30f), Main.rand.NextFloat(-1, 1));

                if (Main.rand.NextBool(80) && RandVel != Vector2.Zero)
                {
                    Particles.GenerateParticle(new MenuParticle(SpawnPos, RandVel, 0, Color.White, 1, 2000,true, 400));
                
                }
            
            


            logoRotation = 0;
            logoScale = 1f;
            float drawX = (logoDrawCenter.X - 486 / 2f);
            float drawY = (logoDrawCenter.Y - 144 / 2f) + 25;
            drawY += (float)MathFunctions.SineWave(2, 0.5f, time / 30f);

                spriteBatch.Draw(Logo.Value, new Vector2(drawX, drawY), Color.White);

            return false;
        }



        //public override ModSurfaceBackgroundStyle MenuBackgroundStyle


        public class ForestBG : ModSurfaceBackgroundStyle
        {
            public override void ModifyFarFades(float[] fades, float transitionSpeed)
            {
                throw new System.NotImplementedException();
            }



            public override int ChooseFarTexture()
            {
                return base.ChooseFarTexture();
            }

            public override int ChooseMiddleTexture()
            {
                return base.ChooseMiddleTexture();
            }

            public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
            {


                return base.ChooseCloseTexture(ref scale, ref parallax, ref a, ref b);
            }
        }

    }
}