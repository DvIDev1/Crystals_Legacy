using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Crystals.Core;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Summoner
{
    public class GuardianFlame : ModItem
    {
        public override string Texture => AssetDirectory.Summoner + Name;

        public override void SetStaticDefaults() {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        
        public override void SetDefaults()
        {
            Item.Size = new Vector2(48 , 72);
            Item.DamageType = DamageClass.Summon;
            Item.damage = 9;
            Item.knockBack = KnockbackValue.Averageknockback;
            Item.noMelee = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.mana = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            
            Item.buffType = ModContent.BuffType<GuardianFlameBuff>();
            Item.shoot = ModContent.ProjectileType<GuardianFlameSummon>();
        }

        public override bool MagicPrefix()
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
            int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2, true);
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage,
            ref float knockback)
        {
            position = Main.MouseWorld;
        }

        class GuardianFlameBuff : ModBuff
        {
            public override string Texture => AssetDirectory.Summoner + Name;

            public override void SetStaticDefaults()
            {
                Main.buffNoSave[Type] = true;
                Main.buffNoTimeDisplay[Type] = true;
            }

            public override void Update(Player player, ref int buffIndex) {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<GuardianFlameSummon>()] > 0) {
                    player.buffTime[buffIndex] = 18000;
                }
                else {
                    player.DelBuff(buffIndex);
                    buffIndex--;
                }
            }
            
        }

        class GuardianFlameSummon : ModProjectile
        {
            public override string Texture => AssetDirectory.Summoner + Name;

            public override void SetStaticDefaults() {
                Main.projFrames[Projectile.type] = 7;
                ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;


                Main.projPet[Projectile.type] = true;
                ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            }

            public sealed override void SetDefaults() {
                Projectile.width = 30;
                Projectile.height = 44;
                Projectile.tileCollide = false;

                Projectile.friendly = true;
                Projectile.minion = true;
                Projectile.minionSlots = 2f;
                Projectile.penetrate = -1;
            }

            // Here you can decide if your minion breaks things like grass or pots
            public override bool? CanCutTiles() => false;

            public override bool MinionContactDamage() => true;

            private Attack CurrentAttack = Attack.FireShots;

            private int hits;

            private enum Attack
            {
                Chase = 0,
                FireShots = 1,
                FireDash = 2,
                Ignite = 3
            }
            
            private float Timer
            {
                get => Projectile.ai[1];
                set => Projectile.ai[1] = value;
            }

            private bool hasAura = false;

            private Projectile? aura;

            private bool lockedPos;
            
            private List<NPC> dashHit = new List<NPC>();
            
            public override void AI()
            {
                Player player = Main.player[Projectile.owner];

                #region Detection Behavior

                float detectionRadius = 750f;
                Vector2 targetCenter = Projectile.position;
                Vector2 targetPosition = Projectile.position;
                Vector2 targetVelocity = Projectile.velocity;

                bool foundTarget = false;
                float between = Vector2.Distance(targetCenter, Projectile.Center);
                Vector2 targetSize = Vector2.Zero;
                
                
                bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                bool inRange = between < detectionRadius;
                bool inRangePlayer = Projectile.Distance(player.Center) > 2000;
                
                if (player.HasMinionAttackTargetNPC) {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    bool DashHit = dashHit.Contains(npc);
                    if (between < 2000f && !DashHit) {
                        detectionRadius = between;
                        targetCenter = npc.Center;
                        targetPosition = npc.position;
                        targetVelocity = npc.velocity;
                        foundTarget = true;
                    }
                }
                
                if (!foundTarget) {
                    for (int i = 0; i < Main.maxNPCs; i++) {
                        NPC npc = Main.npc[i];
                        bool DashHit = dashHit.Contains(npc);
                        if (npc.CanBeChasedBy()) {
                            if (((inRange && inRangePlayer && !DashHit) || !foundTarget)) {
                                List<Vector2> Positions = new List<Vector2>();
                                Positions.Add(npc.Center);
                                if (npc.Center == Pathfinding.GetNearestPos(player.Center , Positions))
                                {
                                    detectionRadius = between;
                                    targetCenter = npc.Center;
                                    targetPosition = npc.position;
                                    targetVelocity = npc.velocity;
                                    foundTarget = true;
                                }
                            }
                        }
                    }
                }

                #endregion
                
                #region Attacking Behavior

                if (foundTarget)
                {
                    Timer++;

                    switch (CurrentAttack)
                    {
                        case Attack.Chase:
                            chase();
                            if (Timer >= 60)
                            {
                                Timer = 0;
                                CurrentAttack = (Attack)Main.rand.Next(4);
                            }
                            break;
                        
                        case Attack.FireShots:
                            ShootFire(targetCenter , targetVelocity);
                            if (Timer >= 180)
                            {
                                Timer = 0;
                                CurrentAttack = Attack.FireDash;
                            }
                            break;
                        case Attack.FireDash:
                            DashTowardsTarget(targetCenter);
                            if (Timer >= 180)
                            {
                                Timer = 0;
                                CurrentAttack = Attack.Ignite;
                                dashHit.Clear();
                            }
                            break;
                        case Attack.Ignite:
                            Ignite();
                            if (Timer >= 360)
                            {
                                Timer = 0;
                                CurrentAttack = Attack.Chase;
                            }
                            break;
                    }
                }

                void chase()
                {
                    Projectile.velocity = Projectile.DirectionTo(targetCenter) * Projectile.Distance(targetCenter) * 0.02f;
                }
                
                void ShootFire(Vector2 Pos , Vector2 Velocity)
                {
                    if (Timer % 15 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(null),
                            Projectile.Top, Projectile.Top.DirectionTo(Pos + Velocity / 2f) * 16, 15,
                            (int) (Projectile.damage * 1.5f), KnockbackValue.Averageknockback);
                    }

                    Projectile.velocity = Projectile.DirectionTo(targetCenter) * 2;
                }
                
                void DashTowardsTarget(Vector2 Pos)
                {
                    Projectile.velocity +=
                        Projectile.DirectionTo(Pos);
                    Projectile.velocity.X = Math.Clamp(Projectile.velocity.X, -10f, 10f);
                    Projectile.velocity.Y = Math.Clamp(Projectile.velocity.Y, -10f, 10f);
                }
                
                void Ignite()
                {
                    hasAura = aura != null && aura.active && aura.type == ModContent.ProjectileType<FireAura>();
                    if (!hasAura)
                    {
                        aura = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(null), Projectile.position, Vector2.Zero,
                            ModContent.ProjectileType<FireAura>(), Projectile.damage * 2, 0, Projectile.owner);
                        hasAura = true;
                    }
                    else
                    {
                        aura.Center = Projectile.Center;
                    }
                    Projectile.velocity = Projectile.DirectionTo(targetCenter);
                }
                
                if (aura != null && CurrentAttack != Attack.Ignite 
                                 && aura.type == ModContent.ProjectileType<FireAura>()) {
                    aura?.Kill();
                }

                #endregion

                #region Passive Behavior
                
                Projectile.ai[2]++;
                
                if (!foundTarget)
                {
                    Timer = 0;
                    CurrentAttack = (Attack)Main.rand.Next(4);
                    
                    Vector2 idlePosition = player.Center;
                    idlePosition.Y -= 48f; 
                    
                    Projectile.position.Y +=  MathFunctions.SineWave(0.5f, 2f, (int) Projectile.ai[2] / 10f);
                    
                    float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
                    idlePosition.X += minionPositionOffsetX;
                    
                    Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                    float distanceToIdlePosition = vectorToIdlePosition.Length();
                    if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f) { 
                        Projectile.position = idlePosition;
                        Projectile.velocity *= 0.0f;
                        Projectile.netUpdate = true;
                    }else if (Main.myPlayer == player.whoAmI && distanceToIdlePosition < 2000f && Main.myPlayer == player.whoAmI && distanceToIdlePosition > 300f)
                    {
                        Projectile.velocity = Projectile.DirectionTo(player.Center) * 5f;
                    }
                    else
                    {
                        Projectile.velocity *= 0.99f;
                    }

                }
                #endregion
                
                #region General Behavior
                Projectile.rotation = Projectile.velocity.X * 0.05f;
                
                float lightMultiplier = (float) MathFunctions.SineWave(0.25f, 0.5f, (int) Projectile.ai[2] / 20f) + 0.5f;

                Lighting.AddLight(Projectile.Center , 0.25f + lightMultiplier , 0.075f + lightMultiplier, 0 + lightMultiplier);
                
                if (++Projectile.frameCounter >= 9f)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= Main.projFrames[Projectile.type] - 1)
                    {
                        Projectile.frame = 0;
                    }
                }
                
                if (player.dead || !player.active) {
                    aura?.Kill();
                    player.ClearBuff(ModContent.BuffType<GuardianFlameBuff>());
                }
                if (player.HasBuff(ModContent.BuffType<GuardianFlameBuff>())) {
                    Projectile.timeLeft = 2;
                }
                #endregion
            }

            public override bool? CanDamage()
            {
                return CurrentAttack != Attack.Ignite;
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (CurrentAttack != Attack.FireDash)
                {
                    target.AddBuff(BuffID.OnFire3 , 60*5);
                }
                else
                {
                    target.AddBuff(BuffID.OnFire , 60*5);
                }
            }

            public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
            {
                if (CurrentAttack == Attack.FireDash)
                {
                    dashHit.Add(target);
                    modifiers.SourceDamage *= 2;
                }
                else 
                {
                    modifiers.Knockback *= 0;
                }
            }

            class FireAura : ModProjectile
            {
                public override string Texture => "Crystals/Assets/Other/FX/light_01-small";

                public sealed override void SetDefaults() {
                    Projectile.width = 256;
                    Projectile.height = 256;
                    Projectile.tileCollide = false;
                    Projectile.timeLeft = 360;
                    Projectile.friendly = true;
                    Projectile.penetrate = -1;
                    Projectile.scale = 1f;
                    Projectile.usesLocalNPCImmunity = true;
                    Projectile.DamageType = DamageClass.Summon;
                }

                public override void OnSpawn(IEntitySource source)
                {
                    Projectile.scale = 0f;
                }

                public override void AI()
                {
                    Player player = Main.player[Projectile.owner];
                    Projectile.ai[0] += 1f / 360f;
                    Projectile.scale = MathHelper.SmoothStep(0f, 1f, (float) MathFunctions.EaseFunctions.EaseInOutQuad(Projectile.ai[0]));
                    if (Projectile.Distance(player.Center) <= 91f)
                    {
                        player.AddBuff(BuffID.Campfire , Projectile.timeLeft);
                    }
                }

                public override Color? GetAlpha(Color lightColor)
                {
                    return new Color(163, 47, 11) * 3;
                }

                public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
                {
                    bool collided = false;
                    float collisionPoint = 0;
                    float radius = (Projectile.width / 2) * Projectile.scale;
                    if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft() , targetHitbox.Size(), Projectile.Center - Projectile.Size / 4f, Projectile.Center + Projectile.Size / 4f, Projectile.width * Projectile.scale , ref collisionPoint))
                    {
                        collided = true;
                    }
                    return collided;
                }

                public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
                {
                    target.AddBuff(BuffID.OnFire3 , 60*10);
                }
            }
            
        }

    }
}