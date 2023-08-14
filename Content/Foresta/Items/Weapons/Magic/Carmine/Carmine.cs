using Crystals.Core;
using Crystals.Helpers;
using MasterMasterMode.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Magic.Carmine
{
    internal class Carmine : ModItem
    {
        public override string Texture => AssetDirectory.Magic + Name;

        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Umbrella);
            Item.useAnimation = 600;
            Item.useAnimation = 600;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 75;
            Item.damage = 214;
            Item.Size = new Vector2(22);
            Item.knockBack = KnockbackValue.LowVeryweakknockback;
            Item.noMelee = true;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.Red;
            Item.holdStyle = ItemHoldStyleID.HoldUp;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<RainbowLaser>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.whoAmI == Main.myPlayer && player.ItemAnimationJustStarted)
            {
                Item.SetDefaults(Type);
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            base.HoldStyle(player, heldItemFrame);
            player.itemRotation = 0f;
            player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)(16 * player.direction);
            player.itemLocation.Y = player.position.Y + 22f;
            player.fallStart = (int)(player.position.Y / 16f);
            if (player.gravDir == -1f)
            {
                player.itemRotation = 0f - player.itemRotation;
                player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
                if (player.velocity.Y < -2f && !player.controlDown)
                {
                    player.velocity.Y = -2f;
                }
            }
            else if (player.velocity.Y > 2f && !player.controlDown)
            {
                player.velocity.Y = 2f;
            }
        }
        public override void HoldItemFrame(Player player)
        {
            if (player.ItemTimeIsZero)
            {
                player.bodyFrame.X = 0;
                player.bodyFrame.Y = 112;
            }
        }
        public override void AddRecipes()
        {
            Recipe.Create(Type)
                .AddIngredient(ItemID.FragmentNebula, 8)
                .AddIngredient(ItemID.TragicUmbrella, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
    
    public class HoldingUmbrellaEffect : ModPlayer
    {
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (Player.HeldItem.type == ModContent.ItemType<Carmine>() && Player.ItemTimeIsZero)
            {
                //4707 tragic umbrella
                Texture2D tex = ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Textures/Weapons/Carmine/CarminePassive").Value;
                Vector2 drawPos = Player.RotatedRelativePoint(Player.Center + new Vector2(Player.direction * 10, -10));
                drawPos -= Main.screenPosition;
                Vector2 origin = new Vector2(Player.direction * 4 + 27, 48);//handle of the umbrella
                Main.EntitySpriteDraw(tex, drawPos, null, Lighting.GetColor(GetPlayerArmPosition(Player).ToTileCoordinates()), Player.fullRotation, origin, Player.GetAdjustedItemScale(Player.HeldItem), Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
        }
        public static Vector2 GetPlayerArmPosition(Player player)
        {
            Vector2 vector = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector.X = player.bodyFrame.Width - vector.X;
            }
            if (player.gravDir != 1f)
            {
                vector.Y = player.bodyFrame.Height - vector.Y;
            }
            vector -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            Vector2 pos = player.MountedCenter - new Vector2(20f, 42f) / 2f + vector + Vector2.UnitY * player.gfxOffY;
            if (player.mount.Active && player.mount.Type == 52)
            {
                pos.Y -= player.mount.PlayerOffsetHitbox;
                pos += new Vector2(12 * player.direction, -12f);
            }
            return player.RotatedRelativePoint(pos);
        }

    }
}
