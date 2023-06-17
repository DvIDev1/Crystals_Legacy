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
            NPC.height = 54;
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
                    "He guarded the wrong people with care," +
                    "Unaware of the harm they caused out there." +
                    "The people outside cried out in fear," +
                    "Their calls for justice fell on deaf ears.")
            });
        }

        private float MaxStamina = 200;
        
        private float stamina = 200;

        private bool charging;

        private float MaxChargeTime = 60*2.5f;

        public float ChargeTime
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            var target = Main.player[NPC.target];
            if (!target.dead && target.active)
            {
                if (!charging)
                {
                    NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
                }
            }
            else if (NPC.Distance(target.Center) >= 1000)
                NPC.EncourageDespawn(5 * 60);
            else
                NPC.DiscourageDespawn(6 * 60);
            chase = target.HasBuff<GreenMark>();

            grounded = NPC.velocity.Y == 0;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (stamina >= 0 && ChargeTime == 0 && NPC.Distance(target.Center) < 400)
            {
                charging = true;
            }
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            else if (stamina == 0 || ChargeTime >= MaxChargeTime)
            {
                charging = false;
            }

            if (stamina <= MaxStamina && !charging) stamina += 0.05f;

            if (!hitted)
            {
                if (!charging)
                {
                    NPC.SuperArmor = false;
                    if (ChargeTime - 1 >= 0)
                    {
                        ChargeTime--;
                    }

                    if (target.HasBuff<GreenMark>())
                    {
                        NPC.velocity.X += NPC.direction * 0.10f;
                        NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, 2 * NPC.direction, 2 * NPC.direction);
                    }
                    else
                    {
                        NPC.velocity.X += NPC.direction * 0.10f;
                        NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, 1 * NPC.direction, 1 * NPC.direction);
                    }
                }
                else
                {
                    ChargeTime++;
                    NPC.SuperArmor = true;
                    if (stamina - 0.4f <= 0)
                    {
                        stamina = 0;
                    }
                    else
                    {
                        stamina -= 0.4f;
                    }
                    if (target.HasBuff<GreenMark>())
                    {
                        NPC.velocity.X += NPC.direction * 0.10f;
                        NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, 7 * NPC.direction, 7 * NPC.direction);
                    }
                    else
                    {
                        NPC.velocity.X += NPC.direction * 0.10f;
                        NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, 5 * NPC.direction, 5 * NPC.direction);
                    }
                }


                if (NPC.collideX && grounded) NPC.velocity.Y -= 6;
            }
            else
            {
                if (NPC.collideX || NPC.collideY) hitted = false;
            }

        }

        public override void FindFrame(int frameHeight)
        {
            if (charging)
            {
                NPC.frameCounter++;
                int frame = (int)(NPC.frameCounter / 8.0);
                NPC.frame.Y = frame * frameHeight;
                if (frame < 16 || frame > Main.npcFrameCount[NPC.type] - 1)
                {
                    NPC.frame.Y = 16 * frameHeight;
                    NPC.frameCounter = 8 * 16;
                }
            }else {
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
        }

        public void DisplayBlocked()
        {
            CombatText.NewText(NPC.Hitbox, Color.Gray, "Blocked", true);
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (charging)
            {
                if (modifiers.CritDamage.Base == 0  || Main.rand.Next((int)modifiers.FinalDamage.Base) + 1 > 15)
                {
                    DisplayBlocked();
                }
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (charging)
            {
                if (modifiers.CritDamage.Base == 0 || Main.rand.Next((int)modifiers.FinalDamage.Base) + 1 > 15 || modifiers.DamageType == DamageClass.Magic || modifiers.DamageType == DamageClass.Summon)
                {
                    DisplayBlocked();
                }
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
                modifiers.FinalDamage *= 1.20f;
            }
        }

    }
}