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
            private enum AttackType
            {
                Down,
                Up,
                Empowered
            }

            private float startRotation = 0f;
            private float endRotation = 0f;
            private float attackTime;

            private List<float> oldRotation = new List<float>();

            public override string Texture =>
                "Crystals/Content/Foresta/Items/Weapons/Melee/Crusolium/CrusoSword"; // Use texture of item as projectile texture

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
            }

            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
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
                
            }
            
            public override bool PreDraw(ref Color lightColor)
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
            }
            
        }
    }
}