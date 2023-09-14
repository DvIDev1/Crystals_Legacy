using Crystals.Core;
using Crystals.Helpers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Tools.EnchantedTimberItems
{
    public class EnchantedTimberPickaxe : ModItem
    {

        public override string Texture => AssetDirectory.Tools + Name;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 32;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = ValueHelper.GetCoinValue(0, 0, 0, 25);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true; // Automatically re-swing/re-use this item after its swinging animation is over.
            Item.pick = 50;
        }
        public override void AddRecipes()
        {
            Recipe mod = CreateRecipe(1);
            mod.AddIngredient<EnchantedTimber>(10);
            mod.AddTile(TileID.WorkBenches);
            mod.Register();
        }
    }
}