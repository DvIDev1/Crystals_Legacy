using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Menus
{
    public class ForestaMystica : ModMenu
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Opening");
        
        public override string DisplayName => "Foresta Mystica";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Textures/Menus/CrystalTitle");

        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Textures/Menus/OvergrownMoon");

        protected float time = 0;

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
            logoRotation = 0;
            logoScale = 1f;
            float drawX = (logoDrawCenter.X - 486 / 2f);
            float drawY = (logoDrawCenter.Y - 144 / 2f) + 25;
            drawY += (float) MathFunctions.SineWave(2, 0.5f, time / 30f);
            spriteBatch.Draw(Logo.Value , new Vector2(drawX, drawY) , Color.White);
            return false;
        }

        //public override ModSurfaceBackgroundStyle MenuBackgroundStyle

       class ForestBG : ModSurfaceBackgroundStyle
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