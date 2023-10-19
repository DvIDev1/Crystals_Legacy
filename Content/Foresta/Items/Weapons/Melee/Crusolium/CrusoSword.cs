using System;
using System.Collections.Generic;
using System.IO;
using Crystals.Core;
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
        public override string Texture => AssetDirectory.Melee + Name;

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
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 3;
            Item.value = 10000;
            Item.rare = ItemRarityID.Orange;
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
                Main.projectile[basic].ai[2] = -1;
                notboollol = false;
            }
            else
            {
                Main.projectile[basic].ai[2] = 1;
                notboollol = true;
            }
            Main.projectile[basic].rotation = Main.projectile[basic].DirectionTo(Main.MouseWorld).ToRotation();
            return false;
        }
        public override void HoldItem(Player player)
        {
            if (CrusoSwordSwing.hitcount >= 3)
                Item.damage = 38;
            else
                Item.damage = 23;
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
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            if (Main.itemAnimations[Item.type] != null)
            {
                // In case this item is animated, this picks the correct frame
                frame = Main.itemAnimations[Item.type].GetFrame(texture, Main.itemFrameCounter[0]);
            }
            else
            {
                frame = texture.Frame();
            }
            float time = Main.GlobalTimeWrappedHourly;
            float timer = Item.timeSinceItemSpawned / 240f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;
            for (float i = 0; i < 4; i += 0.35f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;
                if (CrusoSwordSwing.empowered)
                {
                    spriteBatch.Draw(texture, position + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(255, 175, 247, 70), 0, origin, scale, SpriteEffects.None, 0);
                    spriteBatch.Draw(texture, position, frame, new Color(167, 197, 95), 0, origin, scale * 1.1f, SpriteEffects.None, 0);
                }
                    
            }

            return true;
        }
        class CrusoSwordSwing : ModProjectile
        {
            public override string Texture => AssetDirectory.Melee + Name;

            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            }

            public override void SetDefaults()
            {
                Projectile.width = 120;
                Projectile.height = 110;
                Projectile.friendly = true;
                Projectile.timeLeft = 20;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
                Projectile.ownerHitCheck = true;
                Projectile.DamageType = DamageClass.Melee;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = -1;
            }
            Vector2 dir = Vector2.Zero;
            Vector2 hlende = Vector2.Zero;
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                float rotationFactor = Projectile.rotation + (float)Math.PI / 4f; // The rotation of the Jousting Lance.
                float scaleFactor = 110f; // How far back the hit-line will be from the tip of the Jousting Lance. You will need to modify this if you have a longer or shorter Jousting Lance. Vanilla uses 95f
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
            public override bool PreDraw(ref Color lightColor)
            {
                Main.instance.LoadProjectile(Projectile.type);
                Texture2D proj = TextureAssets.Projectile[Type].Value;

                if (Projectile.ai[2] == 1 && empowered)
                {     
                    Vector2 drawOrigin = new Vector2(proj.Width * 0.5f, Projectile.height * 0.5f);
                    for (int k = 0; k < Projectile.oldPos.Length; k++)
                    {
                        Color lightColorr = lightColor;
                        lightColorr.A = 0;
                        Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                        Main.EntitySpriteDraw(proj, drawPos, null, lightColorr, Projectile.oldRot[k] - 0.78f, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                    }
                }
                if (Projectile.ai[2] == -1 && empowered)
                {
                    Vector2 drawOrigin = new Vector2(proj.Width * 0.5f, Projectile.height * 0.5f);
                    for (int k = 0; k < Projectile.oldPos.Length; k++)
                    {
                        Color lightColorr = lightColor;
                        lightColorr.A = 0;
                        Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                        Main.EntitySpriteDraw(proj, drawPos, null, lightColorr, Projectile.oldRot[k] - 0.78f, drawOrigin, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
                    }
                }

                if (Projectile.ai[2] == 1)
                    Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                if (Projectile.ai[2] == -1)
                    Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

                return false;
            }
            int a;
            public static int hitcount;
            public static bool empowered = false;
            public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
            {
                Player player = Main.player[Projectile.owner];
                Projectile.friendly = false;
                hitcount++;
                if (hitcount >= 3)
                {
                    empowered = true;
                    SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
                }
                   
                if (hitcount >= 4)
                {
                    // Sky projectiles
                    Vector2 pos = npc.Center + new Vector2(-10, -300);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), pos + new Vector2(10, Main.rand.NextFloat(0, -60)), new Vector2(0, 18), ModContent.ProjectileType<CrusoMagicSword>(), Projectile.damage * 2, Projectile.knockBack, player.whoAmI);
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), pos + new Vector2(-10, Main.rand.NextFloat(0, -60)), new Vector2(0, 18), ModContent.ProjectileType<CrusoMagicSword>(), Projectile.damage * 2, Projectile.knockBack, player.whoAmI);

                    // Slash effect
                    float rot = MathHelper.ToRadians(Main.rand.Next(0, 360));
                    Vector2 pos2 = new Vector2(16, 0).RotatedBy(rot);
                    int basic = Projectile.NewProjectile(Projectile.InheritSource(Projectile), npc.Center + pos2, pos2 / -4, ProjectileID.SuperStarSlash, 0, 0, Main.player[Projectile.owner].whoAmI);
                    Main.projectile[basic].rotation = rot;
                    Main.projectile[basic].scale = Main.rand.NextFloat(0.6f, 1.2f);
                    Main.projectile[basic].penetrate = -1;
                    Main.projectile[basic].friendly = false;

                    for (int i = 0; i < 20; i++)
                    {
                        int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, DustID.YellowStarDust, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                        Main.dust[dust].velocity *= 4f;
                    }
                    SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
                    hitcount = 0;
                    empowered = false;
                }
                // Main.NewText(hitcount.ToString());
            }
            public override void SendExtraAI(BinaryWriter writer)
            {
                writer.Write(a);
            }
            public override void ReceiveExtraAI(BinaryReader reader)
            {
                a = reader.ReadInt32();
            }
            public override void AI()
            {
                if (dir == Vector2.Zero)
                {
                    dir = Main.MouseWorld;
                    Projectile.rotation = (MathHelper.PiOver2 * Projectile.ai[2]) - MathHelper.PiOver4 + Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                }
                //FadeInAndOut();
                Projectile.Center = Main.player[Projectile.owner].Center;
                a++;
                Player player = Main.player[Projectile.owner];
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 90);
                if (Projectile.ai[2] <= 8)
                {
                    Projectile.ai[0] += 0.1f;
                    Projectile.ai[1] += 0.1f;
                }
                else
                {
                    Projectile.ai[0] += 0.2f;
                    Projectile.ai[1] += 0.2f;
                }
                if (a >= 9 && a <= 15)
                    Projectile.scale += 0.1f;
                if (a >= 15 && a <= 20)
                    Projectile.scale -= 0.2f;


                if (a == 10 && !empowered)
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                if (a == 10 && empowered)
                    SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);


                if (Projectile.ai[2] == 1)
                    Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((10 - Projectile.ai[0])));
                if (Projectile.ai[2] == -1)
                    Projectile.rotation += (Projectile.ai[1] * MathHelper.ToRadians((-10 - Projectile.ai[0])));
            }  
        }
    }
    public class CrusoMagicSword : ModProjectile
    {
        public override string Texture => AssetDirectory.Melee + Name;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 24;
            Projectile.width = 22;
            Projectile.height = 48;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.alpha -= 30;
            if (Projectile.alpha <= 0)
                Projectile.alpha = 0;
            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.YellowStarDust, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);
                Main.dust[dust].velocity *= 3f;
            }
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }


    }
}
