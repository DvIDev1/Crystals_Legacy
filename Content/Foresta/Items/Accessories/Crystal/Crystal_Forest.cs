using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Accessories.Crystal
{
    public class Crystal_Forest : ModItem
    {
        
        public override void SetStaticDefaults()
        { 
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 19));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.DefaultToAccessory(36 , 60);
        }
        
    }
}