using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Armors.Crusolium;
using Crystals.Content.Foresta.Items.Banners;
using Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad;
using Crystals.Content.Foresta.Items.Consumables.Food.Salad;
using Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium;
using Crystals.Core.Systems.SoundSystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors
{
    public class OvergrownArcher : ModNPC
    {
        private bool aiming;

        public float AimTime = 2 * 60;

        private bool grounded;

        private bool hitted;

        public float AimProgress
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        
        public float FleeTime
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 20;

            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f
                // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.GivenName = "Overgrown Archer";
            NPC.width = 54;
            NPC.height = 66;
            NPC.defense = 4;
            NPC.damage = 21;
            NPC.lifeMax = 120;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 10, 9);
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.3f;

            Banner = Type;
            BannerItem = ModContent.ItemType<OvergrownKnightBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(
                    "His arrows flew true, always hitting their mark, " +
                    "But little did he know, he was causing much dark. " +
                    "For the people he fought against were innocent and true, " +
                    "And his misguided loyalty only caused more rue.")
            });
        }

        public override void OnSpawn(IEntitySource source)
        {
            var target = Main.player[NPC.target];
            NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
        }

        public bool Fleeing;

        public override void AI()
        {
            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            Fleeing = FleeTime >= 0;

            if (target.dead)
            {
                CancelAiming();
            }
            
            if (!target.dead && target.active)
            {
                NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
            }
            else if (NPC.Distance(target.Center + target.velocity) >= 1000 || target.dead || !target.active)
            {
                CancelAiming();
                NPC.EncourageDespawn(5 * 60);
            }
            else
            {
                NPC.DiscourageDespawn(6 * 60);
            }

            grounded = NPC.velocity.Y == 0;

            
            
            if (NPC.Distance(target.Center + target.velocity) <= 750 
                && NPC.Distance(target.Center + target.velocity) >= 250 && grounded && !Fleeing && !target.dead)
            {
                StartAiming();
            }
            else if (NPC.Distance(target.Center + target.velocity) <= 250 && !aiming || Fleeing)
            {
                NPC.spriteDirection = NPC.direction = -Math.Sign(target.Center.X - NPC.Center.X);
                CancelAiming();
            }

            if (FleeTime >= 0) { FleeTime--; }
            

            if (aiming) UpdateAiming();

            if (!hitted && !aiming)
            {
                if (target.HasBuff<GreenMark>())
                {
                    NPC.velocity.X += NPC.direction * 0.10f;
                    NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -4, 4);
                }
                else
                {
                    NPC.velocity.X += NPC.direction * 0.10f;
                    NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2, 2);
                }


                if (NPC.collideX && grounded) NPC.velocity.Y -= 6;
            }
            else
            {
                if (NPC.collideX || NPC.collideY) hitted = false;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<ForestEnergy>(), 4, 1, 3));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Salad>(), 100, 1));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CursedSalad>(), 100, 1));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<BrokenCrusoSet.BrokenHelmet>(), 500));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<BrokenCrusoSet.BrokenChest>(), 500));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<BrokenCrusoSet.BrokenBoots>(), 500));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Crusolium_Bow>(), 19));
        }
        
        public void UpdateAiming()
        {
            AimProgress++;
            if (AimProgress == 60f) 
            {
                SoundEngine.PlaySound(SoundSystem.ChargeBow);
            }
            if (AimProgress >= AimTime) Shoot();
        }

        public void Shoot()
        {
            var target = Main.player[NPC.target];
            SoundEngine.PlaySound(SoundID.Item5, NPC.Center);
            Projectile.NewProjectile(Terraria.Entity.GetSource_None(), NPC.Center,
                NPC.DirectionTo(target.Top) * 8f,
                ModContent.ProjectileType<HostileCrusolium_Arrow>(), NPC.damage / 4, KnockbackValue.Averageknockback);
            AimProgress = 0;
        }

        public void StartAiming()
        {
            aiming = true;
            NPC.velocity.X = 0;
        }

        public void CancelAiming()
        {
            aiming = false;
            AimProgress = 0;
        }


        /*public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow",
                AssetRequestMode.ImmediateLoad).Value;
            SpriteEffects effect = SpriteEffects.None;
            
            if (NPC.spriteDirection != -1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.None;
            }
            
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    NPC.position.X - Main.screenPosition.X + NPC.frame.Width * 0.5f,
                    NPC.position.Y - Main.screenPosition.Y + NPC.frame.Height - NPC.frame.Height * 0.5f + 2f
                ),
                NPC.frame,
                Color.White,
                NPC.rotation,
                NPC.frame.Size() * 0.5f,
                NPC.scale,
                effect,
                0f
            );
        }*/

        public override void FindFrame(int frameHeight)
        {
            if (!aiming)
            {
                if (grounded)
                {
                    NPC.frameCounter++;
                    int frame = (int)(NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame >= 15)
                    {
                        NPC.frame.Y = 2 * frameHeight;
                        NPC.frameCounter = 8 * 2;
                    }
                }
                else
                {
                    NPC.frame.Y = 0;
                }
            }
            else
            {
                List<Vector2> positions = new List<Vector2>();
                positions.AddRange(new[] { NPC.Top , NPC.TopLeft , NPC.TopRight , NPC.BottomLeft , 
                    NPC.BottomRight , NPC.Left , NPC.Right , NPC.Bottom});
                Player target = Main.player[NPC.target];
                Vector2 targetHittingPoint = NPC.DirectionTo(target.Top) * 32f;
                if (Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint , positions).Equals(NPC.Top))
                {
                    NPC.frame.Y = 19 * frameHeight;
                }else if (Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint  , positions).Equals(NPC.TopRight) 
                          || Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint  , positions).Equals(NPC.TopLeft))
                {
                    NPC.frame.Y = 18 * frameHeight;
                }else if (Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint  , positions).Equals(NPC.Left) 
                          || Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint  , positions).Equals(NPC.Right))
                {
                    NPC.frame.Y = 17 * frameHeight;
                }else if (Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint , positions).Equals(NPC.BottomLeft)
                          || Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint , positions).Equals(NPC.BottomRight))
                {
                    NPC.frame.Y = 16 * frameHeight;
                }else if (Pathfinding.GetNearestPos(NPC.Center + targetHittingPoint , positions).Equals(NPC.Bottom))
                {
                    NPC.frame.Y = 15 * frameHeight;
                }
            }
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
                if (spawnInfo.Player.ZoneForest)
                    return SpawnCondition.OverworldNight.Chance * 0.25f;
            return base.SpawnChance(spawnInfo);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (aiming)
            {
                FleeTime = 60;
            }
            CancelAiming();
            hitted = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (aiming)
            {
                FleeTime = 60;
            }
            CancelAiming();
            hitted = true;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<GreenMark>(), 5 * 60);
            if (target.HasBuff<GreenMark>())
            {
                modifiers.FinalDamage *= 1.20f;
            }
        }
        
        public override void OnKill()
        {
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<OvergrownWarriorsGore.WarriorHead>());
            for (var i = 0; i < 2; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                    ModContent.GoreType<OvergrownWarriorsGore.WarriorLeg>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<OvergrownWarriorsGore.WarriorArm>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<OvergrownWarriorsGore.WarriorArm2>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<OvergrownWarriorsGore.WarriorTorso>());
            for (var i = 0; i < Main.rand.Next(10) + 1; i++)
            {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Bone, -NPC.velocity.X * 2);
                d.noGravity = false;
            }
        }

        private class HostileCrusolium_Arrow : ModProjectile
        {
            public int time;

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Crusolium Arrow");
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2; // The length of old position to be recorded
                ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
            }

            public override void SetDefaults()
            {
                Projectile.width = 14;
                Projectile.height = 32;
                Projectile.hostile = true;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;
                Projectile.aiStyle = ProjAIStyleID.Arrow;
                Projectile.penetrate = 2;
            }

            public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
            {
                
                if (target.HasBuff(ModContent.BuffType<GreenMark>()))
                {
                    modifiers.FinalDamage *= 1.5f;
                }
                else
                {
                    target.AddBuff(ModContent.BuffType<GreenMark>(), 60 * 7);
                    Projectile.Kill();
                }
            }

            public override bool PreAI()
            {
                time++;
                Dust.NewDustPerfect(Projectile.Center, 61);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.velocity += Projectile.velocity * 0.02f;
                return false;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                var texture = TextureAssets.Projectile[Projectile.type].Value;

                // Redraw the projectile with the color not influenced by light
                var drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (var k = 0; k < Projectile.oldPos.Length; k++)
                {
                    var drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin +
                                  new Vector2(0f, Projectile.gfxOffY);
                    var color = Projectile.GetAlpha(lightColor) *
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
    }
}