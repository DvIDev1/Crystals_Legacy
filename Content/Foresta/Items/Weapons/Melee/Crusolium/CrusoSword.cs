using System;
using System.Collections.Generic;
using Crystals.Helpers;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Melee.Crusolium
{
    public class CrusoSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cruso Sword");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // Tooltip.SetDefault("Every three hits of an enemy swings a powered-up swing");
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;
            Item.width = 54;
            Item.height = 64;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.knockBack = 3;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<CrusoSwordSwing>();
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTurn = false;
        }
        public override bool CanUseItem(Player player)
        {
            // Ensures no more than one spear can be thrown out, use this when using autoReuse
            return player.ownedProjectileCounts[ModContent.ProjectileType<CrusoSwordSwing>()] < 1;
        }
        bool notboollol = true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int basic = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (notboollol == true)
            {
                Main.projectile[basic].ai[1] = -1;
                notboollol = false;
            }
            else
            {
                Main.projectile[basic].ai[1] = 1;
                notboollol = true;
            }
            Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale,
            int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.12f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[
                    Terraria.Dust.NewDust(position, Item.width, Item.height, 269, 0f, -10f, 0,
                        new Color(255, 255, 255),
                        1f)];
            }

            return true;
        }


        public override bool MeleePrefix()
        {
            return true;
        }

        class CrusoSwordSwing : ModProjectile
        {
            /*private enum AttackType
            {
                Down,
                Up,
                Empowered
            }

            private float startRotation = 0f;
            private float endRotation = 0f;
            private float attackTime;

            private List<float> oldRotation = new List<float>();

            private Player Owner => Main.player[Projectile.owner];

            private bool FacingRight;

            private AttackType CurrentAttack
            {
                get => (AttackType)Projectile.ai[0];
                set => Projectile.ai[0] = (float)value;
            }

            private float Timer
            {
                get => Projectile.localAI[0];
                set => Projectile.localAI[0] = value;
            } */

            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.width = 120;
                Projectile.height = 110;
                Projectile.friendly = true;
                Projectile.timeLeft = 40;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
                Projectile.ownerHitCheck = true;
                Projectile.DamageType = DamageClass.Melee;
            }

            /* public override void OnSpawn(IEntitySource source)
            {
                var owner = Main.player[Projectile.owner];

                startRotation = Projectile.rotation = Owner.DirectionTo(Main.MouseWorld).ToRotation();
                
                switch (CurrentAttack)
                {
                    case AttackType.Down:
                        endRotation = startRotation + 2f * Owner.direction;
                        attackTime = 60;
                        break;
                    case AttackType.Up:
                        endRotation = startRotation - 2f * Owner.direction;
                        attackTime = 60;
                        break;
                    case AttackType.Empowered: 
                        endRotation = startRotation + 2f * Owner.direction;
                        attackTime = 15;
                        break;
                }
                
            }

            public override void AI()
            {
                var owner = Main.player[Projectile.owner];

                Timer += 1f / attackTime;
                
                owner.direction =
                    (Projectile.rotation > -MathHelper.PiOver2 && Projectile.rotation < MathHelper.PiOver2) ? 1 : -1;
                Projectile.Center = owner.Center - new Vector2(owner.direction * 3, 2) +
                                    new Vector2(23, 0).RotatedBy(Projectile.rotation);
                Projectile.position.Y += owner.gfxOffY;
                owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                    Projectile.rotation - MathF.PI / 2);

                Projectile.rotation = MathHelper.Lerp(startRotation, endRotation, MathFunctions.EaseFunctions.EaseInOutQuad(Timer));

                if (Timer >= 1f)
                {
                    Projectile.Kill();
                }
                
            } */
            Vector2 dir = Vector2.Zero;
            Vector2 hlende = Vector2.Zero;
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
                float scaleFactor = 90f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
                float widthMultiplier = 23f; // How thick the hit-line is. Increase or decrease this value if your Jousting Lance is thicker or thinner. Vanilla uses 23f
                float collisionPoint = 0f; // collisionPoint is needed for CheckAABBvLineCollision(), but it isn't used for our collision here. Keep it at 0f.

                // This Rectangle is the width and height of the Jousting Lance's hitbox which is used for the first step of collision.
                // You will need to modify the last two numbers if you have a bigger or smaller Jousting Lance.
                // Vanilla uses (0, 0, 300, 300) which that is quite large for the size of the Jousting Lance.
                // The size doesn't matter too much because this rectangle is only a basic check for the collision (the hit-line is much more important).
                Rectangle lanceHitboxBounds = new Rectangle(0, 0, 284, 284);

                // Set the position of the large rectangle.
                lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
                lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

                // This is the back of the hit-line with Projectile.Center being the tip of the Jousting Lance.
                Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;

                hlende = hitLineEnd;
                // First check that our large rectangle intersects with the target hitbox.
                // Then we check to see if a line from the tip of the Jousting Lance to the "end" of the lance intersects with the target hitbox.
                if (/*lanceHitboxBounds.Intersects(targetHitbox)
                && */Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
                {
                    return true;
                }
                return false;
            }

            public override void AI()
            {
                // All Projectiles have timers that help to delay certain events
                // Projectile.ai[0], Projectile.ai[1] � timers that are automatically synchronized on the client and server
                // Projectile.localAI[0], Projectile.localAI[0] � only on the client
                // In this example, a timer is used to control the fade in / out and despawn of the Projectile
                //Projectile.ai[0] += 1f;
                if (dir == Vector2.Zero)
                {
                    dir = Main.MouseWorld;
                    Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[1]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                }
                Player player = Main.player[Projectile.owner];
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
                Projectile.Center = Main.player[Projectile.owner].Center;
                Projectile.ai[0] += 1f;
                Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((20 - Projectile.ai[0])));
                player.direction = Projectile.direction;
            }
            /*public override bool PreDraw(ref Color lightColor)
            {
                //DrawTrail(Main.spriteBatch);
                Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

                SpriteEffects effect = SpriteEffects.None;

                bool flip = false;
                SpriteEffects effects = SpriteEffects.None;

                var origin = new Vector2(0, tex.Height);

                Vector2 scaleVec = Vector2.One;
                for (int k = 16; k > 0; k--)
                {

                    float progress = 1 - (float)((16 - k) / (float)16);
                    Color color = lightColor * MathFunctions.EaseFunctions.EaseInOutQuad(progress) * 0.1f;
                    if (k > 0 && k < oldRotation.Count)
                        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, color, oldRotation[k] + 0.78f, origin, Projectile.scale * scaleVec, effects, 0f);
                }

                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + 0.78f, origin, Projectile.scale * scaleVec, effects, 0f);
                return false;
            } */
            
        } 
    }
}