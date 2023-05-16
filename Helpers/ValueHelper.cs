namespace Crystals.Helpers
{
    public class ValueHelper
    {

        public static int GetCoinValue(int platinum , int gold , int silver , int copper)
        {
            return (platinum * 400000) + (gold * 4000) + (silver * 40) + (int)(copper * 0.4f);
        }
        
    }
}