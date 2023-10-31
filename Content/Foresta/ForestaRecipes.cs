using System;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Armors.Crusolium;
using Crystals.Content.Foresta.Items.Armors.Magic.Gaia;
using Crystals.Content.Foresta.Items.Weapons.Magic.Feracor;
using Crystals.Content.Foresta.Items.Weapons.Magic.Incaen;
using Crystals.Content.Foresta.Items.Weapons.Magic.Photosynthesia;
using Crystals.Content.Foresta.Items.Weapons.Ranged.Fall;
using Crystals.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta
{
    public class ForestaRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            //Incaen Recipe
            Recipe incaen = Recipe.Create(ModContent.ItemType<Incaen>()).AddTile(TileID.LivingLoom)
                .AddIngredient<ForestShard>()
                .AddIngredient<Leaf>(20)
                .AddIngredient<ForestGel>(5)
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Book)
                .AddIngredient(ItemID.WandofSparking)
                .Register();

            //Photosynthesia Recipe
            //Todo Change Witch Drop Recipes 
            Recipe Photosynthesia = Recipe.Create(ModContent.ItemType<Photosynthesia>())
                .AddTile(TileID.LivingLoom)
                .AddIngredient<ForestEnergy>(15)
                .AddIngredient(ItemID.Book)
                .AddIngredient<Leaf>(10)
                .AddIngredient<ForestGel>(5)
                .Register();

            //Living Loom Recipe
            LocalizedText SaladWarning =
                Language.GetOrRegister(Crystals.Instance, "RecipeConditions.InForest", () => "InForest");
            // SaladWarning.SetDefault("Player Needs to be in a Forest");
            
            //new Condition( NetworkText.FromKey("Mods.Crystals.RecipeConditions.InForest") , (Predicate<Recipe>) (_ =>  Main.LocalPlayer.ZoneForest));
            Recipe LivingLoom = Recipe.Create(ItemID.LivingLoom)
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ItemID.Wood, 35)
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient<Leaf>(15)
                .AddCondition(ConditionHelper.InForest)
                .Register();

            //Fall Recipe
            Recipe Fall = Recipe.Create(ModContent.ItemType<Fall>())
                .AddTile(TileID.LivingLoom)
                .AddIngredient<Leaf>(20)
                .AddIngredient(ItemID.Cobweb, 5)
                .AddIngredient(ItemID.WoodenBow)
                .AddIngredient<ForestGel>(5)
                .AddIngredient<ForestEnergy>(5)
                .Register();

            //Feracor
            Recipe Feracor = Recipe.Create(ModContent.ItemType<Feracor>())
                .AddTile(TileID.LivingLoom)
                .AddIngredient(ItemID.Wood, 20)
                .AddIngredient<ForestShard>(4)
                .AddIngredient<ForestGel>(10)
                .AddIngredient<Leaf>(5)
                .AddIngredient<ForestEnergy>(12)
                .Register();

            //Gaia Hat
            Recipe GaiaHat = Recipe.Create(ModContent.ItemType<Gaia_ArmorSet.Gaia_Hat>())
                .AddIngredient<Leaf>(30)
                .AddIngredient<ForestEnergy>(20)
                .AddIngredient<ForestGel>(5)
                .AddIngredient(ItemID.WoodHelmet)
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.LivingLoom)
                .Register();

            //Gaia Robe
            Recipe Robe = Recipe.Create(ModContent.ItemType<Gaia_ArmorSet.Gaia_Hat>())
                .AddIngredient<Leaf>(40)
                .AddIngredient<ForestEnergy>(25)
                .AddIngredient<ForestGel>(10)
                .AddIngredient(ItemID.WoodBreastplate)
                .AddIngredient(ItemID.Silk, 15)
                .AddTile(TileID.LivingLoom)
                .Register();

            //Gaia Skirt
            Recipe Skirt = Recipe.Create(ModContent.ItemType<Gaia_ArmorSet.Gaia_Hat>())
                .AddIngredient<Leaf>(25)
                .AddIngredient<ForestEnergy>(15)
                .AddIngredient<ForestGel>(5)
                .AddIngredient(ItemID.WoodGreaves)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.LivingLoom)
                .Register();

            //Crusolium Helmet
            Recipe CrusoHelmet = Recipe.Create(ModContent.ItemType<Cruso_ArmorSet.CrusoHelmet>())
                .AddIngredient<BrokenCrusoSet.BrokenHelmet>(1)
                .AddIngredient<ForestGel>(5)
                .AddIngredient<CrusoliumFragment>(2)
                .AddIngredient<ForestEnergy>(15)
                .AddTile(TileID.Anvils)
                .Register();

            //Crusolium Chestplate
            Recipe CrusoPlate = Recipe.Create(ModContent.ItemType<Cruso_ArmorSet.CrusoChestplate>())
                .AddIngredient<BrokenCrusoSet.BrokenChest>(1)
                .AddIngredient<ForestGel>(5)
                .AddIngredient<CrusoliumFragment>(3)
                .AddIngredient<ForestEnergy>(20)
                .AddTile(TileID.Anvils)
                .Register();

            //Crusolium Greaves
            Recipe CrusoGreaves = Recipe.Create(ModContent.ItemType<Cruso_ArmorSet.CrusoGreaves>())
                .AddIngredient<BrokenCrusoSet.BrokenBoots>(1)
                .AddIngredient<ForestGel>(4)
                .AddIngredient<CrusoliumFragment>(2)
                .AddIngredient<ForestEnergy>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}