using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Sunny
{
    public class Sunny : ModNPC
    {
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

            Timer++;
            switch (state)
            {
                case States.Idle:
                    Idle();
                    if (Timer >= 180f)
                    {
                        Timer = 0;
                        state = States.Idle;
                    }
                    break;
            }

            #endregion
        }

        public void Idle()
        {
            NPC.ai[1]++;
            NPC.TargetClosest();
            var target = Main.player[NPC.target];

            NPC.velocity = NPC.DirectionTo(new Vector2(target.Center.X , target.Center.Y - 250f));

            NPC.velocity.Y += MathFunctions.SineWave(2f, 1f, NPC.ai[1] / 15);
        }
        
    }
}