using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.IO;
using Crystals.Core.Systems.CameraShake;
using Terraria.Audio;

namespace Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium
{
    /// <summary>
    /// ai0 is the max charge, being how much time it take to charge the bow. It's similar to use time. Lower = faster.
    /// ai1 is how much the bow is charged by. It's a counter. Spawn it with higher than 0 to make it charge faster the first time.
    /// UNFINISHED!!!!!!
    /// UNFINISHED!!!!!!
    ///-Photonic0
    /// </summary>
    public class CrusoliumBowHeldProj : ModProjectile
    {
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overPlayers.Add(index);
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => false;
        //public override string Texture => "Crystals/Content/Foresta/Items/Weapons/Ranged/CrusoliumbowHeldProj";
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.Size = new Vector2(1);
        }
        public float MaxTimer { get => Projectile.ai[2]; set => Projectile.ai[2] = value; }//MIGHT NOT NEED THIS ACTUALLY
        public float ChargeAmount { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }

        public float MaxCharge { get => Projectile.ai[0]; }
        public float ChargeProgress { get => Projectile.ai[1] / Projectile.ai[0]; }
        public float ChargeProgressWithEasing { get => Helpers.EaseFunctions.EaseOutBack(Projectile.ai[1] / Projectile.ai[0]); }

        public List<short> pierceExceptions = new List<short>();

        public override void AI()
        {
            //UNFINISHED
            Player player = Main.player[Projectile.owner];
            
            pierceExceptions.AddRange( new []{ ProjectileID.JestersArrow ,ProjectileID.HellfireArrow });

            bool isHoldingBow = player.HeldItem.type == ModContent.ItemType<Crusolium_Bow>(); 
            
            if (isHoldingBow)
            {
                Projectile.timeLeft = 30; 
            }
            
            if (!isHoldingBow || player.dead || GetProjIDToShoot() == 0) 
                Projectile.Kill();
            if (!player.channel && ChargeAmount != 0)
            {
                int ammoIdToShoot = GetProjIDToShoot();
                if (ammoIdToShoot != 0)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                        Projectile.Center - Projectile.rotation.ToRotationVector2() * 4,
                        Projectile.Center.DirectionTo(Main.MouseWorld) * 10f * ChargeProgressWithEasing, ammoIdToShoot,
                        (int) (Projectile.damage * ChargeProgressWithEasing),
                        Projectile.knockBack * ChargeProgressWithEasing, Projectile.owner);
                    RemoveItemShot();
                    proj.netUpdate = true;
                    if ((ChargeAmount >= MaxCharge) && !pierceExceptions.Contains( (short) ammoIdToShoot))
                    {
                        proj.penetrate++;
                    }
                    
                    
                    
                    proj.usesLocalNPCImmunity = true;
                    proj.localNPCHitCooldown = 10;
                    
                    SoundEngine.PlaySound(SoundID.Item5 with { MaxInstances = 0 }, Projectile.position);
                    Shake.active = true;
                    Shake.power = 1;
                    Shake.time = 10;
                }
                
