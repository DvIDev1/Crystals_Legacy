using Crystals.Content.Foresta.Buffs.NaturePower;
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
                bool canReanimate = !Player.HasBuff<Reanimate>();

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
                    SoundEngine.PlaySound(SoundID.Item4);
                    playSound = false;
                    genGore = false;
                    
                    Player.AddBuff(ModContent.BuffType<Reanimate>() , (int)DamageToTime);
                    return false;
                }

                return true;
            }
            
            
            
        }

        /*class CrystalVFX : ModProjectile
        {
            
        }*/
         
        
    }
}