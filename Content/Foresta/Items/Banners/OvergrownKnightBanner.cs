using Crystals.Content.Other.Misc.Rarity;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Banners
{
    public class OvergrownKnightBanner : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Slime Banner");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 2;
        }
        
        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<GoldRarity>();
            Item.DefaultToPlaceableTile(ModContent.TileType<Misc.Tiles.Banners>() , 4);
        }
    }
}