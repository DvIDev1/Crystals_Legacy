using System;

namespace Crystals.Helpers
{
    public class MathFunctions
    {
        public static float SineWave(float amplitude , float frequency ,float phase)
        {
            float form = amplitude * (float) Math.Sin(phase / frequency);
            return form;
        }
        
        public static float Flip(float x)
        {
            return 1 - x;
        }
        
    }
}