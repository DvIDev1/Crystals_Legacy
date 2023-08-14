using Crystals.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Magic.Incaen
{
    public class Incaen : ModItem
    {
        public override string Texture => AssetDirectory.Magic + Name;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Incaen");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 3));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.Size = new Vector2(28 ,30);
            Item.noMelee = true;
            Item.crit = 3;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 17;
            Item.useTime = 9;
            Item.useAnimation = 26;
            Item.reuseDelay = 11;
            Item.knockBack = 2.70f;
            Item.mana = 10;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = false;

            Item.shoot = ModContent.ProjectileType<FireLeaves>();
            Item.shootSpeed = 15.0f;
        }

        public override bool MagicPrefix()
        {
            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale,
            int whoAmI)
        {
            Lighting.AddLight(Item.Center , TorchID.Torch);
            return true;
        }

        class FireLeaves : ModProjectile
        {
            public override string Texture => AssetDirectory.Magic + Name;

            private int hitcount = 1; 
            
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Fire Leaves");
                Main.projFrames[Projectile.type] = 9;
            }
            
            public override void SetDefaults()
            {
                Projectile.Size = new Vector2(26 , 28);
                Projectile.DamageType = DamageClass.Magic;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;
                Projectile.friendly = true;
                Projectile.timeLeft = 60;
                Projectile.penetrate = 1;
            }

            private bool burnt = false;
        
            public override void AI()
            {
                
                if (hitcount == 3)
                {
                    Projectile.Kill();
                }
            
                if (burnt)
                {
                    if (Framing.GetTileSafely(Projectile.position).LiquidAmount == 255)
                    {
                        Projectile.Kill();
                    }
                    else
                    {
                        Dust.NewDustPerfect(Projectile.Center, 6);
                    }
                    Projectile.penetrate = 2;
                    
                    Projectile.velocity.Y = Projectile.velocity.Y + 0.1f;
                    Projectile.ignoreWater = false;
                    if (Projectile.velocity.Y > 16f)
                    {
                        Projectile.velocity.Y = 16f;
                    }
                }
                else
                {
                    Dust.NewDustPerfect(Projectile.Center, 3);
                }

                if (++Projectile.frameCounter >= 3)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        burnt = true;
                    }
                }
                
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                hitcount++;
                if (burnt)
                {
                    target.AddBuff(BuffID.OnFire , 60*3);
                }
            }

            public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
            {
                modifiers.SourceDamage /= hitcount;
            }
            
            
            
        }
        
    }
}