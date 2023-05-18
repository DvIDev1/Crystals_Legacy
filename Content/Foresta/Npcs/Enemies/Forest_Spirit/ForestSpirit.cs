using System;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

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
            NPC.damage = 26;
            NPC.lifeMax = 35;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 6, 8);
            NPC.aiStyle = -1;
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

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            var frame = (int) (NPC.frameCounter / 8.0);
            NPC.frame.Y = 1 + frame * frameHeight;
            if (frame >= Main.npcFrameCount[NPC.type] - 1 )
            {
                NPC.frame.Y = frameHeight;
                NPC.frameCounter = 0;
            }
        }
        
        private Vector2 targetPos = Vector2.Zero;

        private Vector2 attackStartPos = Vector2.Zero;

        private bool canDash;

        private bool dash = false;

        private float dashCooldown = 60 * 3;

        public override void AI()
        {

            #region Detection

            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            canDash = (target.Distance(target.Center) < 1000f) && dashCooldown <= 0;

            #endregion

            #region Attack Behaviour

            if (dashCooldown > 0)
            {
                dashCooldown--;
            }

            if (!canDash)
            {
                NPC.velocity += NPC.DirectionTo(target.Center);
                NPC.velocity = Vector2.Clamp(NPC.velocity , new Vector2(-5) , new Vector2(5));
            }
            else
            {
                if (canDash)
                {
                    if (!dash)
                    {
                        StartDash(target.Center);
                    }
                    else
                    {
                        Dash();
                    }
                }
            }

            #endregion

        }
        
        public void StartDash(Vector2 position)
        {
            dash = true;
            targetPos = position;
            NPC.ai[0] = 0f;
            attackStartPos = NPC.Center;
        }

        public void Dash()
        {
            if (NPC.ai[0] < 1f)
            {
                NPC.ai[0] += 0.05f;
                NPC.velocity +=
                    NPC.DirectionTo(
                        Vector2.Lerp(attackStartPos, targetPos, (float) EaseFunctions.easeInBack(NPC.ai[0]))) 
                    * attackStartPos.Distance(targetPos) * 0.0175f;
            }
            else
            {
                dashCooldown = 60 * 3;
                dash = false;
            }
        }
        
    }
}