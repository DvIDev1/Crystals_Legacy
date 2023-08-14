using Crystals.Core;
using Crystals.Helpers;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Tools.EnchantedTimberItems
{
    public class EnchantedTimberAxe : ModItem
    {

        public override string Texture => AssetDirectory.Tools + Name;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = ValueHelper.GetCoinValue(0, 0, 0, 25);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true; // Automatically re-swing/re-use this item after its swinging animation is over.

            Item.axe = 12; // How much axe power the weapon has, note that the axe power displayed in-game is this value multiplied by 5
        }
    }
}