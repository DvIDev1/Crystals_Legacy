using System;
using System.Collections.Generic;
using System.Linq;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Accessories.Crusolium;
using Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad;
using Crystals.Content.Foresta.Items.Consumables.Food.Salad;
using Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors
{
    public class OvergrownPaladin : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 29;

            var value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
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

        enum States
        {
            Defensive,
            Rush,
            Guard,
            Walk,
            Bash
        }

        public float Timer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private States currentAttack = States.Walk;

        private readonly List<NPC> warriors = new List<NPC>();

        private List<Vector2> warriorPositions = new List<Vector2>();

        private Entity target;

        private bool grounded;

        private bool bashing;

        private Vector2 bashPos;

        private int walkDir;

        private bool defensive;

        public override void AI()
        {
            #region Targeting

            NPC.TargetClosest();
            var player = Main.player[NPC.target];

            foreach (var npcs in Main.npc)
            {
                if (npcs.Distance(NPC.Center) <= 1000)
                {
                    if (NPCSets.OvergrownWarrior[npcs.type])
                    {
                        if (npcs.type != NPC.type)
                        {
                            warriors.Add(npcs);
                            warriorPositions.Add(npcs.Center);
                        }
                    }
                }
            }

            warriors.RemoveAll(npc => !npc.active);

            #endregion

            #region Behaviour

            if (currentAttack is not (States.Bash or States.Guard))
            {
                NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
            }

            #region Jumping

            grounded = NPC.velocity.Y == 0;

            if (NPC.collideX && grounded)
            {
                NPC.velocity.Y -= 6;
            }

            #endregion

            Timer++;
            switch (currentAttack)
            {
                case States.Walk:
                    target = player;
                    if (grounded)
                    {
                        Walk();
                    }
                    if (Timer >= 120f)
                    {
                        Timer = 0;

                        if (NPC.Distance(player.Center) <= 700)
                        {
                            currentAttack = States.Rush;
                        }
                        else currentAttack = States.Walk;
                    }

                    break;

                case States.Rush:
                    target = player;
                    Rush();
                    if (Timer >= 120f)
                    {
                        Timer = 0;
                        if (NPC.Distance(target.Center) <= 100)
                        {
                            currentAttack = States.Bash;
                        }
                        else
                        {
                            currentAttack = States.Defensive;
                        }
                    }
                    break;
                case States.Bash:
                    target = player;
                    if (!bashing)
                    {
                        StartBash(new Vector2(player.Center.X + 400 * Math.Sign(target.Center.X - NPC.Center.X) , player.Center.Y));
                    }else Bash();
                    if (Timer >= 120)
                    {
                        bashing = false;
                        Timer = 0;
                        if (NPC.Distance(player.Center) >= 500)
                        {
                            currentAttack = States.Walk;
                        }
                        else
                        {
                            currentAttack = States.Defensive;
                        }
                    }
                    break;
                case States.Defensive:
                    target = player;
                    Defensive();
                    if (Timer >= 180)
                    {
                        Timer = 0;
                        NPC.SuperArmor = false;
                        NPC.knockBackResist = 0.6f;
                        if (NPC.Distance(GetNearestWarrior()) <= 550)
                        {
                            currentAttack = States.Guard;
                        }
                        else
                        {
                            currentAttack = States.Walk;
                        }
                    }
                    break;
                case States.Guard:
                    target = player;
                    Guard();
                    if (Timer > 180)
                    {
                        Timer = 0;
                        NPC.SuperArmor = false;
                        NPC.knockBackResist = 0.6f;
                        if (NPC.Distance(target.Center) <= 200)
                        {
                            currentAttack = States.Bash;
                        }else if (GetNearestWarrior().Distance(target.Center) >= 700)
                        {
                            currentAttack = States.Walk;
                        }
                        else
                        {
                            currentAttack = States.Guard;
                        }
                    }
                    break;
            }

            #endregion
        }

        private void Walk()
        {
            walkDir = Math.Sign(target.Center.X - NPC.Center.X);

            NPC.velocity.X += walkDir * 0.05f;
            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2, 2);
        }

        private void Rush()
        {
            walkDir = Math.Sign(target.Center.X - NPC.Center.X);

            NPC.velocity.X += walkDir * 0.25f;
            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -4, 4);
        }

        private void StartBash(Vector2 bashPos)
        {
            bashing = true;
            this.bashPos = bashPos;
        }

        private void Bash()
        {
            
            NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
            walkDir = Math.Sign(target.Center.X - NPC.Center.X);

            NPC.velocity.X += NPC.DirectionTo(Vector2.Lerp(NPC.Center, bashPos,
                MathFunctions.EaseFunctions.EaseInBack(Timer / 120f))).X * NPC.Distance(bashPos) * 0.125f;

            NPC.velocity.X = MathFunctions.EaseFunctions.EaseInBack(Timer / 120f) < 0
                ? MathHelper.Clamp(NPC.velocity.X, -2, 2)
                : MathHelper.Clamp(NPC.velocity.X, -8, 8);

        }

        private void Defensive()
        {
            walkDir = Math.Sign(target.Center.X - NPC.Center.X);

            NPC.SuperArmor = true;
            NPC.knockBackResist = 0.3f;
            
            NPC.velocity.X += walkDir * 0.50f;
            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1, 1);
        }
        
        private void Guard()
        {
            if (!GetNearestWarrior().Equals(new Vector2(0 ,0)))
            {
                
                NPC.SuperArmor = true;
                NPC.knockBackResist = 0.3f;
                NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);
                int dir = Math.Sign(target.Center.X - GetNearestWarrior().X);
                Vector2 dest = new Vector2(GetNearestWarrior().X + 100 * dir , GetNearestWarrior().Y);
                walkDir = Math.Sign(dest.X - NPC.Center.X);
                    
                NPC.velocity.X += walkDir * 0.10f;
                if (NPC.Distance(dest) > 25)
                {
                    NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -3, 3);
                }
                else
                {
                    NPC.velocity.X = Vector2.Zero.X;
                }
                
            }
            else
            {
                Timer = 0;
                NPC.SuperArmor = false;
                NPC.knockBackResist = 0.6f;
                currentAttack = States.Walk;
            }
        }

        private Vector2 GetNearestWarrior()
        {
            if (warriors.Count != 0)
            {
                if (warriorPositions.Count != 0)
                {
                    return Pathfinding.GetNearestNPC(NPC, warriors).Center;
                }
            }
            return new Vector2(0, 0);
        }

        public override void FindFrame(int frameHeight)
        {
            if (grounded)
            {
                if (NPC.velocity != Vector2.Zero)
                {
                    var frame = (int) (NPC.frameCounter / 8.0);
                    switch (currentAttack)
                    {
                        
                        case States.Walk:
                            NPC.frameCounter += 1.0;
                            NPC.frame.Y = 1 + frame * frameHeight;
                            if (frame >= 15)
                            {
                                NPC.frame.Y = 2 * frameHeight;
                                NPC.frameCounter = 8 * 2;
                            }
                            break;
                        case States.Defensive:
                        case States.Bash:
                        case States.Rush:
                            NPC.frameCounter += 1.0;
                            NPC.frame.Y = 1 + frame * frameHeight;
                            if (frame <= 16 || frame >= Main.npcFrameCount[NPC.type] - 1)
                            {
                                NPC.frame.Y = 17 * frameHeight;
                                NPC.frameCounter = 8 * 17;
                            }
                            break;
                        case States.Guard:
                            if (NPC.velocity.X != 0)
                            {
                                NPC.frameCounter += 1.0;
                                NPC.frame.Y = 1 + frame * frameHeight;
                                if (frame <= 16 || frame >= Main.npcFrameCount[NPC.type] - 1)
                                {
                                    NPC.frame.Y = 17 * frameHeight;
                                    NPC.frameCounter = 8 * 17;
                                }
                            }
                            else
                            {
                                NPC.frame.Y = (Main.npcFrameCount[NPC.type] - 4) * frameHeight;
                                NPC.frameCounter = 8 * Main.npcFrameCount[NPC.type] - 4;
                            }
                            break;
                    }
                }
            }else
            {
                if (currentAttack != States.Guard)
                {
                    NPC.frame.Y = 0;
                }
                else
                {
                    NPC.frame.Y = (Main.npcFrameCount[NPC.type] - 4) * frameHeight;
                    NPC.frameCounter = 8*25;
                }
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<GreenMark>(), 5 * 60);
            if (target.HasBuff<GreenMark>())
            {
                modifiers.FinalDamage *= 1.20f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {

            switch (currentAttack)
            {
                case States.Rush:
                    if (NPC.life >= NPC.lifeMax / 4)
                    {
                        if (NPC.Distance(target.Center) <= 200)
                        {
                            Timer = 0;
                            bashing = false;
                            currentAttack = States.Bash;
                        }
                    }
                    else
                    {
                        currentAttack = States.Defensive;
                    }
                    break;
                case States.Bash:
                    bashing = false;
                    if (NPC.Distance(target.Center) >= 500)
                    {
                        currentAttack = States.Walk;
                    }
                    else
                    {
                        currentAttack = States.Defensive;
                    }
                    break;
            }
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (currentAttack is (States.Bash or States.Rush) && NPC.velocity != Vector2.Zero)
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
            }

            return true;
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
            for (var i = 0; i < Main.rand.Next(10) + 1; i++)
            {
                var d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Bone, -NPC.velocity.X * 2);
                d.noGravity = false;
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
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CrusoShield>(), 8));
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1)
                if (spawnInfo.Player.ZoneForest)
                    return SpawnCondition.OverworldNight.Chance * 0.25f;
            return base.SpawnChance(spawnInfo);
        }
        
    }
}