                ChargeAmount = 0;
            }
            
            if (ChargeAmount < MaxCharge && GetProjIDToShoot() != 0)
                ChargeAmount = !player.channel ? 0 : ChargeAmount + 1 > MaxCharge ? MaxCharge : ChargeAmount + 1;

            //Dust.NewDustPerfect(Projectile.Center, DustID.GreenFairy, Vector2.Zero);//DEBUG DUST
                
            if (Main.myPlayer == Projectile.owner)
                Projectile.ai[2] = ( Main.MouseWorld - player.Center).ToRotation();
            Projectile.netUpdate = true;
            Projectile.rotation = Projectile.ai[2];//multiplayer terribleness
            player.direction = (Projectile.rotation > -MathHelper.PiOver2 && Projectile.rotation < MathHelper.PiOver2) ? 1 : -1;
            Projectile.Center = player.Center - new Vector2(player.direction * 3,2) + new Vector2(23,0).RotatedBy(Projectile.rotation);
            Projectile.position.Y += player.gfxOffY;
            player.SetCompositeArmFront(true, ProgressToStretchAmount( 1 - ChargeProgressWithEasing), Projectile.rotation - MathF.PI / 2);
            
            if (ChargeAmount >= MaxCharge)
            {
                var power = 0.5f;
                Vector2 random = new Vector2(Main.rand.NextFloat(-power, power), Main.rand.NextFloat(-power, power));
                Projectile.position += random;
            }
            
        }

        void RemoveItemShot()
        {
            Item? itemShot = null;
            Player player = Main.player[Projectile.owner];
            bool chosen = false;
            
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].ammo == AmmoID.Arrow)
                {
                    
                    if (i > 53 && i < 58 && !player.inventory[i].IsAir)
                    {
                        itemShot = player.inventory[i];
                        break;
                    }
                    else if(!chosen)
                    {    
                        chosen = true;
                        itemShot = player.inventory[i];
                    }
                }
            }
            
            itemShot.stack--;
        }
        
        int GetProjIDToShoot()
        {
            int ammoIdToShoot = 0;
            Player player = Main.player[Projectile.owner];

            bool chosen = false;
            
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].ammo == AmmoID.Arrow)
                {
                    
                    if (i > 53 && i < 58 && !player.inventory[i].IsAir)
                    {
                        ammoIdToShoot = player.inventory[i].shoot;
                        break;
                    }
                    else if(!chosen)
                    {    
                        chosen = true;
                        ammoIdToShoot = player.inventory[i].shoot;
                    }
                }
            }
            ammoIdToShoot = ammoIdToShoot == ProjectileID.WoodenArrowFriendly ? ModContent.ProjectileType<CrusoliumArrow>() : ammoIdToShoot;
            return ammoIdToShoot;
        }
        /// <summary>
        /// </summary>
        /// <param name="controlPoints"> THIS NEEDS TO BE AT LEAST 4 IN LENGTH OR YOU'LL GET INDEX OUT OF BOUNDS!!! It won't factor in points after the 4th one</param>
        /// <param name="t"> from 0 to 1</param>
        /// <returns>returns an interpolatd point</returns>
        static Vector2 CubicBezier(Vector2[] controlPoints, float t)
        {
            float tSquared = t * t;
            float tCubed = t * t * t;
            return
                -(controlPoints[0] * (-tCubed + (3 * tSquared) - (3 * t) - 1) +
                controlPoints[1] * ((3 * tCubed) - (6 * tSquared) + (3 * t)) +
                controlPoints[2] * ((-3 * tCubed) + (3 * tSquared)) +
                controlPoints[3] * tCubed);
        }
        static Color ColorFunction(float progress) => (progress > 0.8f || progress < 0.2f) ? Color.LimeGreen : Color.YellowGreen;
        static float WidthFunction(float progress) => 4;
        public override bool PreDraw(ref Color lightColor)
        {
            //UNFINISHED
            Texture2D texture = TextureAssets.Projectile[ProjectileID.CultistBossLightningOrbArc].Value;
            Vector2[] bezierPoints = new Vector2[4];
            //List<Vector2> listForPoints = new();
            //List<float> listForRotations = new();
            float[] bezierXOffsets = new float[] { 0, -1, -1, 0 };
            for (float i = 0; i < 4; i++)
            {
                bezierPoints[(int)i] = new Vector2(bezierXOffsets[(int)i] * ChargeProgressWithEasing * -10 - 12, i * -11 -16).RotatedBy(Projectile.rotation) + Projectile.Center;
            }
            float maxPoints = 30;
            for (float i = 0; i < maxPoints; i++)//probably doesn't need to be this high but uh idk -Photonic0
            {
                //listForPoints.Add(CubicBezier(bezierPoints, (float)i / maxPoints));
                Main.EntitySpriteDraw(texture, CubicBezier(bezierPoints, (float)i / maxPoints) - Main.screenPosition, null, ColorFunction(i/maxPoints), 0, texture.Size() / 2, 0.1f, SpriteEffects.None);
                //listForRotations.Add((CubicBezier(bezierPoints, (float)(i + 0.1f) / maxPoints) - CubicBezier(bezierPoints, (float)(i - 0.1f) / maxPoints)).ToRotation());
            }
            //VertexStrip bowString = new();
            //bowString.PrepareStrip(listForPoints.ToArray(), listForRotations.ToArray(), ColorFunction, WidthFunction, -Main.screenPosition, includeBacksides: true);
            //bowString.DrawTrail();

            if (ChargeProgress > 0)
            {
                texture = TextureAssets.Projectile[GetProjIDToShoot()].Value;
                Vector2 extraOffset = new Vector2( 10 * ChargeProgressWithEasing - 8, 0).RotatedBy(Projectile.rotation);
                Projectile proj = new Projectile();
                proj.type = GetProjIDToShoot();
                Color color = proj.GetAlpha(Color.White);
                //I did this to compensate for arrows that are transparent like jester's and holy, so that they look like the actual projectile when released
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition - extraOffset, null, color, Projectile.rotation + MathF.PI / 2, texture.Size() / 2, 1, SpriteEffects.None);
            }
            texture = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        static Player.CompositeArmStretchAmount ProgressToStretchAmount(float progress)
        {
            if (progress < 0.25f)
                return Player.CompositeArmStretchAmount.None;
            if (progress < 0.5f)
                return Player.CompositeArmStretchAmount.Quarter;
            if (progress < 0.75f)
                return Player.CompositeArmStretchAmount.ThreeQuarters;
            return Player.CompositeArmStretchAmount.Full;
        }
    }
}
