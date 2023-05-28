using Crystals.Content.Foresta.Buffs.NaturePower;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Accessories.Crystal
{
    public class Crystal_Forest : ModItem
    {
        
        public override void SetStaticDefaults()
        { 
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 19));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.DefaultToAccessory(36 , 60);
        }
        
        

        class Reanimation : ModPlayer
        {
            public override void Load()
            {
                On_Player.DelBuff += On_PlayerOnDelBuff;
            }

            private void On_PlayerOnDelBuff(On_Player.orig_DelBuff orig, Player self, int b)
            {
                if (self.buffType[b] == ModContent.BuffType<Reanimate>())
                {
                    self.KillMe(PlayerDeathReason.ByCustomReason(self.name + " Decayed"), 1000 , 0 , false);
                }
                orig(self, b);
            }

            public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore,
                ref PlayerDeathReason damageSource)
            {
                bool canReanimate = !Player.HasBuff<Reanimate>() && PlayerHelper.HasAccessoryEquipped(Player ,  ModContent.ItemType<Crystal_Forest>());

                if (damageSource.SourceCustomReason == Player.name + " Decayed")
                {
                    return true;
                }
                
                if (Player.HasBuff<Reanimate>())
                {
                    Player.statLife = Player.statLifeMax2;
                    return false;
                }
                
                if (canReanimate)
                {
                    float DamageToTime = (float) damage;

                    DamageToTime = MathHelper.Clamp(DamageToTime, 100, 600);
                    DamageToTime /= 100f;
                    DamageToTime *= 5f;
                    DamageToTime *= 60f;
                    
                    //TODO Add animation

                    Projectile.NewProjectile(Player.GetSource_Death(null), Player.Center, Vector2.Zero, ModContent.ProjectileType<CrystalVFX>(), 0, 0 , Player.whoAmI);
                    
                    SoundEngine.PlaySound(SoundID.Item4);
                    playSound = false;
                    genGore = false;
                    
                    Player.AddBuff(ModContent.BuffType<Reanimate>() , (int)DamageToTime);
                    return false;
                }

                return true;
            }
            
            
            
        }

        class CrystalVFX : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                Main.projFrames[Projectile.type] = 14;
            }

            public override void SetDefaults()
            {
                Projectile.Size = new Vector2(26, 112);
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.friendly = true;
            }

            public  int maxcount = 4;

            public override bool? CanCutTiles()
            {
                return false;
            }

            public override void AI()
            {
                var owner = Main.player[Projectile.owner];

                Lighting.AddLight(Projectile.Center , TorchID.Green);
                if (owner.dead)
                {
                    for (int i = 0; i < 14; i++)
                    {
                        Dust.NewDustPerfect(Projectile.Center, DustID.GreenFairy, Main.rand.NextVector2Circular(i, i));
                    }
                    Projectile.Kill();
                }
                
                Projectile.Center = new Vector2(owner.Center.X , owner.Center.Y - 50);

                
                
                if (++Projectile.frameCounter >= maxcount)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        maxcount = 6;
                        Projectile.frameCounter = 0;
                        Projectile.frame = 9;
                    }
                }
            }
        }
         
        
    }
}