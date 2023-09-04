using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

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

            /*hitbox = default(Rectangle);
            Player player = Main.player[Projectile.owner];
            int itemAnimationMax = player.itemAnimationMax;
            int itemAnimation = player.itemAnimation;
            int num = player.itemAnimationMax / 3;
            float num2 = Utils.Remap(itemAnimation, itemAnimationMax, num, 0f, 1f);
            float num3 = 10f;
            float num4 = 30f;
            float num5 = 10f;
            float num6 = 10f;
            num4 *= 1f / player.GetAttackSpeed(DamageClass.MeleeNoSpeed);
            float num7 = num3 + num4 * num2;
            float num8 = num5 + num6 * num2;
            float f = Projectile.velocity.ToRotation();
            Vector2 center = Projectile.Center + f.ToRotationVector2() * num7;
            hitbox = Utils.CenteredRectangle(center, new Vector2(num8, num8));*/

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

            if (CrusoSpear.CrusoAbility.HitCount == 5)
            {
                Vector2 vector = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                float num = Projectile.ai[1] / 60f;
                float num2 = 2f;
                for (int i = 0; (float)i < num2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center, 14, 14, 228, 0f, 0f, 110);
                    dust.velocity = vector * 2f;
                    dust.position = Projectile.Center + vector.RotatedBy(num * ((float)Math.PI * 2f) * 2f + (float)i / num2 * ((float)Math.PI * 2f)) * 7f;
                    dust.scale = 1f + 0.6f * Main.rand.NextFloat();
                    dust.velocity += vector * 3f;
                    dust.noGravity = true;
                }
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CrusoSpear.CrusoAbility.HitCount != 5)
            {
                CrusoSpear.CrusoAbility.HitCount++;
            }
        }
    }
}

