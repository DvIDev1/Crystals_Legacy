using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Banners;
using Crystals.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Nature_Slime
{
    public class Nature_Slime : ModNPC
    {
        public override string Texture => AssetDirectory.NatureSlime + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nature Slime");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.GivenName = "Nature Slime";
            NPC.width = 16;
            NPC.height = 15;
            NPC.defense = 3;
            NPC.damage = 25;
            NPC.lifeMax = 60;
            NPC.value = 120.00f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.scale = 2f;
            AnimationType = 1;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 1;

            Banner = Type;
            BannerItem = ModContent.ItemType<NatureSlimeBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Slimes infused with the power of nature")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<ForestGel>(), 2, 1, 3));
        }

        public override void OnKill()
        {
            for (var i = 0; i < Main.rand.Next(12, 24) + 1; i++)
                Dust.NewDustPerfect(NPC.position, DustID.t_Slime, Main.rand.NextVector2Circular(i, i), 0,
                    Color.LightGreen);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
                if (spawnInfo.Player.ZoneForest)
                    return SpawnCondition.OverworldDaySlime.Chance * 0.50f;

            return 0;
        }
    }
}