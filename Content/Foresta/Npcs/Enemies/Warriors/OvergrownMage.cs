using System;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors;

public class OvergrownMage : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 15;
    }

    private int xFrame = 0;

    private int yFrame;

    private Entity target;
    
    private bool grounded;
    
    public float Timer
    {
        get => NPC.ai[0];
        set => NPC.ai[0] = value;
    }
    
    enum States
    {
        Idle,
        Attack,
        Support
    }

    private States currentAttack = States.Idle;
    
    private int walkDir;
    
    public override void SetDefaults()
    {
        NPC.width = 60;
        NPC.height = 50;
        NPC.defense = 4;
        NPC.damage = 21;
        NPC.lifeMax = 120;
        NPC.value = ValueHelper.GetCoinValue(0, 0, 15, 89);
        NPC.aiStyle = -1;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath2;
        NPC.noGravity = false;
        NPC.noTileCollide = false;
        NPC.knockBackResist = 0.6f;
    }
    
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
            new FlavorTextBestiaryInfoElement(
                "With pure intentions once, their heart ablaze, " +
                "Using light magic to mend and amaze. Yet darkness seeped in, " +
                "and their judgment blurred, Now striking down the wrong, their light obscured.")
        });
    }
    
    public override void FindFrame(int frameHeight)
    {
        int frameWidth = 60;
        NPC.frame = new Rectangle(frameWidth * xFrame, frameHeight * yFrame, frameWidth, frameHeight);
        Main.NewText(yFrame);

        if (grounded)
        {
            if (NPC.velocity != Vector2.Zero)
            {
                switch (xFrame)
                {
                    case 0:
                        NPC.frameCounter++;

                        if (NPC.frameCounter % 4 == 0)
                            yFrame++;

                        if (yFrame == 15)
                        {
                            yFrame = 0;
                            xFrame = 0;
                            NPC.frameCounter = 0;
                        }
                        break;
                }
            }
        }else  yFrame = 0;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
        
        var origin = new Vector2(NPC.frame.Width / 2f, 50 / 2f);

        SpriteEffects effect = SpriteEffects.None;
        if (NPC.direction != -1)
        {
            effect = SpriteEffects.FlipHorizontally;
        }
        
        spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, origin, NPC.scale, effect, 0f);
        
        return false;
    }

    public override void AI()
    {
        #region Targeting
        
        NPC.TargetClosest();
        var player = Main.player[NPC.target];

        #endregion

        #region Behaviour
        
        NPC.spriteDirection = NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
        
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
            
            case States.Idle:
                xFrame = 0;
                target = player;
                Walk();
                if (Timer >= 180)
                {
                    Timer = 0;
                    currentAttack = States.Idle;
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
    
}