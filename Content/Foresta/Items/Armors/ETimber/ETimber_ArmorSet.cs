using Crystals.Core;
using Crystals.Helpers;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Armors.ETimber
{
    public class ETimber_ArmorSet : ModPlayer
    {
        public bool hasETimberSet()
        {
            return Player.armor[0].type == ModContent.ItemType<EnchantedTimberHelmet>()
                   && Player.armor[1].type == ModContent.ItemType<EnchantedTimberChestplate>()
                   && Player.armor[2].type == ModContent.ItemType<EnchantedTimberGreaves>();
        }

    }
    [AutoloadEquip(EquipType.Head)]
    public class EnchantedTimberHelmet : ModItem
    {
        public override string Texture => AssetDirectory.EnchantedTimberArmor + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.defense = 3;
            Item.value = ValueHelper.GetCoinValue(0, 0, 47, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {
            // idk
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EnchantedTimberChestplate>() && legs.type == ModContent.ItemType<EnchantedTimberGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            // player.setBonus = "idk";
        }
        public override void AddRecipes()
        {
            Recipe mod = CreateRecipe(1);
            mod.AddIngredient<EnchantedTimber>(15);
            mod.AddTile(TileID.WorkBenches);
            mod.Register();
        }
    }
    [AutoloadEquip(EquipType.Body)]
    public class EnchantedTimberChestplate : ModItem
    {
        public override string Texture => AssetDirectory.EnchantedTimberArmor + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.defense = 5;
            Item.value = ValueHelper.GetCoinValue(0, 0, 25, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {
            // idk
        }
        public override void AddRecipes()
        {
            Recipe mod = CreateRecipe(1);
            mod.AddIngredient<EnchantedTimber>(20);
            mod.AddTile(TileID.WorkBenches);
            mod.Register();
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class EnchantedTimberGreaves : ModItem
    {
        public override string Texture => AssetDirectory.EnchantedTimberArmor + Name;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;
            Item.defense = 2;
            Item.value = ValueHelper.GetCoinValue(0, 0, 10, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {
            // idk
        }
        public override void AddRecipes()
        {
            Recipe mod = CreateRecipe(1);
            mod.AddIngredient<EnchantedTimber>(12);
            mod.AddTile(TileID.WorkBenches);
            mod.Register();
        }
    }

}