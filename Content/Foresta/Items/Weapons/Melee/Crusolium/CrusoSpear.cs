using System.Collections.Generic;
using Crystals.Helpers;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Melee.Crusolium
{
    public class CrusoSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crusolium Spear");
            // Tooltip.SetDefault("A Powerful Spear when fully charged right click for an powerful strike");

            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
            ItemID.Sets.Spears[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.Size = new Vector2(42, 45);
            Item.useAnimation = 24;
            Item.useTime = 36;
            Item.shootSpeed = 0f;
            Item.knockBack = KnockbackValue.LowWeakknockback;
            Item.damage = 23;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Green;
            Item.value = ValueHelper.GetCoinValue(0, 10, 60, 0);

            Item.scale = 2.0f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<CrusoSlash>();
        }

        private static int maxhits = 10;

        private static int hits;


        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool? UseItem(Player player)
        {
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }

            return null;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
            ref int damage,
            ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                knockback = KnockbackValue.HighWeakknockback;
                velocity = new Vector2(0);
                if (player.direction == 1)
                {
                    position = player.Center;
                }
                else
                {
                    position = player.Center - new Vector2(336, 34);
                }

                damage *= 4;
                //type = ModContent.ProjectileType<Slash>();
                hits = 0;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            if (hits == maxhits)
            {
                return true;
            }

            return false;
        }


        class CrusoSpearProj : ModProjectile
        {
            protected virtual float HoldoutRangeMin => 50f;
            protected virtual float HoldoutRangeMax => 120f;

            public override string Texture => "Crystals/Content/Foresta/Items/Weapons/Melee/Crusolium/CrusoSpearProj";

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Spear");
            }

            public override void SetDefaults()
            {
                Projectile.CloneDefaults(ProjectileID.Spear);
                Projectile.scale = 2.0f;
            }

            public override bool PreAI()
            {
                Player player = Main.player[Projectile.owner];
                int duration = player.itemAnimationMax;

                player.heldProj = Projectile.whoAmI;

                if (Projectile.timeLeft > duration)
                {
                    Projectile.timeLeft = duration;
                }

                Projectile.velocity = Vector2.Normalize(Projectile.velocity);

                float halfDuration = duration * 0.5f;
                float progress;

                if (Projectile.timeLeft < halfDuration)
                {
                    progress = Projectile.timeLeft / halfDuration;
                }
                else
                {
                    progress = (duration - Projectile.timeLeft) / halfDuration;
                }

                Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin,
                    Projectile.velocity * HoldoutRangeMax, progress);

                if (Projectile.spriteDirection == -1)
                {
                    Projectile.rotation += MathHelper.ToRadians(45f);
                }
                else
                {
                    Projectile.rotation += MathHelper.ToRadians(135f);
                }

                if (hits == maxhits)
                {
                    Dust.NewDustPerfect(Projectile.Center, 269);
                }

                return false;
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (hits == maxhits)
                {
                    return;
                }

                hits++;
            }
        }

        class CrusoSlash : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                Main.projFrames[Projectile.type] = 20;
            }

            public override void SetDefaults()
            {
                Projectile.CloneDefaults(ProjectileID.Arkhalis);
                Projectile.scale = 1.0f;
                Projectile.width = 360;
                Projectile.height = 360;
                Projectile.ownerHitCheck = true;
                Projectile.usesLocalNPCImmunity = true;
            }

            private List<NPC> hitted = new List<NPC>();

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                hitted.Add(target);
                switch (Projectile.frame)
                {
                    case 2: case 3: case 4: case 5: case 6:
                        hit.Damage *= 2;
                        break;
                    case 10:case 11: case 12: case 13:
                        hit.Crit = false;
                        hit.Damage /= 2;
                        hit.Knockback = KnockbackValue.Averageknockback;
                        break;
                    case 14: case 15: case 16: case 17:
                        hit.Knockback = KnockbackValue.Strongknockback;
                        break;
                }
            }

            public override bool? CanHitNPC(NPC target)
            {
                if (hitted.Contains(target))
                {
                    return false;
                }

                return default;
            }

            public override void AI()
            {
                Player player = Main.player[Projectile.owner]; // Get owner (player)
                Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true); // Get owner position
                player.heldProj = Projectile.whoAmI; // Get projectile

                // Timers
                Projectile.ai[0]++;

                // Functions
                UpdatePlayerVisuals(player, rrp);
                UpdateAim(rrp);


                if (Projectile.frame == Main.projFrames[Projectile.type] - 1)
                {
                    Projectile.Kill(); // Kill projectile if player doesn't channel
                }

                Animate(20, 3);
            }

            public override bool PreDraw(ref Color lightColor) // PreDraw for holdout projectiles
            {
                SpriteEffects effects = Projectile.spriteDirection == -1
                    ? SpriteEffects.FlipVertically
                    : SpriteEffects.None;
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                int frameHeight = texture.Height / Main.projFrames[Projectile.type];
                int spriteSheetOffset = frameHeight * Projectile.frame;
                Vector2 sheetInsertPosition =
                    (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();

                // Always at full brightness, regardless of the surrounding light.

                Main.EntitySpriteDraw(texture, sheetInsertPosition,
                    new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), Color.White,
                    Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, effects,
                    0);
                return false;
            }

            private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
            {
                Projectile.Center =
                    playerHandPos; // Place the Holdout Projectile directly into the player's hand at all times.
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.spriteDirection = Projectile.direction; // Rotate sprite to direction

                // Change player variables to reflect projectile's holdout
                // Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;

                // If you do not multiply by Projectile.direction, the player's hand will point the wrong direction while facing left.
                player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            }

            public override bool? CanCutTiles()
            {
                return base.CanCutTiles();
            }

            private void UpdateAim(Vector2 source)
            {
                // Get the player's current aiming direction as a normalized vector.
                Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
                if (aim.HasNaNs())
                {
                    aim = -Vector2.UnitY;
                }

                if (aim != Projectile.velocity)
                {
                    Projectile.netUpdate = true;
                }

                Projectile.position = Projectile.position + aim * 140;
                Projectile.rotation = aim.ToRotation();
                Projectile.velocity = aim;
            }

            private void PlaySoundRepeat(int interval) // Repeat sound for every interval
            {
                // Play sound intermittently while using, using the vanilla Projectile variable soundDelay.
                if (Projectile.soundDelay <= 0)
                {
                    Projectile.soundDelay = interval;

                    // On the very first frame, the sound playing is skipped. This way it doesn't overlap the starting hiss sound.
                    if (Projectile.ai[0] > 1f)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.position);
                    }
                }
            }


            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                bool collided = false;
                float radius = 50;
                switch (Projectile.frame)
                {
                    case 2: case 3: case 4: case 5: case 6:
                        if (!collided)
                            collided = Projectile.Center.DistanceSQ(targetHitbox.ClosestPointInRect(Projectile.Center))
                                       < radius * Projectile.scale * radius * Projectile.scale;
                        break;
                    case 10:case 11: case 12: case 13:
                        radius = 75;
                        if (!collided)
                            collided = Projectile.Center.DistanceSQ(targetHitbox.ClosestPointInRect(Projectile.Center))
                                       < radius * Projectile.scale * radius * Projectile.scale;
                        break;
                    case 14: case 15: case 16: case 17:
                        radius = 150;
                        if (!collided)
                            collided = Projectile.Center.DistanceSQ(targetHitbox.ClosestPointInRect(Projectile.Center))
                                       < radius * Projectile.scale * radius * Projectile.scale;
                        break;
                    default:
                        collided = false;
                        break;
                }
                return collided;
            }

            private void PlaySoundOnce() // Play sound once lol
            {
                if (Projectile.ai[0] > 1f)
                {
                    SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.position);
                }
            }

            private void Animate(int frames, int ticksPerFrame) // Animate Sprite and play sounds
            {
                if (++Projectile.frameCounter >= ticksPerFrame)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= frames)
                    {
                        Projectile.frame = 0;
                    }

                    if (Projectile.frame == 3 || Projectile.frame == 11) // Play sounds on select frames
                    {
                        PlaySoundOnce();
                        Particles(Projectile.Center, Projectile.width, Projectile.height, 3, 100);
                    }

                    if (Projectile.frame == 11)
                    {
                        hitted.Clear();
                    }
                }
            }

            private void
                Particles(Vector2 source, int width, int height, int intensity = 1,
                    int rate = 30) // Basically just spawns dusts
            {
                source.X -= width / 2;
                source.Y -= height / 2;
                if (intensity <= 1)
                {
                    intensity = 1;
                }

                if (rate >= 101 || rate < 1)
                {
                    rate = 50;
                }

                if (Main.rand.Next(1, 100) <= rate)
                {
                    for (int i = 0; i < intensity; ++i)
                    {
                        Dust.NewDustDirect(source, width, height, 55,
                            Projectile.velocity.X * 10f - Main.rand.NextFloat(-3f, 3f),
                            Projectile.velocity.Y * 10f - Main.rand.NextFloat(-3f, 3f), 0, default(Color), 1.5f);
                    }
                }
            }

            private void DebugParticles() // Test particle spam for hitbox visualization
            {
                for (int i = 0; i < 10; ++i)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        6, -Projectile.velocity.X * 0.1f, -Projectile.velocity.Y * 0.1f,
                        0, default(Color), 1.0f);
                }
            }
        }

        class GoldenBoots : ModBuff
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Golden Boots");
                // Description.SetDefault("Makes the User faster but also attracts more foes");
            }

            public override void Update(Player player, ref int buffIndex)
            {
                Dust.NewDust(player.position, player.width, player.height, 269, -player.velocity.X);
                player.aggro += 100;
                player.maxRunSpeed += 2.5f;
                player.runAcceleration += 0.75f;
                player.runSlowdown += 2.5f;
            }
        }
    }
}