using System;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad;
using Crystals.Content.Foresta.Items.Consumables.Food.Salad;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Forest_Spirit
{
    public class ForestSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;

            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 34;
            NPC.defense = 3;
            NPC.damage = 13;
            NPC.lifeMax = 35;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 6, 8);
            NPC.aiStyle = -1;
            //ToDo Fix Animation
            AnimationType = NPCID.DungeonSpirit;
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 1f;
        }


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(
                    "Suffering souls yearning for the release from the witch's snare")
            });
        }

        public override void AI()
        {
            
            //Dungeon Spirit AI (THANK LOORRD)
            NPC.TargetClosest();
            
            Vector2 vector109 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num872 = Main.player[NPC.target].Center.X - vector109.X;
            float num873 = Main.player[NPC.target].Center.Y - vector109.Y;
            float num874 = (float) Math.Sqrt(num872 * num872 + num873 * num873);
            float num875 = 10f;
            num874 = num875 / num874;
            num872 *= num874;
            num873 *= num874;
            
            NPC.velocity.X = (NPC.velocity.X * 100f + num872) / 101f;
            NPC.velocity.Y = (NPC.velocity.Y * 100f + num873) / 101f;
            NPC.rotation = (float) Math.Atan2(num873, num872) - 1.57f;
            NPC.position += NPC.netOffset;
            
            int num876 = Dust.NewDust(NPC.position, NPC.width, NPC.height, 61);
            Dust dust = Main.dust[num876];
            dust.velocity *= 0.1f;
            Main.dust[num876].scale = 1.3f;
            Main.dust[num876].noGravity = true;
            NPC.position -= NPC.netOffset;
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest) return SpawnCondition.OverworldNightMonster.Chance * 0.12f;
            return base.SpawnChance(spawnInfo);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<ForestEnergy>(), 2, 1, 5));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Salad>(), 100, 1));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CursedSalad>(), 100, 1));
        }
    }
}