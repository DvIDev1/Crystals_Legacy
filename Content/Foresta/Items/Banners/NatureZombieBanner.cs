using Crystals.Core;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Banners
{
    public class NatureZombieBanner : ModItem
    {
        public override string Texture => AssetDirectory.Banners + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Slime Banner");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 2;
        }
        
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.DefaultToPlaceableTile(ModContent.TileType<Misc.Tiles.Banners>() , 2);
        }
    }
}