using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium;
using Crystals.Core.Systems.SoundSystem;

namespace Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium
{
    //STOP USING UNDERSCORES
    public class Crusolium_Bow : ModItem
    {
        //Why did you still have this? -Photonic0
        //public static int hits;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crusolium Bow");
            // Tooltip.SetDefault("Reliable and Efficient");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.keepTime = 80;
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28;
            Item.height = 48;
            Item.maxStack = 1;
            Item.useTime = Item.useAnimation = 40;
            Item.useStyle = 5;//USE ID CLASSES DAMMIT -Photonic0
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Green;
            Item.noMelee = true;
            Item.shoot = 1;
            Item.useAmmo = AmmoID.Arrow;
            Item.noUseGraphic = true;
            Item.shootSpeed = 32f;
            Item.autoReuse = false;
            Item.UseSound = SoundSystem.ChargeBow;
            Item.crit = 14;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            //Item.buyPrice(0, 3, 75, 0);//this doesn't set the actual item's value it just returns an int wtf are you doing? -Photonic0
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<CrusoliumBowHeldProj>()] < 9999;//TEST REMOVE 9999 LATER
        public override bool CanShoot(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<CrusoliumBowHeldProj>()] < 1;//just use this syntax to avoid line bloat -Photonic0
        public override bool RangedPrefix() => true;  //just use this syntax to avoid line bloat -Photonic0
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            
               type = ModContent.ProjectileType<CrusoliumBowHeldProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(Main.myPlayer == player.whoAmI)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, player.itemTimeMax);
            }
            return false;
        }
    }


    class CrusoliumArrow : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crusolium Arrow");
            //you do know that this will draw 2 afterimages right? It's basically nothing. The standart is 10 -Photonic0
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
            Projectile.penetrate = 2;

            //my brother in christ please actuallly remember to make your piercing projectiles have local iframes - Photonic0
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        
        /// <summary>
        /// do this so you can actually sync time when you call netupdate -Photonic0
        /// </summary>
        public int Time { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }

        private Vector2 startPos = Vector2.Zero;
        
        public override void OnSpawn(IEntitySource source)
        {
            startPos = Projectile.Center;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            modifiers.DisableCrit();
            modifiers.FinalDamage += player.Distance(startPos) * 0.001f;
            if (target.HasBuff(ModContent.BuffType<GreenMark>()))
                modifiers.FinalDamage *= 1.5f;
            else
            {
                target.AddBuff(ModContent.BuffType<GreenMark>(), 60 * 7);
                Projectile.Kill();
            }
        }

        public override bool PreAI()//if you set an AIStyle why are you always returning false on PreAI?
        {
            Time++;
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
            for (int i = 0; i < Projectile.oldPos.Length; i++) {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
        
        public override void Kill(int timeLeft) {
            
            Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
            //set maxinstances to 0 (infinite) to make the sound not cut itself off when more than 1 plays at once -Photonic0
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
        }        
    }

    public class GreenMark : ModBuff
    {
        public override void Update(Player player, ref int buffIndex) => UpdateVFX(player);
        public override void Update(NPC npc, ref int buffIndex) => UpdateVFX(npc);

        /// <summary>
        /// Both player and npc inherit width and height from terraria's vanilla Entity class, so you can do this instead -Photonic0
        /// </summary>
        private static void UpdateVFX(Entity entity)
        {
            if (Main.rand.NextFloat() < 0.10465116f)//why this number? -Photonic0
            {
                //use NewDustDirect or NewDustPerfect next time, Color.White and DustID.GreenFairy. Also Scale is already 1 by default. -Photonic0
                //You don't need to declare the dust variable on a separate line either -Photonic0
                Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(entity.Hitbox), DustID.GreenFairy, new Vector2(0, -2.0930233f)).fadeIn = 0.20930234f;
            }
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.DelBuff(buffIndex);
            return false;
        }
    }


}