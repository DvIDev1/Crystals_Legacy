using System;
using System.Collections.Generic;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Items.Armors.Crusolium;
using Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad;
using Crystals.Content.Foresta.Items.Consumables.Food.Salad;
using Crystals.Core;
using Crystals.Core.Systems.SoundSystem;
using Crystals.Core.Systems.TrailSystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors;

public class OvergrownMage : ModNPC
{
    public override string Texture => AssetDirectory.Warriors + Name;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 20;
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
        Support,
    }

    private States currentAttack = States.Idle;

    private int walkDir;

    private bool castedSwords;

    public override void SetDefaults()
    {
        NPC.width = 60;
        NPC.height = 50;
        NPC.defense = 4;
        NPC.damage = 21;
        NPC.lifeMax = 50;
        NPC.value = ValueHelper.GetCoinValue(0, 0, 15, 89);
        NPC.aiStyle = -1;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath2;
        NPC.noGravity = false;
        NPC.noTileCollide = false;
        NPC.knockBackResist = 0.2f;
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

        if (grounded)
        {
            switch (xFrame)
            {
                case 0:
                    if (NPC.velocity != Vector2.Zero)
                    {
                        NPC.frameCounter++;

                        if (NPC.frameCounter % 5 == 0)
                            yFrame++;

                        if (yFrame == 15)
                        {
                            yFrame = 0;
                            xFrame = 0;
                            NPC.frameCounter = 0;
                        }
                    }

                    break;
                case 1:
                    NPC.frameCounter++;

                    if (NPC.frameCounter % 5 == 0)
                        yFrame++;

                    if (yFrame == 15)
                    {
                        yFrame = 0;
                        xFrame = 0;
                        NPC.frameCounter = 0;
                        currentAttack = States.Idle;
                        CastSwords();
                    }

                    break;
                
                case 2: 
                    NPC.frameCounter++;

                    if (NPC.frameCounter % 10 == 0)
                        yFrame++;

                    if (yFrame == 15)
                    {
                        yFrame = 12;
                        xFrame = 2;
                        NPC.frameCounter = 0;
                    }
                    break;
            }
        }
        else yFrame = 0;
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

        spriteBatch.Draw(tex, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, origin, NPC.scale, effect,
            0f);

        return false;
    }

    private int charge;

    private int maxCharge = 300;

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
 
        if (target != null)
        {
            NPC.direction = NPC.spriteDirection = Math.Sign(target.Center.X - NPC.Center.X);
        }
        else
        {
            NPC.direction = NPC.spriteDirection = Math.Sign(NPC.velocity.X - NPC.Center.X);
        }

        #region Jumping

        grounded = NPC.velocity.Y == 0;

        if (NPC.collideX && grounded)
        {
            NPC.velocity.Y -= 6;
        }

        #endregion

        if (charge >= maxCharge)
        {
            SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal , NPC.Center);
        }


        if (Main.rand.NextFloat() < 0.23f && charge >= maxCharge)
        {
            Vector2 pos = NPC.Center - new Vector2(0 , 75);
            Dust dust;
            Vector2 position = pos;
            dust = Dust.NewDustPerfect(pos, 107);
        }
        
        if (charge <= maxCharge)
        {
            charge++;
        }

        Timer++;
        switch (currentAttack)
        {
            case States.Idle:
                target = player;
                xFrame = 0;
                Walk();
                if (Timer >= 90)
                {
                    Timer = 0;
                    yFrame = 0;
                    NPC.frameCounter = 0;
                    if (NPC.Distance(target.Center) <= 500f && charge >= maxCharge )
                    {
                        currentAttack = States.Attack;
                    }else if(HasNearAllies())
                    {
                        if (GetNearestWarrior().life < GetNearestWarrior().lifeMax / 2 && GetNearestWarrior().active)
                        {
                            if (NPC.Distance(GetNearestWarriorPos()) <= 500f)
                            {
                                currentAttack = States.Support;
                            }
                        }
                    }
                    else currentAttack = States.Idle;
                }

                break;
            case States.Attack:
                xFrame = 1;
                target = player;
                NPC.velocity.X = 0;
                break;
            case States.Support:
                xFrame = 2;
                target = player;
                Support();
                if (Timer >= 360)
                {
                    Timer = 0;
                    yFrame = 0;
                    NPC.frameCounter = 0;
                    currentAttack = States.Idle;
                }

                break;
        }

        #endregion
    }

    private readonly List<NPC> warriors = new List<NPC>();

    private List<Vector2> warriorPositions = new List<Vector2>();
    
    private Vector2 GetNearestWarriorPos()
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

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (NPC.downedBoss1)
            if (spawnInfo.Player.ZoneForest)
                return SpawnCondition.OverworldNight.Chance;
        return base.SpawnChance(spawnInfo);
    }
    
    private NPC GetNearestWarrior()
    {
        if (warriors.Count != 0)
        {
            if (warriorPositions.Count != 0)
            {
                return Pathfinding.GetNearestNPC(NPC, warriors);
            }
        }

        return null;
    }
    
    public bool HasNearAllies()
    {
        return !GetNearestWarriorPos().Equals(new Vector2(0, 0));
    }
    
    private void Walk()
    {
        walkDir = Math.Sign(target.Center.X - NPC.Center.X);

        NPC.velocity.X += walkDir * 0.05f;
        NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2, 2);
    }

    private void CastSwords()
    {
        SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, NPC.Center);
        Vector2 pos =  NPC.Center - new Vector2(0 , 75);
        Projectile.NewProjectile(NPC.GetSource_FromAI(null),
            pos,
            pos.DirectionTo(target.Center) * 16f, ModContent.ProjectileType<MageSword>(), NPC.damage, 0.0f, -1);
        charge = 0;
    }

    private void Support()
    {
        NPC.velocity.X = 0; 
        if (Timer % 60 == 0 && !Main.dedServ)
        {
            foreach (var npcs in Main.npc)
            {
                if (NPCSets.OvergrownWarrior[npcs.type])
                {
                    if (NPC.Distance(npcs.Center) <= 500f)
                    {
                        npcs.life += 10;
                        npcs.HealEffect(10);
                        for (int i = 0; i < Main.rand.Next(5 , 12); i++)
                        {
                            Dust.NewDustPerfect(npcs.Center, DustID.GreenTorch, Main.rand.NextVector2Circular(2 , 2));
                        }
                    }
                }
            }
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
    }

    class MageSword : ModProjectile
    {
        public override string Texture => AssetDirectory.Warriors + Name;

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(22, 48);
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 125;
        }

        private Vector2 startPos;

        public override void AI()
        {
            Projectile.ai[0] += 1f / 125f;
            
            Lighting.AddLight(Projectile.Center , TorchID.Green);
            
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            
        }
        
        public PrimitiveTrail trail = new PrimitiveTrail();
        public List<Vector2> oldPositions = new List<Vector2>();
        public override bool PreDraw(ref Color lightColor)
        {
                
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null , null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            lightColor = Color.White;

            Color color = Color.LightGreen;

            Vector2 pos = (Projectile.Center).RotatedBy(Projectile.rotation, Projectile.Center);

            oldPositions.Add(pos);
            while (oldPositions.Count > 30)
                oldPositions.RemoveAt(0);

            trail.Draw(color, pos, oldPositions, 1.4f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return true;
        }
        
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(5 , 12); i++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch, Main.rand.NextVector2Circular(2 , 4));
            }
            SoundEngine.PlaySound(SoundID.DD2_PhantomPhoenixShot with {Volume = 2.5f}, Projectile.Center);
        }
    }
}