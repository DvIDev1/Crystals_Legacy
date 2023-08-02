using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Crystals.Common.UI;

namespace Crystals.Common.Prefixes
{
    public class Hexed : ModPrefix
    {

        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override float RollChance(Item item)
        {
            return 5f;
        }

        public override bool CanRoll(Item item)
        {
            return true;
        }


        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult -= 0.35f;
            critBonus += 30;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f - 1f;
        }


    }
}