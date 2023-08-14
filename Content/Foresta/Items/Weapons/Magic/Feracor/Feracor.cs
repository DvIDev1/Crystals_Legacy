using System;
using Crystals.Core;
using Crystals.Core.Systems.SoundSystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Crystals.Content.Foresta.Items.Weapons.Magic.Feracor
{
    public class Feracor : ModItem
    {
        public override string Texture => AssetDirectory.Magic + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Feracor");
            /* Tooltip.SetDefault("An Ancient Staff that uses the Power of Nature to to Charge up."
                               + "\nCharge for 2 seconds to shot out a dual burst of forest energy"
                               + "\nKeep holding to conjure floating forest energy around you"
                               + "\nGain enough charge to shoot out a projectile that does 2x the damage"); */
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(33, 33);
            Item.noMelee = true;
            Item.crit = 20;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 51;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.noUseGraphic = true;
            Item.channel = true;

            Item.knockBack = KnockbackValue.LowAverageknockback;
            Item.mana = 34;
            Item.scale = 1.5f;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundSystem.FeracorShot;
            Item.autoReuse = false;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.None;
            Item.ArmorPenetration = 30;
            Item.shoot = ModContent.ProjectileType<FeracorHeld>();
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.value = Item.buyPrice(0, 25, 0, 0);
            Item.staff[Item.type] = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Request<Texture2D>("Crystals/Content/Foresta/Items/Weapons/Magic/Feracor/Feracor_Glow",
                AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        private static int charge;

        public int MaxCharge = 600;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
            Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            for (int i = 0; i < 80; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Center, DustID.TerraBlade, speed * 4, Scale: 1.3f);
                ;
                d.noGravity = true;
            }

            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.23f)
            {
                for (int i = 0; i < charge / (MaxCharge / 4); i++)
                {
                    Vector2 pos = Item.Center + Vector2.One.RotatedBy((MathHelper.TwoPi / 4 * i) + 90f) *
                        (Item.width + Item.height) / 2;
                    Dust dust;
                    Vector2 position = pos;
                    dust = Dust.NewDustPerfect(pos, 107);
                }
            }

            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
            ref int damage,
            ref float knockback)
        {
            if (charge >= MaxCharge)
            {
                damage *= 2;
                knockback *= 2;
                type = ModContent.ProjectileType<BigEnergyBlast>();
                charge -= MaxCharge;
            }
            else if (charge >= MaxCharge / 4)
            {
                charge -= MaxCharge / 4;
            }
        }

        public override bool CanUseItem(Player player)
        {
            return charge >= MaxCharge / 4;
        }

        public override void HoldItem(Player player)
        {
            if (Main.rand.NextFloat() < 0.23f)
            {
                for (int i = 0; i < charge / (MaxCharge / 4); i++)
                {
                    Vector2 pos = player.Center + Vector2.One.RotatedBy((MathHelper.TwoPi / 4 * i) + 90f) *
                        (player.width + player.height);
                    Dust dust;
                    Vector2 position = pos;
                    dust = Dust.NewDustPerfect(pos, 107);
                    dust.shader = GameShaders.Armor.GetSecondaryShader(18, player);
                }
            }

            if (charge <= MaxCharge)
            {
                charge++;
            }
        }

        public class EnergyBlast : ModProjectile
        {
            public override string Texture => AssetDirectory.Magic + Name;

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Energy Blast");
                Main.projFrames[Projectile.type] = 4;
            }

            public override void SetDefaults()
            {
                Projectile.width = 14;
                Projectile.height = 9;
                Projectile.scale = 1.5f;
                Projectile.friendly = true;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (hit.Crit)
                {
                    charge += 100;
                }
            }

            public override void AI()
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Dust dust;
                Vector2 position = Projectile.position;
                dust = Main.dust[
                    Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 107, 0f, 0f, 0,
                        new Color(255, 255, 255), 1f)];
                if (++Projectile.frameCounter >= 6)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                }
            }

            public override void Kill(int timeLeft)
            {
                Vector2 position = Projectile.position;
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, 107,
                        Main.rand.NextVector2Circular(5 , 5).X * 2, 
                        Main.rand.NextVector2Circular(5 , 5).Y * 2, 0,
                        new Color(255, 255, 255), 1f);
                }
            }
        }

        public class BigEnergyBlast : ModProjectile
        {
            public override string Texture => AssetDirectory.Magic + Name;

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Big Energy Blast");
                Main.projFrames[Projectile.type] = 6;
            }

            public override void SetDefaults()
            {
                Projectile.width = 38;
                Projectile.height = 25;
                Projectile.friendly = true;
                Projectile.ignoreWater = true;
                Projectile.penetrate = 1;
                Projectile.tileCollide = true;
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (!target.active)
                {
                    Projectile.penetrate++;
                    return;
                }

                Player owner = Main.player[Projectile.owner];
                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos = owner.Center + Vector2.One.RotatedBy((MathHelper.TwoPi / 4 * i) + 90f) *
                        (owner.width + owner.height);
                    pos.Y -= owner.height;
                    Terraria.Projectile proj = Terraria.Projectile.NewProjectileDirect(owner.GetSource_OnHit(target),
                        pos, Projectile.oldVelocity,
                        ModContent.ProjectileType<EnergyBlast>(), Projectile.damage / 2, Projectile.knockBack / 2,
                        owner.whoAmI);
                    proj.velocity = proj.DirectionTo(target.Center) * 20f;
                    proj.CritChance = 25;
                }
            }

            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                Player owner = Main.player[Projectile.owner];
                for (int i = 0; i < 4; i++)
                {
                    Vector2 pos = owner.Center + Vector2.One.RotatedBy((MathHelper.TwoPi / 4 * i) + 90f) *
                        (owner.width + owner.height);
                    pos.Y -= owner.height;
                    Terraria.Projectile proj = Terraria.Projectile.NewProjectileDirect(Projectile.GetSource_None(), pos,
                        Projectile.oldVelocity,
                        ModContent.ProjectileType<EnergyBlast>(), Projectile.damage / 2, Projectile.knockBack / 2,
                        owner.whoAmI);
                    proj.velocity = proj.DirectionTo(Projectile.Center) * 20f;


                    for (int t = 0; t < 60; t++)
                    {
                        Vector2 speed = Main.rand.NextVector2Square(0.5f, 0.5f);
                        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.TerraBlade, speed * 5, Scale: 1f);
                        ;
                        d.noGravity = true;
                    }
                }

                return true;
            }
            
            public override void Kill(int timeLeft)
            {
                Vector2 position = Projectile.position;
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, 107,
                        Main.rand.NextVector2Circular(10 , 10).X *3, 
                        Main.rand.NextVector2Circular(10 , 10).Y * 3, 0,
                        new Color(255, 255, 255), 1f);
                }
            }

            public override void AI()
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                Dust dust;
                Vector2 position = Projectile.position;
                Player owner = Main.player[Projectile.owner];
                dust = Main.dust[
                    Terraria.Dust.NewDust(position, Projectile.width, Projectile.height, 107, 0f, 0f, 0,
                        new Color(255, 255, 255), 1f)];
                if (++Projectile.frameCounter >= 8)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                }

                if (Projectile.Distance(owner.Center) >= 1000)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 pos = owner.Center + Vector2.One.RotatedBy((MathHelper.TwoPi / 10 * i) + 90f) * 1000;
                        Dust.NewDustPerfect(pos, 107);
                        Projectile.Kill();
                    }
                }
            }
        }
    }
}