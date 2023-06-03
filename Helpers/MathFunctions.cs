using System;
using Microsoft.Xna.Framework;

namespace Crystals.Helpers
{
    public class MathFunctions
    {
        public static float SineWave(float amplitude, float frequency, float phase)
        {
            float form = amplitude * (float)Math.Sin(phase / frequency);
            return form;
        }

        public static float Flip(float x)
        {
            return 1 - x;
        }

        public class EaseFunctions
        {
            public static float EaseIn(float t)
            {
                return t * t;
            }

            public static float EaseInOutQuad(float x)
            {
                return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
            }

            public static float EaseInOutQuint(float x)
            {
                return x < 0.5 ? 16 * x * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 5) / 2;
            }

            public static float EaseInOutCirc(float x)
            {
                return x < 0.5
                    ? (1 - (float)Math.Sqrt(1 - (float)Math.Pow(2 * x, 2))) / 2
                    : ((float)Math.Sqrt(1 - (float)Math.Pow(-2 * x + 2, 2)) + 1) / 2;
            }

            public static float EaseOutBounce(float x)
            {
                const float n1 = 7.5625f;
                const float d1 = 2.75f;

                if (x < 1 / d1)
                {
                    return n1 * x * x;
                }
                else if (x < 2 / d1)
                {
                    return n1 * (x -= 1.5f / d1) * x + 0.75f;
                }
                else if (x < 2.5 / d1)
                {
                    return n1 * (x -= 2.25f / d1) * x + 0.9375f;
                }
                else
                {
                    return n1 * (x -= 2.625f / d1) * x + 0.984375f;
                }
            }

            public static float EaseOutQuad(float x)
            {
                return 1f - (1f - x) * (1f - x);
            }

            public static float EaseInOutBack(float x)
            {
                const float c1 = 1.70158f;
                const float c2 = c1 * 1.525f;

                return x < 0.5f
                    ? ((float)Math.Pow(2f * x, 2f) * ((c2 + 1f) * 2f * x - c2)) / 2f
                    : ((float)Math.Pow(2f * x - 2f, 2f) * ((c2 + 1f) * (x * 2f - 2f) + c2) + 2f) / 2f;
            }

            public static float EaseOutBack(float x)
            {
                const float c1 = 1.70158f;
                const float c3 = c1 + 1f;

                return 1 + c3 * (float)Math.Pow(x - 1, 3) + c1 * (float)Math.Pow(x - 1, 2);
            }

            public static float EaseOutElastic(float x)
            {
                const float c4 = (2 * (float)Math.PI) / 3;

                return x == 0f
                    ? 0f
                    : x == 1f
                        ? 1f
                        : (float)Math.Pow(2f, -10f * x) * (float)Math.Sin((x * 10f - 0.75f) * c4) + 1f;
            }

            public static float EaseInOutExpo(float x)
            {
                return x == 0f
                    ? 0f
                    : x == 1f
                        ? 1f
                        : x < 0.5f
                            ? (float)Math.Pow(2f, 20f * x - 10f) / 2f
                            : (2f - (float)Math.Pow(2f, -20f * x + 10f)) / 2f;
            }

            public static float EaseInBack(float x)
            {
                const float c1 = 1.70158f;
                const float c3 = c1 + 1f;

                return c3 * x * x * x - c1 * x * x;
            }

            public static float EaseInBounce(float x)
            {
                return 1 - EaseOutBounce(1 - x);
            }

            public static float EaseOutQuint(float x)
            {
                return 1f - (float)Math.Pow(1f - x, 5f);
            }
        }
    }
}