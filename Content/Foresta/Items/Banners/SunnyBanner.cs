using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Banners;

public class SunnyBanner : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Nature Slime Banner");
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 2;
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Orange;
        Item.DefaultToPlaceableTile(ModContent.TileType<Misc.Tiles.Banners>(), 8);
    }
}