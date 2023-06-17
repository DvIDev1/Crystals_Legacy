using Crystals.Helpers;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items;

public class CursedEnergy : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
    }
        
    public override void SetDefaults()
    {
        Item.maxStack = 99;
        Item.width = 26;
        Item.height = 26;
        Item.rare = ItemRarityID.Purple;
        Item.material = true;
        Item.value = ValueHelper.GetCoinValue(0 ,0 , 0 , 0);
    }
}