using Crystals.Core;
using Crystals.Helpers;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items
{
    public class EnchantedTimber : ModItem
    {
        public override string Texture => AssetDirectory.Items + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Purple;
            Item.material = true;
        }
    }
}