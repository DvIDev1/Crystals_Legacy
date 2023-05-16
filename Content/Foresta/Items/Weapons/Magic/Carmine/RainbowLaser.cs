using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;

namespace MasterMasterMode.Projectiles
{
    public struct RainbowLaserConnector
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }
        
        /// <summary>
        /// a float that determines where in the line the rotation center is, using lerp
        /// </summary>
        public float RotationCenter { get; set; }
        public float Opacity { get; set; }
        /// <summary>
        /// X is the length, Y is the width
        /// </summary>
        public float Width { get; set; }
        public RainbowLaserConnector(Vector2 start, Vector2 end, float width, float opacity = 1 )
        {
            Start = start;
            End = end;
            Width = width;
            Opacity = opacity;
            RotationCenter = 0.5f;
        }
        public void DrawLaserConnector()
        {
            Texture2D texture = ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Shaders/Misc/RainbowLaser5").Value;
            Texture2D textureStart = ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Shaders/Misc/RainbowLaser5Start").Value;
            Vector2 drawScale = new Vector2(((End - Start).Length() - 16) * 0.001953125f, Width);// 1/512, 512 = tex width
            Vector2 origin = Vector2.Lerp(new Vector2(0, texture.Height * 0.5f), new Vector2(texture.Width, texture.Height * 0.5f), RotationCenter * 0.5f);//idk why but if I don't halve this it extrapolates
            float rotation = (End - Start).ToRotation();
            Vector2 drawPos = Start - Main.screenPosition + (End - Start) * 0.25f;
            Main.EntitySpriteDraw(texture, drawPos + new Vector2(4,0).RotatedBy(rotation), null, Color.White, rotation, 
                origin, drawScale, SpriteEffects.None , 0);
            Vector2 drawPosEdge = Start - Main.screenPosition - new Vector2(8,0).RotatedBy(rotation);
            Vector2 edgeDrawScale = new Vector2(0.001953125f * 32, Width);
            Main.EntitySpriteDraw(textureStart, drawPosEdge, null, Color.White, rotation, textureStart.Size() / 2, edgeDrawScale, SpriteEffects.None , 0);
            drawPosEdge = End - Main.screenPosition + new Vector2(8, 0).RotatedBy(rotation);
            Main.EntitySpriteDraw(textureStart, drawPosEdge, null, Color.White, rotation + MathF.PI, textureStart.Size() / 2, edgeDrawScale, SpriteEffects.FlipVertically , 0);
        }
        public Vector2 GetRotationCenterCoords()
        {
            return Vector2.Lerp(Start, End, RotationCenter);
        }
    }
    public class RainbowLaser : ModProjectile
    {
        public override string Texture => "Crystals/Assets/Foresta/Shaders/Misc/RainbowLaser10Start";
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        static float EaseBackOut(float progress)
        {
            float returnValue = (1 - (MathF.Cos(progress * MathF.PI * 1.359f) * 0.5f + 0.5f)) * 1.4f;;
            return returnValue;
        }
        static float EaseInOutSine(float progress)
        {
            return -(MathF.Cos(MathF.PI * progress) - 1) * 0.5f;
        }
        RainbowLaserConnector[] connectorsOct = new RainbowLaserConnector[16];
        public override void SetDefaults()
        {
            Projectile.hide = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 590;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
        }
        public override void AI()
        {
            Projectile.ai[0]+= 1f;
            Player owner = Main.player[Projectile.owner];
            if (owner.dead || !owner.channel)
            {
                owner.itemTime = owner.itemAnimation = 0;
                Projectile.Kill();
            }
                
            switch ((int)Projectile.ai[0])
            {
                case 20:
                    SoundEngine.PlaySound(
                        new SoundStyle("Crystals/Sounds/Items/Carmine/se_boss_kiila_crossbomb_return")
                            .WithVolumeScale(1.1f));
                    break;
                case 80:
                    SoundEngine.PlaySound(new SoundStyle("Crystals/Sounds/Items/Carmine/se_boss_kiila_spear_ready"));
                    break;
                case 160:
                    SoundEngine.PlaySound(
                        new SoundStyle("Crystals/Sounds/Items/Carmine/se_boss_kiila_threat_eye_extinction")
                            .WithVolumeScale(0.3f));
                    break;
                case 200:
                    SoundEngine.PlaySound(new SoundStyle("Crystals/Sounds/Items/Carmine/se_boss_kiila_straightbullet_ready"));
                    break;
                case 340:
                    SoundEngine.PlaySound(
                        new SoundStyle("Crystals/Sounds/Items/Carmine/se_boss_kiila_energybullet_landing")
                            .WithVolumeScale(2).WithPitchOffset(-.95f)); //with { Volume = 2, Pitch = -0.95f, MaxInstances = 0 });
                    SoundEngine.PlaySound(
                        new SoundStyle("Crystals/Sounds/Items/Carmine/se_boss_kiila_energybullet_landing")
                            .WithVolumeScale(2).WithPitchOffset(-.95f)); //with { Volume = 2, Pitch = -0.95f, MaxInstances = 0 });
                    break;          
            }
            Projectile.velocity = Main.MouseWorld - Main.player[Projectile.owner].Center;
            Projectile.velocity.Normalize();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 rotationVec = Projectile.ai[0] < 400 ? new Vector2(0.5f,-1) : Projectile.velocity;
          
                Main.player[Projectile.owner].itemRotation = (rotationVec * Main.player[Projectile.owner].direction).ToRotation();
            
            Projectile.Center = Main.player[Projectile.owner].Center - Projectile.velocity;
            //if(Projectile.ai[0] < 380)
            //Projectile.Center = Main.GetPlayerArmPosition(Projectile);

        }
        static Effect ConnectorShader;
        static Effect LaserShader;
        public override bool PreDraw(ref Color lightColor)
        {
            LoadShaderVars(out int laserWidth, out Vector2 scale, out Vector2 origin, out float increment, out Texture2D tex2, out Texture2D tex2Start);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ApplyConnectorShader();
            for (int i = 0; i < connectorsOct.Length; i++)
            {
                if (connectorsOct[i].Width < float.Epsilon)//I can't make it null if I want to set the values for some reason ugh
                    connectorsOct[i] = new RainbowLaserConnector(Projectile.Center, Projectile.Center, 0.035f);
            }
            float progress = Projectile.ai[0] / 40;
            if (Projectile.ai[0]  < 400)
            {
                float testRotMult = 1; 
                float t = EaseInOutSine(progress % 1);
                float connectorDist = 100;
                float connectorOffsetDist = 0;
                float globalScale = 1;
                float globalRotationPrimary = 0;
                float globalRotationSecondary = 0;
                float globalRotationFirstStar = 0;
                float globalRotationSecondStar = 0;
                float rotation = 0;
                switch (progress)
                {
                    case < 1:
                        connectorOffsetDist = MathHelper.Lerp(0, 0, t);
                        connectorDist = MathHelper.Lerp(0, 100, t);
                        break;
                    case < 2:
                        connectorOffsetDist = -MathHelper.Lerp(0, 114, t);
                        connectorDist = MathHelper.Lerp(100, 210, t);
                        break;
                    case < 3:
                        connectorOffsetDist = -114;
                        connectorDist = 210;
                        globalScale = MathHelper.Lerp(1, 2, t);
                        globalRotationPrimary = Utils.AngleLerp(0, MathF.PI * 0.25f * testRotMult - MathF.Tau * 1.5f, t);
                        globalRotationSecondary = Utils.AngleLerp(0, -MathF.PI * 0.25f * testRotMult + MathF.Tau * 1.5f, t);
                        break;
                    case < 4:
                        connectorOffsetDist = -MathHelper.Lerp(114, 228, t);
                        connectorDist = MathHelper.Lerp(210, 210, t);
                        globalScale = 2;
                        globalRotationPrimary = MathF.PI * 0.25f * testRotMult;
                        globalRotationSecondary = -MathF.PI * 0.25f * testRotMult;
                        break;
                    case < 5:
                        connectorOffsetDist = -MathHelper.Lerp(228, 228, t);
                        connectorDist = MathHelper.Lerp(210, 210, t);
                        globalScale = 2;
                        globalRotationPrimary = Utils.AngleLerp(MathF.PI * 0.25f * testRotMult, MathF.PI * 0.125f * testRotMult - MathF.Tau, t);
                        globalRotationSecondary = Utils.AngleLerp(-MathF.PI * 0.25f * testRotMult, -MathF.PI * 0.375f * testRotMult + MathF.Tau, t);
                        break;
                    case < 6:
                        connectorOffsetDist = -MathHelper.Lerp(228, 228, t);
                        connectorDist = MathHelper.Lerp(210, 210, t);
                        globalScale = 2;
                        globalRotationPrimary = MathF.PI * 0.125f * testRotMult;
                        globalRotationSecondary = -MathF.PI * 0.375f * testRotMult;
                        globalRotationFirstStar = Utils.AngleLerp(0, MathF.PI * 0.125f * testRotMult, t);
                        globalRotationSecondStar = Utils.AngleLerp(0, -MathF.PI * 0.125f * testRotMult, t);
                        break;
                    case < 7:
                        connectorOffsetDist = -MathHelper.Lerp(228, 228, t);
                        connectorDist = MathHelper.Lerp(210, 210, t);
                        globalScale = MathHelper.Lerp(2, 3, t);
                        globalRotationPrimary = Utils.AngleLerp(MathF.PI * 0.125f * testRotMult, MathF.PI * 0.125f * testRotMult, t);
                        globalRotationSecondary = Utils.AngleLerp(-MathF.PI * 0.375f * testRotMult, MathF.PI * 0.125f * testRotMult, t);
                        globalRotationFirstStar = Utils.AngleLerp(MathF.PI * 0.125f * testRotMult, -MathF.PI * 0.125f * testRotMult, t);
                        globalRotationSecondStar = globalRotationFirstStar;
                        break;
                    case < 8:
                        connectorOffsetDist = -MathHelper.Lerp(228, 0, t);
                        connectorDist = MathHelper.Lerp(210, 100, t);
                        globalScale = MathHelper.Lerp(3, 1, t);
                        globalRotationPrimary = MathF.PI * 0.125f * testRotMult;
                        globalRotationSecondary = MathF.PI * 0.125f * testRotMult;
                        globalRotationFirstStar = -MathF.PI * 0.125f * testRotMult;
                        globalRotationSecondStar = globalRotationFirstStar;
                        break;
                    case <= 9:
                        connectorOffsetDist = -MathHelper.Lerp(0, 0, t);
                        connectorDist = MathHelper.Lerp(100, 0, t);
                        globalScale = MathHelper.Lerp(1, 1, t);
                        globalRotationPrimary = Utils.AngleLerp(MathF.PI * 0.125f * testRotMult, 0, t);
                        globalRotationSecondary = Utils.AngleLerp(MathF.PI * 0.125f * testRotMult, 0, t);
                        globalRotationFirstStar = Utils.AngleLerp(-MathF.PI * 0.125f * testRotMult, 0, t);
                        globalRotationSecondStar = globalRotationFirstStar;
                        break;
                }      
                if(progress < 9)
                    for (float i = 0; i < connectorsOct.Length; i+= 1)
                    {
                        connectorsOct[(int)i].RotationCenter = 0.5f;
                        Vector2 flatOffset = new Vector2(connectorOffsetDist * globalScale,0).RotatedBy(i / (float)(connectorsOct.Length / 2) * MathF.Tau)
;                       Vector2 endPos = new Vector2(connectorDist * globalScale, 0).RotatedBy(((i - 0.5f)/(float)(connectorsOct.Length / 2)) * MathF.Tau) + Projectile.Center;
                        Vector2 startPos = new Vector2(connectorDist * globalScale, 0).RotatedBy((i + 0.5f)/(float)(connectorsOct.Length / 2) * MathF.Tau) + Projectile.Center;
                        Vector2 rotationCenter = Vector2.Lerp(startPos, endPos, connectorsOct[(int)i].RotationCenter * 0.5f + 0.25f);
                        float whichRotationToUse = i % 2 == 0 ? globalRotationPrimary : globalRotationSecondary;
                        float whichStarRotationToUse = i < connectorsOct.Length / 2 ? globalRotationFirstStar : globalRotationSecondStar;
                        connectorsOct[(int)i].Start = (startPos.RotatedBy(rotation, rotationCenter) + flatOffset).RotatedBy(whichRotationToUse + whichStarRotationToUse, Projectile.Center);
                        connectorsOct[(int)i].End = (endPos.RotatedBy(rotation, rotationCenter) + flatOffset).RotatedBy(whichRotationToUse + whichStarRotationToUse, Projectile.Center);
                    }               
            }

            if (Projectile.ai[0] / 40 < 9)
            {
                for (int i = 0; i < connectorsOct.Length; i++)
                {
                    if (progress > 8.5f)
                        connectorsOct[i].Opacity = 1 - (progress - 8.5f);
                    connectorsOct[i].DrawLaserConnector();
                }
            }
            if (progress < 8.7f)        
                return false;

            scale.Y = 0.5f * EaseBackOut(MathHelper.Clamp((progress - 8.7f) * 4, 0, 1));
            if(Projectile.timeLeft < 25)
            {
                scale.Y = 0.5f * (0.5f + 0.5f * EaseInOutSine((float)Projectile.timeLeft / 25f));
                Projectile.Opacity = EaseInOutSine((float)Projectile.timeLeft / 25f);
            }
            Vector2 drawOffset = Projectile.rotation.ToRotationVector2() * 130;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ApplyShaders();
            Main.EntitySpriteDraw(tex2Start, Projectile.Center - Main.screenPosition + Projectile.rotation.ToRotationVector2() * increment * 0.5f - drawOffset, null, Color.White, Projectile.rotation, origin, Projectile.scale * scale, SpriteEffects.None, 0);
            for (int i = (int)(increment * 1.5f); i < laserWidth; i += (int)(increment))
            {
                Main.EntitySpriteDraw(tex2, Projectile.Center - Main.screenPosition + new Vector2(i, 0).RotatedBy(Projectile.rotation) - drawOffset, null, Color.White, Projectile.rotation, origin, Projectile.scale * scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            tex2 = ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Textures/Weapons/Carmine/CarmineUse", AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(tex2, Projectile.Center + Projectile.velocity * 30 - Main.screenPosition, null, Color.White, Projectile.rotation, tex2.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;

          
        }
        void ApplyConnectorShader()
        {
            ConnectorShader.Parameters["h"].SetValue((float)(Main.timeForVisualEffects / 6));
            ConnectorShader.Parameters["uOpacity"].SetValue(Projectile.Opacity * 0.6f);
            ConnectorShader.Parameters["s"].SetValue(1);
            ConnectorShader.Parameters["l"].SetValue(0.7f);
            ConnectorShader.Parameters["gradientScale"].SetValue(2);
            ConnectorShader.Parameters["lightnessFromTexture"].SetValue(0.4f);
            ConnectorShader.CurrentTechnique.Passes[0].Apply();
        }
        void ApplyShaders()
        {
            float testSpeedMult = 0.9f;
            Texture2D erosion2 = TextureAssets.Extra[ExtrasID.FlameLashTrailShape].Value;
            Texture2D erosion = TextureAssets.Extra[ExtrasID.MagicMissileTrailErosion].Value;
            float lightnessFromNoiseTexture = (2f + 4f * MathF.Pow(MathF.Sin((float)Main.timeForVisualEffects * 0.15f * testSpeedMult) * 0.6f + 0.6f, 2f));
            LaserShader.Parameters["h"].SetValue((float)(Main.timeForVisualEffects / 6 * testSpeedMult));
            LaserShader.Parameters["uOpacity"].SetValue(Projectile.Opacity);
            LaserShader.Parameters["s"].SetValue(1);
            LaserShader.Parameters["l"].SetValue(0.7f);
            LaserShader.Parameters["gradientScale"].SetValue(10);
            LaserShader.Parameters["noiseScroll"].SetValue(new Vector2((float)Main.timeForVisualEffects * -0.0845f * testSpeedMult % 1, 0));
            LaserShader.Parameters["noiseScroll2"].SetValue(new Vector2((float)Main.timeForVisualEffects * -0.06f * testSpeedMult % 1, 1));
            LaserShader.Parameters["uImage1"].SetValue(erosion);
            LaserShader.Parameters["uImage2"].SetValue(erosion2);
            LaserShader.Parameters["noiseScale"].SetValue(new Vector2(1));
            LaserShader.Parameters["noiseScale2"].SetValue(new Vector2(1, 0.3f));
            LaserShader.Parameters["lightnessFromTexture"].SetValue(0.1f);
            LaserShader.Parameters["lightnessFromNoiseTexture"].SetValue(lightnessFromNoiseTexture);
            LaserShader.CurrentTechnique.Passes[0].Apply();
        }
        static void LoadShaderVars(out int laserWidth, out Vector2 scale, out Vector2 origin, out float increment, out Texture2D tex2, out Texture2D tex2Start)
        {
            tex2 = ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Shaders/Misc/RainbowLaser10").Value;
            tex2Start = ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Shaders/Misc/RainbowLaser10Start").Value;
            if (LaserShader == null)//do this so you don't need to reload the shader every frame           
                LaserShader = ModContent.Request<Effect>("Crystals/Assets/Foresta/Shaders/Misc/RainbowLaser", AssetRequestMode.ImmediateLoad).Value;
            if (ConnectorShader == null)//do this so you don't need to reload the shader every frame
                ConnectorShader = ModContent.Request<Effect>("Crystals/Assets/Foresta/Shaders/Misc/HslScroll", AssetRequestMode.ImmediateLoad).Value;
            int laserSegments = 30;
            laserWidth = tex2.Width * laserSegments;
            scale = new Vector2(0.5f, 0.5f);
            origin = tex2.Size() / 2;          
            increment = MathHelper.Clamp(tex2.Width * scale.X, 1, 10000);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //uhhh old laser collision fix start laser hitbox 
            if (Projectile.ai[0] < 350 )
                return false;
            float collisionPoint = 86555;
            bool collided = false;
            Vector2 laserPoint = Projectile.Center + Projectile.velocity * 64;
            if(!collided)
                collided = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + (Projectile.velocity) * 200, Projectile.velocity * 6000 + Projectile.Center, 192, ref collisionPoint);
            if (!collided)           
                collided = laserPoint.DistanceSQ(targetHitbox.ClosestPointInRect(laserPoint)) < 40 * 40;
                laserPoint = Projectile.Center + Projectile.velocity * 128;
            if (!collided)
                collided = laserPoint.DistanceSQ(targetHitbox.ClosestPointInRect(laserPoint)) < 72 * 72;
            laserPoint = Projectile.Center + Projectile.velocity * 16;
            if (!collided)
                collided = laserPoint.DistanceSQ(targetHitbox.ClosestPointInRect(laserPoint)) < 16 * 16;
            if (!collided)           
                for (int j = -1; j < 2; j += 2)
                {              
                    laserPoint = Projectile.Center + Projectile.velocity * 160 - Projectile.velocity.RotatedBy(j * MathF.PI * 0.5f) * 64;
                    collided = laserPoint.DistanceSQ(targetHitbox.ClosestPointInRect(laserPoint)) < 32 * 32;
                }          
            return collided;
        }
    }
}
