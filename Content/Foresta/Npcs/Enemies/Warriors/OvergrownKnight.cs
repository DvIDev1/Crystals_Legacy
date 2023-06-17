using System;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Armors.Crusolium;
using Crystals.Content.Foresta.Items.Banners;
using Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad;
using Crystals.Content.Foresta.Items.Consumables.Food.Salad;
using Crystals.Content.Foresta.Items.Weapons.Melee.Crusolium;
using Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors
{
    public class OvergrownKnight : ModNPC
    {
        private bool charge;

        private bool chase;

        private bool grounded;

        private bool hitted;

        private float jumpcooldown = 10f;

        private float stamina = 100f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;

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
            NPC.GivenName = "Overgrown Knight";
            NPC.width = 54;
            NPC.height = 56;
            NPC.defense = 6;
            NPC.damage = 32;
            NPC.lifeMax = 150;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 12, 3);
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.6f;

            Banner = Type;
            BannerItem = ModContent.ItemType<OvergrownKnightBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("The knight may have thought he was right," +
                                                  "But his actions caused pain and plight." +
                                                  "For protecting the wrong is not noble or true," +
                                                  "Justice and honor demand what is due.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
                if (spawnInfo.Player.ZoneForest)
                    return SpawnCondition.OverworldNight.Chance * 0.5f;
            return base.SpawnChance(spawnInfo);
        }

        public override void AI()
        {
            NPC.TargetClosest();
            var target = Main.player[NPC.target];
            if (!target.dead && target.active)
                NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
            else if (NPC.Distance(target.Center) >= 1000)
                NPC.EncourageDespawn(5 * 60);
            else
                NPC.DiscourageDespawn(6 * 60);

            if (stamina <= 100f) stamina += 0.5f;


            if (jumpcooldown >= 0) jumpcooldown -= 0.2f;

            grounded = NPC.velocity.Y == 0;

            chase = target.HasBuff<GreenMark>();

            if (!hitted)
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


                if (grounded && jumpcooldown <= 0 && stamina >= 25f)
                    if (NPC.Distance(target.Center) <= 100f)
                    {
                        jumpcooldown = 10f;
                        stamina -= 25f;
                        NPC.velocity.Y -= 6;
                    }

                if (NPC.collideX && grounded && jumpcooldown <= 0) NPC.velocity.Y -= 9;
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
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CrusoliumFragment>(), 10));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CrusoSword>(), 19));
        }

        public override void FindFrame(int frameHeight)
        {
            if (grounded)
            {
                if (NPC.velocity != Vector2.Zero)
                {
                    NPC.frameCounter += 1.0;
                    var frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = 1 + frame * frameHeight;
                    if (frame >= Main.npcFrameCount[NPC.type] - 1 )
                    {
                        NPC.frame.Y = 2 * frameHeight;
                        NPC.frameCounter = 8 * 2;
                    }
                }
            }
            else
            {
                NPC.frame.Y = 0;
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
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<OvergrownWarriorsGore.WarriorSword>());
            for (var i = 0; i < Main.rand.Next(10) + 1; i++)
            {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Bone, -NPC.velocity.X * 2);
                d.noGravity = false;
            }
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.direction == NPC.direction) target.AddBuff(BuffID.BrokenArmor, 60 * 10);
            
            if (target.HasBuff<GreenMark>())
            {
                modifiers.FinalDamage *= 1.20f;
            }

            target.AddBuff(ModContent.BuffType<GreenMark>(), 5 * 60);
        }
    }
}