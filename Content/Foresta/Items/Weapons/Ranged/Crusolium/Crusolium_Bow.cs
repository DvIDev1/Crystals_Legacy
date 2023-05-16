using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium
{
    
    public class Crusolium_Bow : ModItem
    {
        public static int hits;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crusolium Bow");
            // Tooltip.SetDefault("Reliable and Efficient");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.keepTime = 80;
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28;
            Item.height = 48;
            Item.maxStack = 1;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = 5;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5;
            Item.noMelee = true;
            Item.shoot = 1;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 32f;
            Item.autoReuse = false;
            Item.crit = 14;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.buyPrice(0, 3, 75, 0);
        }
        
        public override bool RangedPrefix()
        {
            return true;
        }
        
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage,
            ref float knockback)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<Crusolium_Arrow>();
            }
        }
    }


    class Crusolium_Arrow : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crusolium Arrow");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.penetrate = 2;
        }
        
        public int time;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (hit.Crit)
            {
                hit.Crit = false;
                player.statLife++;
                Projectile.penetrate++;
                player.HealEffect(1);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            if (target.HasBuff(ModContent.BuffType<GreenMark>()))
            {
                modifiers.FinalDamage *= 1.5f;
            }
            else
            {
                target.AddBuff(ModContent.BuffType<GreenMark>() , 60 * 7);
                Projectile.Kill();
            }
            modifiers.FinalDamage.Flat += (int)player.Distance(target.Center) / 100;
        }

        public override bool PreAI()
        {
            time++;
            Dust.NewDustPerfect(Projectile.Center, 61);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity += Projectile.velocity * 0.02f; 
            return false;
        }
        
        public override bool PreDraw(ref Color lightColor) {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f , Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        
        public override void Kill(int timeLeft) {
            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
        
        
    }

    public class GreenMark : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Green Mark");
            // Description.SetDefault("Marks Enemies marked enemies get 50% more damage");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.NextFloat() < 0.10465116f)
            {
                Dust dust; 
                Vector2 position = player.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, player.width, player.height, 74, 0f, -2.0930233f, 0, new Color(255,255,255), 1f)];
                dust.fadeIn = 0.20930234f;
            }

        }
        
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextFloat() < 0.10465116f)
            {
                Dust dust; 
                Vector2 position = npc.position;
                dust = Main.dust[Terraria.Dust.NewDust(position, npc.width, npc.height, 74, 0f, -2.0930233f, 0, new Color(255,255,255), 1f)];
                dust.fadeIn = 0.20930234f;
            }

        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.DelBuff(buffIndex);
            return false;
        }
    }


}