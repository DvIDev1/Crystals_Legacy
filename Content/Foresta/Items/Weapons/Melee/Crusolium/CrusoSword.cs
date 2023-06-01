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
            private float AttackTime;
            
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
                Projectile.width = 54; 
                Projectile.height = 64;
                Projectile.friendly = true; 
                Projectile.timeLeft = 100; 
                Projectile.penetrate = -1; 
                Projectile.tileCollide = false; 
                Projectile.usesLocalNPCImmunity = true; 
                Projectile.localNPCHitCooldown = -1;
                Projectile.ownerHitCheck = true; 
                Projectile.DamageType = DamageClass.Melee;
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
                base.AI();
            }
        }
        
    }
}