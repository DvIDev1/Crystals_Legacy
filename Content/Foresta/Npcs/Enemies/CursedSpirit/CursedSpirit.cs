using System;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.CursedSpirit;

public class CursedSpirit : ModNPC
{
     public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;

            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 34;
            NPC.defense = 0;
            NPC.damage = 33;
            NPC.lifeMax = 45;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 14, 9);
            NPC.aiStyle = -1;
            
            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 1f;
        }


        //Todo Beastiary 
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime
            });
        }

        public override void AI()
        {
            //Dungeon Spirit AI (THANK LOORRD)
            NPC.TargetClosest();

            Vector2 vector109 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num872 = Main.player[NPC.target].Center.X - vector109.X;
            float num873 = Main.player[NPC.target].Center.Y - vector109.Y;
            float num874 = (float)Math.Sqrt(num872 * num872 + num873 * num873);
            float num875 = 10f;
            num874 = num875 / num874;
            num872 *= num874;
            num873 *= num874;

            NPC.velocity.X = (NPC.velocity.X * 100f + num872) / 101f;
            NPC.velocity.Y = (NPC.velocity.Y * 100f + num873) / 101f;
            NPC.rotation = (float)Math.Atan2(num873, num872) - 1.57f;
            NPC.position += NPC.netOffset;

            if (Main.rand.NextBool())
            {
                int num876 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CorruptTorch);
                Dust dust = Main.dust[num876];
                dust.velocity *= 0.1f;
                Main.dust[num876].scale = 1.3f;
                Main.dust[num876].noGravity = true;
            }

            NPC.position -= NPC.netOffset;
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.instance.LoadNPC(NPC.type);
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
                
            SpriteEffects effect = SpriteEffects.None;
            
            if (NPC.spriteDirection != -1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.None;
            }
            
            Vector2 drawOrigin = new Vector2(NPC.frame.Width * 0.5f, NPC.frame.Height * 0.5f);
            for (int k = 0; k < NPC.oldPos.Length; k++) {
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effect, 0);
            }

            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            ++NPC.frameCounter;
            if (NPC.frameCounter >= 6.0)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0.0;
            }

            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
            {
                NPC.frame.Y = 0;
            }
        }
        
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CursedEnergy>(), 2, 1, 4));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CursedSalad>(), 100, 1));
        }
}