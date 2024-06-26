using System;
using Crystals.Core;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Ranged.Fall
{
    public class Fall : ModItem
    {
        public override string Texture => AssetDirectory.Ranged + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fall");
            // Tooltip.SetDefault("An Bow only used by the most skilled Huntresses");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(14, 40);
            Item.noMelee = true;
            Item.crit = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 29 / projAmount; //Gets divided By projAmount so be careful
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = KnockbackValue.LowExtremelyweakknockback;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.DD2_BookStaffCast;
            Item.autoReuse = true;

            Item.shoot = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.buyPrice(0, 1, 75, 0);
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 14.0f;
        }

        public int spreadMax = 16; //Maximal Projectile Spread
        public int spreadMin = -15; //Minimum Projectile Spread
        public int projAmount = 3; //The Amount of Projectiles Shot 

        public int ArrowUse = 10; //The amount of uses you need for the Neon Arrow to be shot

        public int uses;


        public override bool RangedPrefix()
        {
            return true;
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            uses++;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (uses == ArrowUse)
            {
                return false;
            }

            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
            ref int damage,
            ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<LeafArrow>();
            }

            if (uses == ArrowUse)
            {
                type = ModContent.ProjectileType<NeonArrow>();
                knockback *= 2;
                damage *= 2;
                uses = 0;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
            Vector2 velocity, int type,
            int damage, float knockback)
        { // shoots *(Inserted Value)* of projectiles
            for (int index = 0; index < projAmount; ++index)
            {
                Vector2 vector2_1 = new Vector2(
                    (float) ((double) player.position.X + (double) player.width * 0.5 +
                             (double) (Main.rand.Next(201) * -player.direction) + ((double) Main.mouseX +
                                 (double) Main.screenPosition.X - (double) player.position.X)),
                    (float) ((double) player.position.Y + (double) player.height * 0.5 -
                             600.0)); //this defines the projectile width, direction and position
                vector2_1.X = (float) (((double) vector2_1.X + (double) player.Center.X) / 2.0) +
                              (float) Main.rand.Next(-200, 201);
                vector2_1.Y -= (float) (100 * index);
                float num12 = (float) Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float) Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double) num13 < 0.0) num13 *= -1f;
                if ((double) num13 < 20.0) num13 = 20f;
                float num14 = (float) Math.Sqrt((double) num12 * (double) num12 + (double) num13 * (double) num13);
                float num15 = Item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float) Main.rand.Next(spreadMin, spreadMax) * 0.02f; //Projectile Spread
                float SpeedY = num17 + (float) Main.rand.Next(-40, 41) * 0.02f;
                Projectile.NewProjectile(Item.GetSource_None(), vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage,
                    knockback, Main.myPlayer, 0.0f, (float) Main.rand.Next(5));
            }

            return false;
        }

        class NeonArrow : ModProjectile
        {
            public override string Texture => AssetDirectory.Ranged + Name;

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Neon Arrow");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
            }

            public override void SetDefaults()
            {
                Projectile.width = 14;
                Projectile.height = 40;
                Projectile.friendly = true;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;
                Projectile.arrow = true;
                Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
                Projectile.aiStyle = ProjAIStyleID.Arrow;
                Projectile.penetrate = -1;
            }

            public int startVelo = 2; //Velocity Multiplier on Spawn !!Makes The Arrows Inaccurate!!
            public int BuffTime = 5; //The Time the Debuff will be applied for (InSeconds) !!Is Halved!!

            public override void OnSpawn(IEntitySource source)
            {
                Projectile.velocity *= 2;
            }

            public override void AI()
            {
                Dust.NewDustPerfect(Projectile.Center, 61);
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(BuffID.Venom, BuffTime * 30);
            }

            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

                // Redraw the projectile with the color not influenced by light
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin +
                                      new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) *
                                  ((Projectile.oldPos.Length - k) / (float) Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin,
                        Projectile.scale, SpriteEffects.None, 0);
                }

                return true;
            }

            public override void Kill(int timeLeft)
            {
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width,
                    Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
        }

        class LeafArrow : ModProjectile
        {
            public override string Texture => AssetDirectory.Ranged + Name;

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Leaf Arrow");
            }

            public override void SetDefaults()
            {
                Projectile.width = 14;
                Projectile.height = 40;
                Projectile.friendly = true;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;
                Projectile.arrow = true;
                Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
                Projectile.aiStyle = ProjAIStyleID.Arrow;
                Projectile.penetrate = 1;
            }

            public int BuffTime = 5; //The Time the Debuff will be applied for (InSeconds) 

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(BuffID.Poisoned, BuffTime * 60);
            }

            public override void Kill(int timeLeft)
            {
                // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
                Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width,
                    Projectile.height);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
        }
    }
}