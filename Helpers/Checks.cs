using System;

namespace Crystals.Helpers
{
    public class Checks
    {
        public static bool inArrayBounds (int index, Array array) 
        {
            return (index >= 0) && (index < array.Length);
        }
    }
}