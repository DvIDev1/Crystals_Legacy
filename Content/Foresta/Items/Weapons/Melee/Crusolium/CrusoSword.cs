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
        }

        public int attackType = 0; 
        public int comboExpireTimer = 0;


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
            Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, attackType);
            attackType = (attackType + 1) % 2;
            Main.NewText(attackType);
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
        

        public override bool MeleePrefix() {
            return true; 
        }

        class CrusoSwordSwing : ModProjectile
        {
            private enum AttackType 
            {
                Down,
                Up,
                Empowered
            }
            
            private float StartRotation = 0f;
            private float EndRotation = 0f;
            private int AttackTime;
            
            private List<float> oldRotation = new List<float>();

            public override string Texture => "Crystals/Content/Foresta/Items/Weapons/Melee/Crusolium/CrusoSword"; // Use texture of item as projectile texture
            private Player Owner => Main.player[Projectile.owner];
            
            private bool FacingRight;
            
            private AttackType CurrentAttack {
                get => (AttackType)Projectile.ai[0];
                set => Projectile.ai[0] = (float) value;
            }

            public override void SetStaticDefaults() {
                ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            }

            public override void SetDefaults() {
                Projectile.width = 54; // Hitbox width of projectile
                Projectile.height = 64; // Hitbox height of projectile
                Projectile.friendly = true; // Projectile hits enemies
                Projectile.timeLeft = 100; // Time it takes for projectile to expire
                Projectile.penetrate = -1; // Projectile pierces infinitely
                Projectile.tileCollide = false; // Projectile does not collide with tiles
                Projectile.usesLocalNPCImmunity = true; // Uses local immunity frames
                Projectile.localNPCHitCooldown = -1; // We set this to -1 to make sure the projectile doesn't hit twice
                Projectile.ownerHitCheck = true; // Make sure the owner of the projectile has line of sight to the target (aka can't hit things through tile).
                Projectile.DamageType = DamageClass.Melee; // Projectile is a melee projectile
            }

            public override void OnSpawn(IEntitySource source)
            {
                //Sword Pos
                Projectile.velocity = Vector2.Zero;
                Owner.heldProj = Projectile.whoAmI;
                

                if (Owner.DirectionTo(Main.MouseWorld).X > 0)
                    FacingRight = true;
                else
                    FacingRight = false;
                
                //Swing Rotaion
                float rotation = Owner.DirectionTo(Main.MouseWorld).ToRotation();
                
                EndRotation = rotation - 1f * Owner.direction;
                
                StartRotation = EndRotation;

                switch (CurrentAttack)
                {
                    case AttackType.Down:
                        EndRotation = rotation + 2f * Owner.direction;
                        AttackTime = 120;
                        break;
                    case AttackType.Up:
                        EndRotation = rotation - 2f * Owner.direction;
                        AttackTime = 120;
                        break;
                    case AttackType.Empowered: 
                        EndRotation = rotation + 2f * Owner.direction;
                        AttackTime = 60;
                        break;
                }
                
               

            }

            public override void AI()
            {
                
                Projectile.Center = Main.GetPlayerArmPosition(Projectile);
                Projectile.ai[1] += 15f / AttackTime;
                Projectile.rotation = MathHelper.Lerp(StartRotation, EndRotation,
                    Projectile.ai[1]);
                    
                oldRotation.Add(Projectile.rotation);
                
                Owner.ChangeDir(FacingRight ? 1 : -1);

                float wrappedRotation = MathHelper.WrapAngle(Projectile.rotation);

                if (FacingRight)
                    Owner.itemRotation = MathHelper.Clamp(wrappedRotation, -1.57f, 1.57f);
                else if (wrappedRotation > 0)
                    Owner.itemRotation = MathHelper.Clamp(wrappedRotation, 1.57f, 4.71f);
                else
                    Owner.itemRotation = MathHelper.Clamp(wrappedRotation, -1.57f, -4.71f);
                Owner.itemRotation = MathHelper.WrapAngle(Owner.itemRotation - (FacingRight ? 0 : MathHelper.Pi));
                Owner.itemAnimation = Owner.itemTime = 5;

                if (Projectile.rotation == EndRotation)
                {
                    Projectile.Kill();
                }

            }
            
            public override bool PreDraw(ref Color lightColor)
            {
                Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

                bool flip = false;
                SpriteEffects effects = SpriteEffects.None;

                var origin = new Vector2(0, tex.Height);

                Vector2 scaleVec = Vector2.One;
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                
                for (int k = 16; k > 0; k--)
                {
                    float progress = 1 - (float)((16 - k) / (float)16);
                    Color color = lightColor * EaseFunctions.easeInOutQuad(progress) * 0.1f;
                    if (k > 0 && k < oldRotation.Count)
                        Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, color, oldRotation[k] + 0.78f, origin, Projectile.scale * scaleVec, effects, 0f);
                }
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + 0.78f, origin, Projectile.scale * scaleVec, effects, 0f);
                return false;
            }
            

            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {

                float collisionPoint = 0f;

                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + 54 * Projectile.rotation.ToRotationVector2(), 20, ref collisionPoint))
                    return true;

                return false;
            }


        }
        
    }
}