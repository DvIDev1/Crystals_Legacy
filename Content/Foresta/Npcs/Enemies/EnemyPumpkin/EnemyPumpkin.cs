using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Crystals.Content.Foresta.Items.Banners;
using Crystals.Core.Systems.CameraShake;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Crystals.Content.Foresta.Npcs.Enemies.EnemyPumpkin
{
    public class EnemyPumpkin : ModNPC
    {
        
        //ToDo Add Variants
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 21;
            
            NPCID.Sets.SpawnsWithCustomName[Type] = true;
                
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 3;
            NPC.damage = 26;
            NPC.lifeMax = 60;
            NPC.value = ValueHelper.GetCoinValue(0, 4, 2, 0);
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.8f;
            
            Banner = Type;
            BannerItem = ModContent.ItemType<EnemyPumpkinBanner>();
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(
                    "An Friendly Ally go near so it will help you!")
            });
        }
        
        
        
        public override List<string> SetNPCNameList()
        {
            List<string> names = new List<string>();
            names.AddRange( new []{ "Pumk" , "Pumpkin Patroller" , "Gerald" , "Walter"  , "Hugh" ,  
                "Hughie" ,  "Highie" , "Laboo" , "Pumpkin Grimace" , "Hap" , "Robert" , "Pumpkin Pal" ,  "Crawler pumpkin" ,
                "Foxy the Pirate Pumpkin" ,  "Squashler" ,  "Gex" ,  "Baleful Crop" ,  "Pimpnaut" , "Pumpkin" ,
                "Pummie" ,  "Pumpky Pie" , "Puma" , "S(cream)" , "Biden the Biteful Bait"
            });
            return names;
        }

        private bool active = false;

        private bool transforming = false;

        private bool hostile = false;

        private bool attacking = false;
        
        private Vector2 targetPos = Vector2.Zero;

        private Vector2 attackStartPos = Vector2.Zero;

        private bool stomp = false;

        private bool isFalling;

        public override void AI()
        {
            
            #region Player Detection

            NPC.TargetClosest();
            var target = Main.player[NPC.target];
            active = (transforming || attacking) && target.active && !target.dead;

            NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);

            #endregion

            #region Transformation

            if (active || NPC.Distance(target.Center) < 300f && !target.dead)
            {
                if (!transforming && !hostile)
                {
                    StartTransformation();
                }
            }
            else
            {
                hostile = false;
                NPC.rotation += NPC.velocity.X * 0.1f;
                if (NPC.velocity == Vector2.Zero)
                {
                    NPC.rotation = 0;
                }

                if (NPC.collideX)
                {
                    NPC.velocity = -NPC.velocity / 2f;
                }

            }
            
            
            #endregion

            #region Behaviour

            if (hostile)
            {

                if (!attacking)
                {
                    StartAttack(new Vector2( target.Center.X + target.velocity.X , (target.Center.Y - 200) + target.velocity.Y / 1.5f));
                }
                else
                {
                    if (!stomp)
                    {
                        targetPos = new Vector2(target.Center.X + target.velocity.X,
                            (target.Center.Y - 200) + target.velocity.Y);
                        FloatAttack();
                    }
                    else
                    {
                        if (!isFalling)
                        {
                            StartStompAttack(new Vector2(target.Center.X , NPC.Center.Y + 2000f));
                        }
                        else
                        {
                            StompAttack();
                            if (NPC.ai[0] > 1f || NPC.collideY)
                            {
                                isFalling = false;
                                NPC.noGravity = false;
                                stomp = false;
                                StartAttack(new Vector2( target.Center.X + target.velocity.X , (target.Center.Y - 200) + target.velocity.Y / 1.5f));
                                NPC.ai[0] = 0;
                            }

                            if (NPC.collideY)
                            {
                                if (Main.player.Any(player => player.Distance(NPC.Center) < 1000f))
                                {
                                    ShakeSystem.power = 10;
                                    ShakeSystem.MaxTime = 10;
                                    ShakeSystem.WeakShake = true;
                                }
                                VisualHelper.CreateGroundExplosion(NPC , 7 , 20, 20 , 0 ,10 , 5);
                                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                            }
                            
                        }
                    }
                }
                
            }

            #endregion

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ItemID.Pumpkin, 1, 1, 3));
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            StartTransformation();
            attacking = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            StartTransformation();
            attacking = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && spawnInfo.SpawnTileType == TileID.Grass) return SpawnCondition.Overworld.Chance * 0.01f;
            return base.SpawnChance(spawnInfo);
        }

        public void StompAttack()
        {
            if (NPC.ai[0] < 1f || NPC.collideY)
            {
                NPC.ai[0] += 0.02f;
                NPC.noGravity = true;
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, NPC.ai[0]);
                NPC.velocity +=
                    NPC.DirectionTo(
                        Vector2.Lerp(attackStartPos, targetPos, (float) EaseFunctions.easeInBack(NPC.ai[0]))) * 2.5f; 
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, 0f, 0f);
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -10f, Single.MaxValue);
            }
            else
            {
                isFalling = false;
                stomp = false;
                NPC.ai[0] = 0;
            }
        }

        public void StartStompAttack(Vector2 Position)
        {
            attacking = true;
            isFalling = true;
            targetPos = Position;
            NPC.ai[0] = 0;
            attackStartPos = NPC.Center; 
        }

        public void FloatAttack()
        {
            if (NPC.ai[0] < 1f)
            {
                NPC.rotation = NPC.velocity.X * 0.10f;
                NPC.ai[0] += 0.0075f;
                NPC.velocity +=
                    NPC.DirectionTo(Vector2.Lerp(attackStartPos, targetPos, 
                            (float) EaseFunctions.easeOutBack(NPC.ai[0])));
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, -15f, 15f);
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -15f, 15f);
            }
            else
            {
                stomp = true;
            }

        }

        private void StartAttack(Vector2 Position)
        {
            attacking = true;
            targetPos = Position;
            NPC.ai[0] = 0.95f;
            attackStartPos = NPC.Center;
        }

        private void StartTransformation()
        {
            transforming = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow",
                AssetRequestMode.ImmediateLoad).Value;
            SpriteEffects effect = SpriteEffects.None;
            
            if (NPC.spriteDirection != -1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.None;
            }
            
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    NPC.position.X - Main.screenPosition.X + NPC.frame.Width * 0.5f,
                    NPC.position.Y - Main.screenPosition.Y + NPC.frame.Height - NPC.frame.Height * 0.5f + 2f
                ),
                NPC.frame,
                Color.White,
                NPC.rotation,
                NPC.frame.Size() * 0.5f,
                NPC.scale,
                effect,
                0f
            );
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*Rectangle frame =
                texture.Frame(40 , 40  , 0, NPC.frame.Y);
            
            Main.NewText(frame);*/

            
            if (isFalling && NPC.velocity != Vector2.Zero)
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

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage += NPC.velocity.Length() * 0.05f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage += NPC.velocity.Length() * 0.05f;
        }

        public override void FindFrame(int frameHeight)
        {
            if (active)
            {
                if (transforming)
                {
                    NPC.frameCounter += 6.0;
                    float frame = (float) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = (int) frame * frameHeight;
                    if (frame >= 13)
                    {
                        transforming = false;
                        hostile = true;
                    }
                }else if (hostile && !stomp && !isFalling)
                {
                    NPC.frameCounter++;
                    int frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame <= 13 || frame >= 18)
                    {
                        NPC.frame.Y = 14 * frameHeight;
                        NPC.frameCounter = 8 * 14;
                    }
                }else if (isFalling)
                {
                    NPC.frameCounter++;
                    int frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame <= 18 || frame >= 21)
                    {
                        NPC.frame.Y = 19 * frameHeight;
                        NPC.frameCounter = 8 * 19;
                    }
                }
            }
            else
            {
                NPC.frame.Y =  frameHeight;
                NPC.frameCounter = 8;
            }
        }
        
        public override void OnKill()
        {
            for (var i = 0; i < Main.rand.Next(4) + 1; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, Main.rand.NextVector2Circular(-4 , 4),
                    GoreID.Smoke2);
        }
        
    }

    public class SquashlerAggresive : ModNPC
    {
        //ToDo Add Variants
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 21;
            
            NPCID.Sets.SpawnsWithCustomName[Type] = true;
                
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 6;
            NPC.damage = 26;
            NPC.lifeMax = 70;
            NPC.value = ValueHelper.GetCoinValue(0, 1,8, 7);
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.8f;
            
            Banner = Type;
            BannerItem = ModContent.ItemType<EnemyPumpkinBanner>();
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(
                    "An Friendly Ally go near so it will help you!")
            });
        }
        
        
        
        public override List<string> SetNPCNameList()
        {
            List<string> names = new List<string>();
            names.AddRange( new []{ "Pumk" , "Pumpkin Patroller" , "Gerald" , "Walter"  , "Hugh" ,  
                "Hughie" ,  "Highie" , "Laboo" , "Pumpkin Grimace" , "Hap" , "Robert" , "Pumpkin Pal" ,  "Crawler pumpkin" ,
                "Foxy the Pirate Pumpkin" ,  "Squashler" ,  "Gex" ,  "Baleful Crop" ,  "Pimpnaut" , "Pumpkin" ,
                "Pummie" ,  "Pumpky Pie" , "Puma" , "S(cream)" , "Biden the Biteful Bait"
            });
            return names;
        }

        private bool active = false;

        private bool transforming = false;

        private bool hostile = false;

        private bool attacking = false;
        
        private Vector2 targetPos = Vector2.Zero;

        private Vector2 attackStartPos = Vector2.Zero;

        private bool stomp = false;

        private bool isFalling;

        public override void AI()
        {
            
            #region Player Detection

            NPC.TargetClosest();
            var target = Main.player[NPC.target];
            active = (transforming || attacking) && target.active && !target.dead;

            NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);

            #endregion

            #region Transformation

            if (active || NPC.Distance(target.Center) < 300f && !target.dead)
            {
                if (!transforming && !hostile)
                {
                    StartTransformation();
                }
            }
            else
            {
                hostile = false;
                NPC.rotation += NPC.velocity.X * 0.1f;
                if (NPC.velocity == Vector2.Zero)
                {
                    NPC.rotation = 0;
                }

                if (NPC.collideX)
                {
                    NPC.velocity = -NPC.velocity / 2f;
                }

            }
            
            
            #endregion

            #region Behaviour

            if (hostile)
            {

                if (!attacking)
                {
                    StartAttack(new Vector2( target.Center.X + target.velocity.X , (target.Center.Y - 200) + target.velocity.Y));
                }
                else
                {
                    if (!stomp)
                    {
                        targetPos = new Vector2(target.Center.X + target.velocity.X,
                            (target.Center.Y - 200) + target.velocity.Y);
                        FloatAttack();
                    }
                    else
                    {
                        if (!isFalling)
                        {
                            StartStompAttack(new Vector2(target.Center.X , NPC.Center.Y + 2000f));
                        }
                        else
                        {
                            StompAttack();
                            if (NPC.ai[0] > 1f || NPC.collideY)
                            {
                                isFalling = false;
                                NPC.noGravity = false;
                                stomp = false;
                                StartAttack(new Vector2( target.Center.X + target.velocity.X , (target.Center.Y - 200) + target.velocity.Y));
                                NPC.ai[0] = 0;
                            }

                            if (NPC.collideY)
                            {
                                if (Main.player.Any(player => player.Distance(NPC.Center) < 1000f))
                                {
                                    ShakeSystem.power = 20;
                                    ShakeSystem.MaxTime = 10;
                                    ShakeSystem.WeakShake = true;
                                }
                                VisualHelper.CreateGroundExplosion(NPC , 14 , 40, 40 , 0 ,20 , 10);
                                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                            }
                            
                        }
                    }
                }
                
            }

            #endregion

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ItemID.Pumpkin, 1, 1, 3));
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            StartTransformation();
            attacking = true;
        }
        
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow",
                AssetRequestMode.ImmediateLoad).Value;
            SpriteEffects effect = SpriteEffects.None;
            
            if (NPC.spriteDirection != -1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.None;
            }
            
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    NPC.position.X - Main.screenPosition.X + NPC.frame.Width * 0.5f,
                    NPC.position.Y - Main.screenPosition.Y + NPC.frame.Height - NPC.frame.Height * 0.5f + 2f
                ),
                NPC.frame,
                Color.White,
                NPC.rotation,
                NPC.frame.Size() * 0.5f,
                NPC.scale,
                effect,
                0f
            );
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            StartTransformation();
            attacking = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && spawnInfo.SpawnTileType == TileID.Grass) return SpawnCondition.Overworld.Chance * 0.01f;
            return base.SpawnChance(spawnInfo);
        }

        public void StompAttack()
        {
            if (NPC.ai[0] < 1f || NPC.collideY)
            {
                NPC.ai[0] += 0.01f;
                NPC.noGravity = true;
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, NPC.ai[0]);
                NPC.velocity +=
                    NPC.DirectionTo(
                        Vector2.Lerp(attackStartPos, targetPos, (float) EaseFunctions.easeInBack(NPC.ai[0]))) * 2.5f; 
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, 0f, 0f);
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -10f, Single.MaxValue);
            }
            else
            {
                isFalling = false;
                stomp = false;
                NPC.ai[0] = 0;
            }
        }

        public void StartStompAttack(Vector2 Position)
        {
            attacking = true;
            isFalling = true;
            targetPos = Position;
            NPC.ai[0] = 0;
            attackStartPos = NPC.Center; 
        }

        public void FloatAttack()
        {
            if (NPC.ai[0] < 1f)
            {
                NPC.rotation = NPC.velocity.X * 0.10f;
                NPC.ai[0] += 0.0075f;
                NPC.velocity +=
                    NPC.DirectionTo(Vector2.Lerp(attackStartPos, targetPos, 
                            (float) EaseFunctions.easeOutBack(NPC.ai[0])));
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, -10f, 10f);
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -10f, 10f);
            }
            else
            {
                stomp = true;
            }

        }

        private void StartAttack(Vector2 Position)
        {
            attacking = true;
            targetPos = Position;
            NPC.ai[0] = 0.95f;
            attackStartPos = NPC.Center;
        }

        private void StartTransformation()
        {
            transforming = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*Rectangle frame =
                texture.Frame(40 , 40  , 0, NPC.frame.Y);
            
            Main.NewText(frame);*/

            
            if (isFalling && NPC.velocity != Vector2.Zero)
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

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage += NPC.velocity.Length() * 0.05f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage += NPC.velocity.Length() * 0.05f;
        }

        public override void FindFrame(int frameHeight)
        {
            if (active)
            {
                if (transforming)
                {
                    NPC.frameCounter += 6.0;
                    float frame = (float) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = (int) frame * frameHeight;
                    if (frame >= 13)
                    {
                        transforming = false;
                        hostile = true;
                    }
                }else if (hostile && !stomp && !isFalling)
                {
                    NPC.frameCounter++;
                    int frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame <= 13 || frame >= 18)
                    {
                        NPC.frame.Y = 14 * frameHeight;
                        NPC.frameCounter = 8 * 14;
                    }
                }else if (isFalling)
                {
                    NPC.frameCounter++;
                    int frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame <= 18 || frame >= 21)
                    {
                        NPC.frame.Y = 19 * frameHeight;
                        NPC.frameCounter = 8 * 19;
                    }
                }
            }
            else
            {
                NPC.frame.Y =  frameHeight;
                NPC.frameCounter = 8;
            }
        }
        
        public override void OnKill()
        {
            for (var i = 0; i < Main.rand.Next(4) + 1; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, Main.rand.NextVector2Circular(-4 , 4),
                    GoreID.Smoke2);
        }
    }

    public class SquashlerScary : ModNPC
    {
        //ToDo Add Variants
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 21;
            
            NPCID.Sets.SpawnsWithCustomName[Type] = true;
                
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.defense = 2;
            NPC.damage = 26;
            NPC.lifeMax = 50;
            NPC.value = ValueHelper.GetCoinValue(0, 6,9, 0);
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0.8f;
            
            Banner = Type;
            BannerItem = ModContent.ItemType<EnemyPumpkinBanner>();
        }
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(
                    "An Friendly Ally go near so it will help you!")
            });
        }
        
        
        
        public override List<string> SetNPCNameList()
        {
            List<string> names = new List<string>();
            names.AddRange( new []{ "Pumk" , "Pumpkin Patroller" , "Gerald" , "Walter"  , "Hugh" ,  
                "Hughie" ,  "Highie" , "Laboo" , "Pumpkin Grimace" , "Hap" , "Robert" , "Pumpkin Pal" ,  "Crawler pumpkin" ,
                "Foxy the Pirate Pumpkin" ,  "Squashler" ,  "Gex" ,  "Baleful Crop" ,  "Pimpnaut" , "Pumpkin" ,
                "Pummie" ,  "Pumpky Pie" , "Puma" , "S(cream)" , "Biden the Biteful Bait"
            });
            return names;
        }

        private bool active = false;

        private bool transforming = false;

        private bool hostile = false;

        private bool attacking = false;
        
        private Vector2 targetPos = Vector2.Zero;

        private Vector2 attackStartPos = Vector2.Zero;

        private bool stomp = false;

        private bool isFalling;

        public override void AI()
        {
            
            #region Player Detection

            NPC.TargetClosest();
            var target = Main.player[NPC.target];
            active = (transforming || attacking) && target.active && !target.dead;

            NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);

            #endregion

            #region Transformation

            if (active || NPC.Distance(target.Center) < 300f && !target.dead)
            {
                if (!transforming && !hostile)
                {
                    StartTransformation();
                }
            }
            else
            {
                hostile = false;
                NPC.rotation += NPC.velocity.X * 0.1f;
                if (NPC.velocity == Vector2.Zero)
                {
                    NPC.rotation = 0;
                }

                if (NPC.collideX)
                {
                    NPC.velocity = -NPC.velocity / 2f;
                }

            }
            
            
            #endregion

            #region Behaviour

            if (hostile)
            {

                if (!attacking)
                {
                    StartAttack(new Vector2( target.Center.X + target.velocity.X , (target.Center.Y - 200) + target.velocity.Y / 2f));
                }
                else
                {
                    if (!stomp)
                    {
                        targetPos = new Vector2(target.Center.X + target.velocity.X,
                            (target.Center.Y - 200) + target.velocity.Y);
                        FloatAttack();
                    }
                    else
                    {
                        if (!isFalling)
                        {
                            StartStompAttack(new Vector2(target.Center.X , NPC.Center.Y + 2000f));
                        }
                        else
                        {
                            StompAttack();
                            if (NPC.ai[0] > 1f || NPC.collideY)
                            {
                                isFalling = false;
                                NPC.noGravity = false;
                                stomp = false;
                                StartAttack(new Vector2( target.Center.X + target.velocity.X , (target.Center.Y - 200) + target.velocity.Y / 2f));
                                NPC.ai[0] = 0;
                            }

                            if (NPC.collideY)
                            {
                                if (Main.player.Any(player => player.Distance(NPC.Center) < 1000f))
                                {
                                    ShakeSystem.power = 5;
                                    ShakeSystem.MaxTime = 10;
                                    ShakeSystem.WeakShake = true;
                                }
                                VisualHelper.CreateGroundExplosion(NPC , 4 , 10, 10 , 0 ,5 , 5);
                                SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                            }
                            
                        }
                    }
                }
                
            }

            #endregion

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ItemID.Pumpkin, 1, 1, 3));
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            StartTransformation();
            attacking = true;
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            StartTransformation();
            attacking = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && spawnInfo.SpawnTileType == TileID.Grass) return SpawnCondition.Overworld.Chance * 0.01f;
            return base.SpawnChance(spawnInfo);
        }

        public void StompAttack()
        {
            if (NPC.ai[0] < 1f || NPC.collideY)
            {
                NPC.ai[0] += 0.025f;
                NPC.noGravity = true;
                NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, NPC.ai[0]);
                NPC.velocity +=
                    NPC.DirectionTo(
                        Vector2.Lerp(attackStartPos, targetPos, (float) EaseFunctions.easeInBack(NPC.ai[0]))) * 4f; 
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, 0f, 0f);
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -10f, 55);
            }
            else
            {
                isFalling = false;
                stomp = false;
                NPC.ai[0] = 0;
            }
        }

        public void StartStompAttack(Vector2 Position)
        {
            attacking = true;
            isFalling = true;
            targetPos = Position;
            NPC.ai[0] = 0;
            attackStartPos = NPC.Center; 
        }

        public void FloatAttack()
        {
            if (NPC.ai[0] < 1f)
            {
                NPC.rotation = NPC.velocity.X * 0.10f;
                NPC.ai[0] += 0.0075f;
                NPC.velocity +=
                    NPC.DirectionTo(Vector2.Lerp(attackStartPos, targetPos, 
                            (float) EaseFunctions.easeOutBack(NPC.ai[0])));
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, -20f, 20f);
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -20f, 20f);
            }
            else
            {
                stomp = true;
            }

        }
        
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Glow",
                AssetRequestMode.ImmediateLoad).Value;
            SpriteEffects effect = SpriteEffects.None;
            
            if (NPC.spriteDirection != -1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                effect = SpriteEffects.None;
            }
            
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    NPC.position.X - Main.screenPosition.X + NPC.frame.Width * 0.5f,
                    NPC.position.Y - Main.screenPosition.Y + NPC.frame.Height - NPC.frame.Height * 0.5f + 2f
                ),
                NPC.frame,
                Color.White,
                NPC.rotation,
                NPC.frame.Size() * 0.5f,
                NPC.scale,
                effect,
                0f
            );
        }

        private void StartAttack(Vector2 Position)
        {
            attacking = true;
            targetPos = Position;
            NPC.ai[0] = 0.95f;
            attackStartPos = NPC.Center;
        }

        private void StartTransformation()
        {
            transforming = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /*Rectangle frame =
                texture.Frame(40 , 40  , 0, NPC.frame.Y);
            
            Main.NewText(frame);*/

            
            if (isFalling && NPC.velocity != Vector2.Zero)
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

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage += NPC.velocity.Length() * 0.05f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage += NPC.velocity.Length() * 0.05f;
        }

        public override void FindFrame(int frameHeight)
        {
            if (active)
            {
                if (transforming)
                {
                    NPC.frameCounter += 6.0;
                    float frame = (float) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = (int) frame * frameHeight;
                    if (frame >= 13)
                    {
                        transforming = false;
                        hostile = true;
                    }
                }else if (hostile && !stomp && !isFalling)
                {
                    NPC.frameCounter++;
                    int frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame <= 13 || frame >= 18)
                    {
                        NPC.frame.Y = 14 * frameHeight;
                        NPC.frameCounter = 8 * 14;
                    }
                }else if (isFalling)
                {
                    NPC.frameCounter++;
                    int frame = (int) (NPC.frameCounter / 8.0);
                    NPC.frame.Y = frame * frameHeight;
                    if (frame <= 18 || frame >= 21)
                    {
                        NPC.frame.Y = 19 * frameHeight;
                        NPC.frameCounter = 8 * 19;
                    }
                }
            }
            else
            {
                NPC.frame.Y =  frameHeight;
                NPC.frameCounter = 8;
            }
        }
        
        public override void OnKill()
        {
            for (var i = 0; i < Main.rand.Next(4) + 1; i++)
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, Main.rand.NextVector2Circular(-4 , 4),
                    GoreID.Smoke2);
        }
    }
}