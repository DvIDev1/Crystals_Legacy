using Crystals.Core;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items
{
    public class ForestGel : ModItem
    {
        public override string Texture => AssetDirectory.Items + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Forest Gel");
            // Tooltip.SetDefault("Its Gross");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 28;
            Item.height = 30;
            Item.rare = ItemRarityID.Green;
            Item.material = true;
            Item.value = 50;
        }
    }
}