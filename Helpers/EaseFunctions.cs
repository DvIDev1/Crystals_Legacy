
using System;

namespace Crystals.Helpers
{
    public class EaseFunctions
    {
        public static float easeIn(float t)
        {
            return t * t;
        }
        
        public static float easeInOutQuad(float x) {
            return x < 0.5 ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
        }
        
        public static float easeOutBounce(float x) {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (x < 1 / d1) {
                return n1 * x * x;
            } else if (x < 2 / d1) {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            } else if (x < 2.5 / d1) {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            } else {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }
        
        public static double easeInOutBack(double x){
            const double c1 = 1.70158f;
            const double c2 = c1 * 1.525f;

            return x < 0.5
                ? (Math.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2f
                : (Math.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2f;
        }
        
        public static double easeOutBack(double x) {
            const double c1 = 1.70158;
            const double c3 = c1 + 1;

            return 1 + c3 * Math.Pow(x - 1, 3) + c1 * Math.Pow(x - 1, 2);
        }
        
        public static double easeOutElastic(double x) {
            const double c4 = (2 * Math.PI) / 3;

            return x == 0
                ? 0
                : x == 1
                    ? 1
                    : Math.Pow(2, -10 * x) * Math.Sin((x * 10 - 0.75) * c4) + 1;
        }
        
        public static double easeInOutExpo(double x){
            return x == 0
                ? 0
                : x == 1
                    ? 1
                    : x < 0.5 ? Math.Pow(2, 20 * x - 10) / 2
                        : (2 - Math.Pow(2, -20 * x + 10)) / 2;
        }
        
        public static double easeInBack(double x) {
            const double c1 = 1.70158;
            const double c3 = c1 + 1;

            return c3 * x * x * x - c1 * x * x;
        }

        public static float easeInBounce(float x){
            return 1 - easeOutBounce(1 - x);
        }
        
    }
}