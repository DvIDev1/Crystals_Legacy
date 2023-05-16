using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items
{
    public class ForestShard : ModItem
    {
     
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Forest Shard");
            // Tooltip.SetDefault("Its Ancient and Filled with Energy");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.Green;
            Item.material = true;
            Item.value = 1700;
        }
        
    }
}