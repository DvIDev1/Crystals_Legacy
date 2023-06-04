using System;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Sunny
{
    public class Sunny : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.Size = new Vector2(48 ,48);
            NPC.damage = 32;
            NPC.lifeMax = 80;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.defense = 2;
            NPC.knockBackResist = 0.6f;
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
            
            Timer++;
            switch (state)
            {
                case States.Idle:
                    Idle();
                    if (Timer >= 180f)
                    {
                        Timer = 0;
                        state = States.HalfCircle;
                        NPC.ai[1] = 0;
                    }
                    break;
                case States.HalfCircle:
                    if (!charging)
                    {
                        StartCharge(NPC.Center , new Vector2(target.Bottom.X , target.Bottom.Y + target.height * 2) , new Vector2(target.Bottom.X , target.Bottom.Y + target.height*2), new Vector2(target.Center.X + NPC.direction * 150 , target.Center.Y - 250));
                    }
                    else
                    {
                        Charge();
                    }
                    if (Timer >= 120f)
                    {
                        charging = false;
                        Timer = 0;
                        state = States.Idle;
                        NPC.ai[1] = 0;
                    }
                    break;
            }

            #endregion
        }

        private void Idle()
        {
            NPC.ai[1]++;
            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            NPC.velocity = NPC.DirectionTo(new Vector2(target.Center.X , target.Center.Y - 250f));

            NPC.velocity.Y += MathFunctions.SineWave(2f, 1f, NPC.ai[1] / 15);
        }

        private bool charging = false;

        private Vector2 startPos;

        private Vector2 midPos1;

        private Vector2 midPos2;

        private Vector2 endPos;
        
        private void StartCharge(Vector2 startPos , Vector2 midPos1, Vector2 midPos2 , Vector2 endPos)
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
            Vector2 dest = new MathFunctions.BezierCurve(MathFunctions.EaseFunctions.EaseInOutBack(NPC.ai[1]) , startPos , midPos1 , midPos2 , endPos).GetPoint();
            float speed = NPC.Distance(dest) * 0.185f;
            NPC.velocity = NPC.DirectionTo(dest) * speed;
        }
        
    }
}