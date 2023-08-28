using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace Crystals.Content.Foresta.Items.Weapons.Melee.Crusolium
{
    public class CrusoSpearProj : ModProjectile
    {
        public override string Texture => "Crystals/Content/Foresta/Items/Weapons/Melee/Crusolium/CrusoSpear";
        public override void SetDefaults()
        {
            Projectile.damage = 75;
            //Projectile.aiStyle = ProjAIStyleID.ForwardStab; please dont
            Projectile.width = 88;
            Projectile.height = 108;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Vector2 HitboxSize = new(88 * Projectile.scale, 88 * Projectile.scale);
            Vector2 HitboxCenter = Projectile.Center + (Projectile.velocity * (Projectile.Size.Length() / 2f - HitboxSize.Length() / 2f));
            hitbox = new Rectangle((int)(HitboxCenter.X - HitboxSize.X / 2f), (int)(HitboxCenter.Y - HitboxSize.Y / 2f), (int)(HitboxSize.X), (int)(HitboxSize.Y));
        }
        public int SwingDirection = 1;
        public float Extension = 0;
        int OrigAnimMax = 30;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (Projectile.localAI[0] == 0)
            {
                OrigAnimMax = player.itemAnimationMax;
                Projectile.localAI[0] = 1;
            }
            float HoldoutRangeMax = (float)Projectile.Size.Length() / 2;
            float HoldoutRangeMin = (float)-Projectile.Size.Length() / 6;
            if (player.heldProj == Projectile.whoAmI)
            {
                Projectile.friendly = true;

                int duration = (int)(OrigAnimMax / 1.5f);
                int WaitTime = OrigAnimMax / 5;


                if (Projectile.ai[1] == 0)
                    SwingDirection = Main.rand.NextBool(2) ? 1 : -1;
                float Swing = 13; //higher value = less swing
                Projectile.localNPCHitCooldown = OrigAnimMax;
                if (Projectile.timeLeft > OrigAnimMax)
                {
                    Projectile.timeLeft = OrigAnimMax;
                }
                if (Projectile.ai[1] <= duration / 2)
                {
                    Extension = Projectile.ai[1] / (duration / 2);
                    Projectile.velocity = Projectile.velocity.RotatedBy(SwingDirection * Projectile.spriteDirection * -Math.PI / (Swing * OrigAnimMax));
                }
                else if (Projectile.ai[1] <= duration / 2 + WaitTime)
                {
                    Extension = 1;
                    Projectile.velocity = Projectile.velocity.RotatedBy(SwingDirection * Projectile.spriteDirection * (1.5 * duration / WaitTime) * Math.PI / (Swing * OrigAnimMax)); //i know how wacky this looks
                }
                else
                {
                    Projectile.friendly = false; //no hit on backswing
                    Extension = (duration + WaitTime - Projectile.ai[1]) / (duration / 2);
                    Projectile.velocity = Projectile.velocity.RotatedBy(SwingDirection * Projectile.spriteDirection * -Math.PI / (Swing * OrigAnimMax));
                }

                Projectile.ai[1]++;
                Projectile.velocity = Vector2.Normalize(Projectile.velocity); //store direction
                Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, Extension);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.spriteDirection = Projectile.direction;
            player.ChangeDir(Projectile.direction);
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            player.itemTime = 2;
            player.itemAnimation = 2;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.ToRadians(-45f) + (float)Math.PI;
            }
            else
            {
                Projectile.rotation += MathHelper.ToRadians(-135f) + (float)Math.PI;
            }

        }
    }
}

