using Crystals.Content.Foresta.Items;
using Crystals.Core;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Leafling.Gold
{
    public class GoldenLeafling : ModNPC
    {
        public override string Texture => AssetDirectory.Gold_Leafling + Name;

        private bool fast;

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
            NPC.defense = 14;
            NPC.damage = 30;
            NPC.lifeMax = 200;
            NPC.value = ValueHelper.GetCoinValue(0, 10, 0, 0);
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0;
            NPC.friendly = false;
            NPC.HitSound = SoundID.NPCDeath7;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = NPCAIStyleID.Unicorn;
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<GoldenLeaf>(), 4));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
            {
                if (spawnInfo.Player.ZoneForest)
                    return SpawnCondition.OverworldNight.Chance * 0.01f;
                return 0;
            }

            return 0;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (fast)
            {
                modifiers.FinalDamage.Flat += (int)NPC.velocity.X;
            }
        }

        public override void AI()
        {
            NPC.rotation += NPC.velocity.X / 6;
            var player = Main.player[NPC.target];
            if (NPC.velocity.X / 6 >= 1 || NPC.velocity.X / 6 <= -1)
            {
                fast = true;
                if (Main._rand.NextBool()) Dust.NewDustPerfect(NPC.Center, 64);
            }
            else
            {
                fast = false;
            }
        }

        public override void OnKill()
        {
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, NPC.velocity,
                ModContent.GoreType<LeaflingGoldGore1>());
            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, NPC.velocity,
                ModContent.GoreType<LeaflingGoldGore2>());
            for (var i = 0; i < Main.rand.Next(4) + 1; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, NPC.velocity,
                    ModContent.GoreType<LeaflingGoldGore>());
            for (var i = 0; i < Main.rand.Next(6) + 1; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldCoin,
                    Main.rand.NextVector2Circular(2f, 2f).X, Main.rand.NextVector2Circular(2f, 2f).Y);
        }

        private class LeaflingGoldGore1 : ModGore
        {
            public override string Texture => AssetDirectory.Gold_Leafling + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        private class LeaflingGoldGore2 : ModGore
        {
            public override string Texture => AssetDirectory.Gold_Leafling + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        private class LeaflingGoldGore : ModGore
        {
            public override string Texture => AssetDirectory.Gold_Leafling + Name;

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