using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.CameraShake
{
    public class Shake : ModPlayer
    {
        public static bool active;
        public static int time = 10;
        public static int power = 6;
        private float timer;


        public override void ModifyScreenPosition()
        {

            if (active)
            {
                var random = new Vector2(Main.rand.Next(-power, power), Main.rand.Next(-power, power));

                timer++;
                if (timer > 0) Main.screenPosition += random;

                if (timer >= time)
                {
                    timer = 0;
                    active = false;
                }
            }
        }

        public override void ResetEffects()
        {
            if (!active)
            {
                time = 10;
            }
        }
    }
}