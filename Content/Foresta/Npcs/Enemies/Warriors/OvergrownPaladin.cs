using System;
using Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors
{
    public class OvergrownPaladin : ModNPC
    {
        private bool chase;

        private bool grounded;

        private bool hitted;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 29;

            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.GivenName = "Overgrown Paladin";
            NPC.width = 50;
            NPC.height = 73;
            NPC.defense = 14;
            NPC.damage = 30;
            NPC.lifeMax = 170;
            NPC.value = ValueHelper.GetCoinValue(0, 0, 14, 7);
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.6f;

            //Banner = Type;
            //BannerItem = ModContent.ItemType<OvergrownWarriorBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(
                    "The knight may have thought he was right," +
                    "But his actions caused pain and plight. " +
                    "For protecting the wrong is not noble or true, " +
                    "Justice and honor demand what is due.")
            });
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
            chase = target.HasBuff<GreenMark>();

            grounded = NPC.velocity.Y == 0;

            if (!hitted)
            {
                if (target.HasBuff<GreenMark>())
                {
                    NPC.velocity.X += NPC.direction * 0.10f;
                    NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2, 2);
                }
                else
                {
                    NPC.velocity.X += NPC.direction * 0.10f;
                    NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1, 1);
                }


                if (NPC.collideX && grounded) NPC.velocity.Y -= 6;
            }
            else
            {
                if (NPC.collideX || NPC.collideY) hitted = false;
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
            target.AddBuff(ModContent.BuffType<GreenMark>(), 5 * 60);
            if (target.HasBuff<GreenMark>())
            {
                modifiers.FinalDamage.Flat *= 1.20f;
            }
        }

    }
}