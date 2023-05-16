using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.CameraShake
{
    public class ShakeSystem : ModPlayer
    {
        public static bool WeakShake;
        public static bool makeTimerWork;
        public static int MaxTime = 10;
        public static int power = 6;
        private int timer;


        public override void ModifyScreenPosition()
        {
            if (WeakShake) makeTimerWork = true;

            if (makeTimerWork)
            {
                var random = new Vector2(Main.rand.Next(-power, power), Main.rand.Next(-power, power));

                timer++;
                if (timer > 0) Main.screenPosition += random;

                if (timer >= MaxTime)
                {
                    timer = 0;
                    makeTimerWork = false;
                }
            }
        }

        public override void ResetEffects()
        {
            if (!makeTimerWork)
            {
                MaxTime = 10;
                WeakShake = false;
            }
        }
    }
}