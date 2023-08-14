using System;
using System.Collections.Generic;
using System.Linq;
using Crystals.Content.Foresta.Items.Weapons.Melee.Sunwirl;
using Crystals.Core;
using Crystals.Core.Systems.TrailSystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Sunny
{
    public class Sunny : ModNPC
    {
        public override string Texture => AssetDirectory.Sunny + Name;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.Size = new Vector2(30, 30);
            NPC.damage = 32;
            NPC.lifeMax = 80;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.defense = 2;
            NPC.knockBackResist = 0.0f;

            NPC.HitSound = SoundID.NPCDeath7;
            NPC.DeathSound = SoundID.NPCDeath2;

        }

        enum States
        {
            Idle,
            HalfCircle,
            SpinShot
        }

        public float Timer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private States state = States.Idle;

        public override void AI()
        {

            #region Detection

            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            #endregion

            #region Behaviour

            NPC.spriteDirection = NPC.direction = Math.Sign(target.Center.X - NPC.Center.X);

            PetalRot += 0.05f * NPC.spriteDirection;

            FaceRot = NPC.AngleTo(target.Center);

            if (target.dead)
            {
                NPC.EncourageDespawn(60);
            }

            Timer++;
            switch (state)
            {
                case States.Idle:
                    FaceFrame.Y = 0;
                    Idle();
                    if (Timer >= 180f)
                    {

                        if (NPC.Distance(target.Center) >= 800)
                        {
                            Timer = 0;
                            state = States.Idle;
                            NPC.ai[1] = 0;
                        }
                        else
                        {
                            Timer = 0;
                            state = States.HalfCircle;
                            NPC.ai[1] = 0;
                        }

                    }
                    break;
                case States.HalfCircle:
                    FaceFrame.Y = 62;
                    if (!charging)
                    {
                        StartCharge(NPC.Center, new Vector2(target.Bottom.X, target.Bottom.Y + target.height * 2), new Vector2(target.Bottom.X, target.Bottom.Y + target.height * 2), new Vector2(target.Center.X + NPC.direction * 150, target.Center.Y - 125));
                    }
                    else
                    {
                        Charge();
                    }
                    if (Timer >= 120f)
                    {
                        charging = false;
                        Timer = 0;
                        state = States.SpinShot;
                        NPC.ai[1] = 0;
                    }
                    break;
                case States.SpinShot:
                    FaceFrame.Y = 62 * 2;
                    SpinShot();
                    if (Timer >= 60f)
                    {
                        Timer = 0;
                        state = States.Idle;
                        NPC.ai[1] = 0;
                    }
                    break;
            }

            #endregion
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            if (spawnInfo.Player.ZoneForest)
            {
                return SpawnCondition.OverworldDay.Chance * 0.01f;
            }
            return 0;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new CommonDrop(ItemID.Sunflower, 10));
            npcLoot.Add(new CommonDrop(ModContent.ItemType<Sunwirl>(), 19));
        }

        private void Idle()
        {
            NPC.ai[1]++;
            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            NPC.velocity = NPC.DirectionTo(new Vector2(target.Center.X, target.Center.Y - 125));

            NPC.velocity.Y += MathFunctions.SineWave(2f, 1f, NPC.ai[1] / 15);

        }

        private bool charging = false;

        private Vector2 startPos;

        private Vector2 midPos1;

        private Vector2 midPos2;

        private Vector2 endPos;

        private void StartCharge(Vector2 startPos, Vector2 midPos1, Vector2 midPos2, Vector2 endPos)
        {
            this.startPos = startPos;
            this.midPos1 = midPos1;
            this.midPos2 = midPos2;
            this.endPos = endPos;
            charging = true;
        }

        private void Charge()
        {
            NPC.ai[1] += 1f / 120f;
            NPC.TargetClosest();
            var target = Main.player[NPC.target];
            Vector2 dest = new MathFunctions.BezierCurve(MathFunctions.EaseFunctions.EaseInOutBack(NPC.ai[1]), startPos, midPos1, midPos2, endPos).GetPoint();
            float speed = NPC.Distance(dest) * 0.125f;
            PetalRot += NPC.ai[1] * 0.2f;
            NPC.velocity = NPC.DirectionTo(dest) * speed;
        }

        private void SpinShot()
        {
            NPC.ai[1] += 1f / 60f;
            PetalRot = +MathHelper.SmoothStep(PetalRot, PetalRot += NPC.ai[1] * 0.25f * NPC.spriteDirection,
                MathFunctions.EaseFunctions.EaseInOutQuad(NPC.ai[1]));

            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            NPC.velocity = NPC.DirectionTo(new Vector2(target.Center.X, target.Center.Y - 75));
            if (Timer % 30 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {

                for (int i = 0; i < 4; i++)
                {
                    Dust.NewDustPerfect(PetalPos, DustID.YellowStarDust, Main.rand.NextVector2Circular(4, 4));
                }

                Projectile.NewProjectile(Terraria.Entity.GetSource_None(), PetalPos,
                    PetalPos.DirectionTo(target.Center) * 16f,
                    ModContent.ProjectileType<HostilePetal>(), NPC.damage / 2, 0);

                SoundEngine.PlaySound(SoundID.Item63 with { MaxInstances = 2 });
            }
        }

        public float PetalRot;

        public Vector2 PetalPos;

        public Rectangle PetalFrame = new Rectangle(0, 0, 62, 62);

        public Rectangle FaceFrame = new Rectangle(0, 0, 62, 62);

        public float FaceRot;

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Crystals/Content/Foresta/Npcs/Enemies/Sunny/SunnyPetals").Value;

            Vector2 drawOrigin = new Vector2(PetalFrame.Width * 0.5f, PetalFrame.Height * 0.5f);
            Vector2 drawPos = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY + 4);

            PetalPos = NPC.Top + new Vector2(0f, NPC.gfxOffY + 4);
            PetalPos.RotatedBy(PetalRot);

            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw(texture, drawPos, PetalFrame, Lighting.GetColor(PetalPos.ToTileCoordinates()), PetalRot, drawOrigin, NPC.scale - 0.20f, effects, 0);

            Texture2D textureFace = ModContent.Request<Texture2D>("Crystals/Content/Foresta/Npcs/Enemies/Sunny/SunnyFace").Value;

            spriteBatch.Draw(textureFace, drawPos, FaceFrame, Lighting.GetColor(PetalPos.ToTileCoordinates()), FaceRot + NPC.rotation, drawOrigin, NPC.scale, effects, 0);
        }

        public override void OnKill()
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDustPerfect(PetalPos, DustID.YellowStarDust, -NPC.velocity);
            }

            Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                ModContent.GoreType<FlowerGore>());

            for (int i = 0; i < 4; i++)
            {
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                    ModContent.GoreType<PetalGore>());
            }

            for (int i = 0; i < 2; i++)
            {
                Gore.NewGore(new EntitySource_Death(NPC), NPC.position, -NPC.velocity,
                    ModContent.GoreType<PetalGore2>());
            }

        }

        public class FlowerGore : ModGore
        {
            public override string Texture => AssetDirectory.Sunny + Name;
        }

        public class PetalGore : ModGore
        {
            public override string Texture => AssetDirectory.Sunny + Name;

            public override bool Update(Gore gore)
            {
                gore.velocity.Y *= 0.90f;
                return true;
            }
        }

        public class PetalGore2 : ModGore
        {
            public override string Texture => AssetDirectory.Sunny + Name;

            public override bool Update(Gore gore)
            {
                gore.velocity.Y *= 0.75f;
                return true;
            }
        }

        class HostilePetal : ModProjectile
        {
            public override string Texture => AssetDirectory.Sunny + Name;

            public override void SetDefaults()
            {
                Projectile.Size = new Vector2(14, 18);
                Projectile.ignoreWater = false;
                Projectile.tileCollide = true;
                Projectile.hostile = true;
            }

            public override void AI()
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                Projectile.ai[0] += 1f; // Use a timer to wait 15 ticks before applying gravity.
                if (Projectile.ai[0] >= 15f)
                {
                    Projectile.ai[0] = 15f;
                    Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
                }
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }

                Lighting.AddLight(Projectile.Center, .255f, .253f, .98f);

            }

            public PrimitiveTrail trail = new PrimitiveTrail();
            public List<Vector2> oldPositions = new List<Vector2>();
            public override bool PreDraw(ref Color lightColor)
            {

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                lightColor = Color.White;

                Color color = new Color(255, 253, 98);

                Vector2 pos = Projectile.Center.RotatedBy(Projectile.rotation, Projectile.Center);

                oldPositions.Add(pos);
                while (oldPositions.Count > 30)
                    oldPositions.RemoveAt(0);

                trail.Draw(color, pos, oldPositions, 1f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
                return true;
            }

            public override void Kill(int timeLeft)
            {
                Vector2 position = Projectile.position;
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, DustID.YellowStarDust,
                        Main.rand.NextVector2Circular(2, 2).X * 2,
                        Main.rand.NextVector2Circular(2, 2).Y * 2, 0,
                        new Color(255, 255, 255), 1f);
                }
            }

        }

    }
}