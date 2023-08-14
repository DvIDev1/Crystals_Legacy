using Crystals.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Consumables.Food.Salad
{
    public class Salad : ModItem
    {
        public override string Texture => AssetDirectory.Consumables + Name;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Forest Salad");
        }
        
        public override void SetDefaults()
        {
            Item.DefaultToFood(32 , 24 , BuffID.WellFed , 3600 * 5);
        }
        
    }
}