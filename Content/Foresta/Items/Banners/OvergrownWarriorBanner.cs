using Crystals.Content.Other.Misc.Rarity;
using Crystals.Core;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Banners
{
    public class OvergrownWarriorBanner : ModItem
    {
        public override string Texture => AssetDirectory.Banners + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Slime Banner");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 2;
        }
        
        public override void SetDefaults()
        {
            Item.rare = ModContent.RarityType<GoldRarity>();
            Item.DefaultToPlaceableTile(ModContent.TileType<Misc.Tiles.Banners>() , 3);
        }
    }
}