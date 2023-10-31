using Crystals.Core;
using Crystals.Core.Systems.SoundSystem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Ranged.EnchantedTimber;

public class ETimberBow : ModItem
{
    public override string Texture => AssetDirectory.Ranged + Name;

    //Why did you still have this? -Photonic0
    //public static int hits;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Crusolium Bow");
        // Tooltip.SetDefault("Reliable and Efficient");
    }

    public override void SetDefaults()
    {
        Item.channel = true;
        Item.keepTime = 80;
        Item.damage = 12;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 28;
        Item.height = 48;
        Item.maxStack = 1;
        Item.useTime = Item.useAnimation = 40;
        Item.useStyle = 5;
        Item.knockBack = 2;
        Item.rare = ItemRarityID.Green;
        Item.noMelee = true;
        Item.shoot = 1;
        Item.useAmmo = AmmoID.Arrow;
        Item.noUseGraphic = true;
        Item.shootSpeed = 32f;
        Item.autoReuse = false;
        Item.UseSound = SoundSystem.ChargeBow;
        Item.crit = 14;
        Item.value = Item.sellPrice(0, 2, 0, 0);
    }

    public override bool CanUseItem(Player player) =>
        player.ownedProjectileCounts[ModContent.ProjectileType<ETimberBowHeldProj>()] < 9999; //TEST REMOVE 9999 LATER

    public override bool CanShoot(Player player) =>
        player.ownedProjectileCounts[ModContent.ProjectileType<ETimberBowHeldProj>()] <
        1; 

    public override bool RangedPrefix() => true;

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
        ref int damage, ref float knockback)
    {
        type = ModContent.ProjectileType<ETimberBowHeldProj>();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
        int type, int damage, float knockback)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI,
                player.itemTimeMax);
        }

        return false;
    }

    public override bool CanConsumeAmmo(Item ammo, Player player) => false;
}