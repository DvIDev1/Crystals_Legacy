using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Crystals.Content.Other.Misc.Rarity
{
    public class GoldRarity : ModRarity
    {
        public override Color RarityColor => new Color(255, 215, 0);

        public override string Name => "Gold";
    }
}