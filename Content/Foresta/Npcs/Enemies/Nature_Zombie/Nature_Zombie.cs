using System;
using System.Collections.Generic;
using System.Linq;
using Crystals.Content.Foresta.Items.Banners;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Nature_Zombie
{
    public class Nature_Zombie : ModNPC
    {
        private bool grounded;

        private bool hitted;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Zombie");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

            NPCID.Sets.Zombies[Type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Soulless walking Corpses Reanimated by Her")
            });

            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.GivenName = "Nature Zombie";
            NPC.width = 40;
            NPC.height = 50;
            NPC.defense = 3;
            NPC.damage = 13;
            NPC.lifeMax = 90;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 6, 24);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = -1;
            AnimationType = NPCID.Zombie;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.45f;

            Banner = Type;
            BannerItem = ModContent.ItemType<NatureZombieBanner>();
        }

        //Todo Buff During Spiritual Night  
        public override void AI()
        {
            NPC.TargetClosest();

            var NPCTarget = new List<NPC>();

            foreach (var npc in Main.npc)
                if (npc.townNPC && npc.active)
                    NPCTarget.Add(npc);

            foreach (var npc in NPCTarget)
                if (!npc.active)
                    NPCTarget.Remove(npc);

            var target = Main.player[NPC.target];
            if (!target.dead && target.active)
            {
                NPC.DiscourageDespawn(5 * 60);
                if (NPCTarget.Any(x => x.Distance(NPC.Center) <= NPC.Distance(target.Center)))
                {
                    foreach (var npc in NPCTarget)
                        if (npc.Distance(NPC.Center) <= NPC.Distance(target.Center))
                            NPC.spriteDirection = NPC.direction = Math.Sign(npc.Center.X - NPC.Center.X);
                }
                else
                {
                    NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
                }
            }
            else if (NPC.Distance(target.Center) >= 1000)
            {
                NPC.EncourageDespawn(5 * 60);
            }
            else
            {
                NPC.DiscourageDespawn(6 * 60);
            }

            grounded = NPC.velocity.Y == 0;

            if (!hitted)
            {
                NPC.velocity.X += NPC.direction * 0.10f;
                NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1, 1);

                if (NPC.collideX && grounded) NPC.velocity.Y -= 6;
            }
            else
            {
                if (NPC.collideX || NPC.collideY) hitted = false;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest) return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            return base.SpawnChance(spawnInfo);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public void Heal()
        {
            var heal = Main.rand.Next(10, 30);
            if (heal + NPC.life >= NPC.lifeMax)
            {
                NPC.HealEffect(NPC.Hitbox, NPC.lifeMax - NPC.life);
                NPC.life = NPC.lifeMax;
            }
            else
            {
                NPC.HealEffect(NPC.Hitbox, heal);
                NPC.life += heal;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool())
                if (target.statLife != target.statLifeMax)
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.Center);
                    for (var i = 0; i < 10; i++)
                    {
                        var d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0, 2);
                        d.noGravity = false;
                        d.scale = 2;
                    }
                    
                    Heal();
                }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool())
                if (target.life != target.lifeMax)
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.Center);
                    for (var i = 0; i < 10; i++)
                    {
                        var d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0, 2);
                        d.noGravity = false;
                        d.scale = 2;
                    }
                    
                    modifiers.SetCrit();
                    Heal();
                }
        }

        public override void OnKill()
        {
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieHead>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieTorso>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieArm>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieArm2>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieLeg>());
            for (var i = 0; i < Main.rand.Next(10) + 1; i++)
            {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood, -NPC.velocity.X * 2);
                d.noGravity = false;
                d.scale = 2.5f;
            }
        }
    }

    public class Nature_Zombie2 : ModNPC
    {
        private bool grounded;

        private bool hitted;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Zombie");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
            
            NPCID.Sets.Zombies[Type] = true;

            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Soulless walking Corpses Reanimated by Her")
            });
        }

        public override void SetDefaults()
        {
            NPC.GivenName = "Nature Zombie";
            NPC.width = 40;
            NPC.height = 50;
            NPC.defense = 6;
            NPC.damage = 10;
            NPC.lifeMax = 120;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 5, 9);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = -1;
            AnimationType = NPCID.Zombie;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.6f;

            Banner = Type;
            BannerItem = ModContent.ItemType<NatureZombieBanner>();
        }

        //Todo Buff During Spiritual Night  
        public override void AI()
        {
            NPC.TargetClosest();

            var NPCTarget = new List<NPC>();

            foreach (var npc in Main.npc)
                if (npc.townNPC && npc.active)
                    NPCTarget.Add(npc);

            foreach (var npc in NPCTarget)
                if (!npc.active)
                    NPCTarget.Remove(npc);

            var target = Main.player[NPC.target];
            if (!target.dead && target.active)
            {
                NPC.DiscourageDespawn(5 * 60);
                if (NPCTarget.Any(x => x.Distance(NPC.Center) <= NPC.Distance(target.Center)))
                {
                    foreach (var npc in NPCTarget)
                        if (npc.Distance(NPC.Center) <= NPC.Distance(target.Center))
                            NPC.spriteDirection = NPC.direction = Math.Sign(npc.Center.X - NPC.Center.X);
                }
                else
                {
                    NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
                }
            }
            else if (NPC.Distance(target.Center) >= 1000)
            {
                NPC.EncourageDespawn(5 * 60);
            }
            else
            {
                NPC.DiscourageDespawn(6 * 60);
            }

            grounded = NPC.velocity.Y == 0;

            if (!hitted)
            {
                NPC.velocity.X += NPC.direction * 0.10f;
                NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1, 1);

                if (NPC.collideX && grounded) NPC.velocity.Y -= 6;
            }
            else
            {
                if (NPC.collideX || NPC.collideY) hitted = false;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest) return SpawnCondition.OverworldNightMonster.Chance * 0.12f;
            return base.SpawnChance(spawnInfo);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public void Heal()
        {
            var heal = Main.rand.Next(10, 20);
            if (heal + NPC.life >= NPC.lifeMax)
            {
                NPC.HealEffect(NPC.Hitbox, NPC.lifeMax - NPC.life);
                NPC.life = NPC.lifeMax;
            }
            else
            {
                NPC.HealEffect(NPC.Hitbox, heal);
                NPC.life += heal;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool())
                if (target.statLife != target.statLifeMax)
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.Center);
                    for (var i = 0; i < 10; i++)
                    {
                        var d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0, 2);
                        d.noGravity = false;
                        d.scale = 2;
                    }
                    
                    Heal();
                }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool())
                if (target.life != target.lifeMax)
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.Center);
                    for (var i = 0; i < 10; i++)
                    {
                        var d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0, 2);
                        d.noGravity = false;
                        d.scale = 2;
                    }
                    
                    modifiers.SetCrit();
                    Heal();
                }
        }

        public override void OnKill()
        {
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieHead2>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieTorso>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieArm>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieArm2>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieLeg>());
            for (var i = 0; i < Main.rand.Next(10) + 1; i++)
            {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood, -NPC.velocity.X * 2);
                d.noGravity = false;
                d.scale = 2.5f;
            }
        }
    }

    public class Nature_Zombie3 : ModNPC
    {
        private bool grounded;

        private bool hitted;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Zombie");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

            NPCID.Sets.Zombies[Type] = true;
            
            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("Soulless walking Corpses Reanimated by Her")
            });
        }

        public override void SetDefaults()
        {
            NPC.GivenName = "Nature Zombie";
            NPC.width = 40;
            NPC.height = 50;
            NPC.defense = 3;
            NPC.damage = 19;
            NPC.lifeMax = 85;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 3, 13);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = -1;
            AnimationType = NPCID.Zombie;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.7f;

            Banner = Type;
            BannerItem = ModContent.ItemType<NatureZombieBanner>();
        }

        //Todo Buff During Spiritual Night  
        public override void AI()
        {
            NPC.TargetClosest();

            var NPCTarget = new List<NPC>();

            foreach (var npc in Main.npc)
                if (npc.townNPC && npc.active)
                    NPCTarget.Add(npc);

            foreach (var npc in NPCTarget)
                if (!npc.active)
                    NPCTarget.Remove(npc);

            var target = Main.player[NPC.target];
            if (!target.dead && target.active)
            {
                NPC.DiscourageDespawn(5 * 60);
                if (NPCTarget.Any(x => x.Distance(NPC.Center) <= NPC.Distance(target.Center)))
                {
                    foreach (var npc in NPCTarget)
                        if (npc.Distance(NPC.Center) <= NPC.Distance(target.Center))
                            NPC.spriteDirection = NPC.direction = Math.Sign(npc.Center.X - NPC.Center.X);
                }
                else
                {
                    NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
                }
            }
            else if (NPC.Distance(target.Center) >= 1000)
            {
                NPC.EncourageDespawn(5 * 60);
            }
            else
            {
                NPC.DiscourageDespawn(6 * 60);
            }

            grounded = NPC.velocity.Y == 0;

            if (!hitted)
            {
                NPC.velocity.X += NPC.direction * 0.10f;
                NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2, 2);

                if (NPC.collideX && grounded) NPC.velocity.Y -= 7;
            }
            else
            {
                if (NPC.collideX || NPC.collideY) hitted = false;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest) return SpawnCondition.OverworldNightMonster.Chance * 0.12f;
            return base.SpawnChance(spawnInfo);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            hitted = true;
        }

        public void Heal()
        {
            var heal = Main.rand.Next(5, 10);
            if (heal + NPC.life >= NPC.lifeMax)
            {
                NPC.HealEffect(NPC.Hitbox, NPC.lifeMax - NPC.life);
                NPC.life = NPC.lifeMax;
            }
            else
            {
                NPC.HealEffect(NPC.Hitbox, heal);
                NPC.life += heal;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextBool())
                if (target.statLife != target.statLifeMax)
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.Center);
                    for (var i = 0; i < 10; i++)
                    {
                        var d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0, 2);
                        d.noGravity = false;
                        d.scale = 2;
                    }
                    
                    Heal();
                }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool())
                if (target.life != target.lifeMax)
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.Center);
                    for (var i = 0; i < 10; i++)
                    {
                        var d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood, 0, 2);
                        d.noGravity = false;
                        d.scale = 2;
                    }
                    
                    modifiers.SetCrit();
                    Heal();
                }
        }

        public override void OnKill()
        {
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieHead3>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieTorso>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieArm>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieArm2>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<NatureZombieGore.NatureZombieLeg>());
            for (var i = 0; i < Main.rand.Next(10) + 1; i++)
            {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Blood, -NPC.velocity.X * 2);
                d.noGravity = false;
                d.scale = 2.5f;
            }
        }
    }
}