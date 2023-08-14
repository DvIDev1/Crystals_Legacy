using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Banners;
using Crystals.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Leafling
{
    public class Leafling : ModNPC
    {
        private bool fast;
        public override string Texture => AssetDirectory.Leafling + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Leafling");
            Main.npcFrameCount[NPC.type] = 2;

            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 6f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.Size = new Vector2(40, 40);
            NPC.height = 38;
            NPC.defense = 7;
            NPC.damage = 30;
            NPC.lifeMax = 120;
            NPC.value = 63;
            NPC.value = 63.0f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.50f;
            NPC.friendly = false;
            NPC.HitSound = SoundID.NPCDeath7;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = NPCAIStyleID.Unicorn;

            Banner = Type;
            BannerItem = ModContent.ItemType<LeaflingBanner>();
        }

        public override void FindFrame(int frameHeight)
        {
            if (fast)
                NPC.frame.Y = frameHeight;
            else NPC.frame.Y = 0 * frameHeight;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("A Soul in the Only place it found to rest.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<ForestEnergy>(), 3, 1, 4));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<ForestShard>(), 40));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
            {
                if (spawnInfo.Player.ZoneForest)
                    return SpawnCondition.OverworldNightMonster.Chance * 0.50f;
                return 0;
            }

            return 0;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (fast)
            {
                modifiers.FinalDamage.Flat += (int)NPC.velocity.X;
                target.AddBuff(BuffID.Poisoned, 60 * 10);
            }
        }

        public override void AI()
        {
            NPC.rotation += NPC.velocity.X / 6;
            var player = Main.player[NPC.target];
            if (NPC.velocity.X / 6 >= 1 || NPC.velocity.X / 6 <= -1)
            {
                fast = true;
                Dust.NewDustPerfect(NPC.Center, 61);
            }
            else
            {
                fast = false;
            }
        }

        public override void OnKill()
        {
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, NPC.velocity * 2,
                ModContent.GoreType<LeaflingWoodGore1>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, NPC.velocity * 2,
                ModContent.GoreType<LeaflingWoodGore2>());
            for (var i = 0; i < Main.rand.Next(4) + 1; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, NPC.velocity * 4,
                    ModContent.GoreType<LeaflingLeafGore>());
            for (var i = 0; i < Main.rand.Next(6) + 1; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, Main.rand.NextVector2Circular(i, i),
                    GoreID.TreeLeaf_Normal);
        }

        private class LeaflingWoodGore1 : ModGore
        {
            public override string Texture => AssetDirectory.Leafling + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        private class LeaflingWoodGore2 : ModGore
        {
            public override string Texture => AssetDirectory.Leafling + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        private class LeaflingLeafGore : ModGore
        {
            public override string Texture => AssetDirectory.Leafling + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }
    }
}