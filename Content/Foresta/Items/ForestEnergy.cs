using Crystals.Helpers;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items
{
    public class ForestEnergy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Forest Energy");
            // Tooltip.SetDefault("Its Ancient and Filled with Energy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Green;
            Item.material = true;
            Item.value = ValueHelper.GetCoinValue(0 ,0 , 1 , 0);
        }
    }
}