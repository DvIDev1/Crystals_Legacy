using Terraria;
using Terraria.ModLoader;

namespace Crystals.Content.Other.Misc.Prefixes
{
    public class Overwhelming : ModPrefix
    {

        public override PrefixCategory Category => PrefixCategory.AnyWeapon;


        public override float RollChance(Item item)
        {
            return 0;
        }

        public override bool CanRoll(Item item)
        {
            return true;
        }


        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            knockbackMult *= 1f + 0.50f;
            critBonus -= 50;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f - 1f;
        }

        public override void Apply(Item item)
        {
            //
        }


    }
}