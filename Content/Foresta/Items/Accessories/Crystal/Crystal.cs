using System.Linq;
using Crystals.Content.Foresta.Buffs.NaturePower;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Crystals.Content.Foresta.Items.Accessories.Crystal
{
    public class Crystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal of The Forest");
            // Tooltip.SetDefault("It holds something together... \nIncreased max health by 50");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.rare = ItemRarityID.Expert;
            Item.accessory = true; 
        }

        //Todo as soon as other Crystals added make them incompitable 
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
        }

        class ResurrectionPlayer : ModPlayer
        {
            public bool ForestCrystal = false;

            public int alrheal;

            public override void UpdateEquips()
            {
                ForestCrystal = false;
                foreach (var acc in Player.armor)
                {
                    if (acc.type == ModContent.ItemType<Crystal>())
                    {
                        ForestCrystal = true;
                    }
                }

                if (Player.HasBuff(ModContent.BuffType<NaturePower>()))
                {
                    if (Main.rand.NextFloat() < 0.24)
                    {
                        Dust dust; 
                        Vector2 position = Player.position;
                        dust = Main.dust[Terraria.Dust.NewDust(position, Player.width, Player.height, 61, Main._rand.NextFloat(-10 , 10), Main._rand.NextFloat(-10 , 10), 0, new Color(255,255,255), 1f)];
                        dust.noGravity = true;
                        dust.fadeIn = 1.15f;
                    }
                }

            }

            public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
            {
                if (!target.active)
                {
                    if (ForestCrystal)
                    {
                        if (Player.HasBuff(ModContent.BuffType<NaturePower>()))
                        {
                            if (alrheal <= Player.statLifeMax)
                            {
                                Player.Heal(10);
                                alrheal += 10;
                            }
                        }
                    }
                }
            }

            public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
            {
                if (!target.active)
                {
                    if (ForestCrystal)
                    {
                        if (Player.HasBuff(ModContent.BuffType<NaturePower>()))
                        {
                            if (alrheal <= Player.statLifeMax)
                            {
                                Player.Heal(10);
                                alrheal += 10;
                            }
                        }
                    }
                }            
            }

            public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore,
                ref PlayerDeathReason damageSource)
            {
                if (ForestCrystal)
                {
                    if (!Player.HasBuff(ModContent.BuffType<NaturePower>()))
                    {
                        Player.Heal((int)damage);
                        Player.AddBuff(ModContent.BuffType<NaturePower>(), 20 * 10000 , false);
                        SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                        return false;
                    }
                    else return true;
                }
                else return true;

            }

        }
        

    }
}