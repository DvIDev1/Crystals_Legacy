using System;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Armors.Crusolium
{
    public class Cruso_ArmorSet : ModPlayer
    {

        public static float charge = 0;
        
        public override void PostUpdate()
        {
            if (hasCrusoSet())
            {
                if (Main.rand.NextFloat() < charge / 100f)
                {
                    Dust dust;
                    Vector2 position = Player.Center - new Vector2(charge *4 , charge *4) /2;
                    dust = Main.dust[Dust.NewDust(position, (int)charge*4, (int)charge*4, 107, 0f, -10f, 0, new Color(255,255,255), 0.5f)];
                    dust.shader = GameShaders.Armor.GetSecondaryShader(5 , Player);
                    dust.fadeIn = 0.5f;
                }
                if (charge >= 200f)
                {
                    charge = 200f;
                }

                if (Player.HasBuff<Empowered>())
                {
                    VisualHelper.Yoraiz0rEye(Player , 107 , 5);
                }
                
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (hasCrusoSet())
            {
                if (charge < 100f)
                {
                    charge +=  hit.Damage / 25f;
                }
                else if (!Player.HasBuff<Empowered>())
                {
                    Player.AddBuff(ModContent.BuffType<Empowered>() , 60*30);
                    SoundEngine.PlaySound(SoundID.AbigailUpgrade.WithVolumeScale(5), Player.Center);
                }
            }

        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (hasCrusoSet())
            {
                if (charge < 100f)
                {
                    charge += hit.Damage / 25f;
                }
                else if (!Player.HasBuff<Empowered>())
                {
                    Player.AddBuff(ModContent.BuffType<Empowered>() , 60*30);
                    SoundEngine.PlaySound(SoundID.AbigailUpgrade.WithVolumeScale(5), Player.Center);
                }
            }
        }

        public bool hasCrusoSet()
        {
            return Player.armor[0].type == ModContent.ItemType<CrusoHelmet>()
                   && Player.armor[1].type == ModContent.ItemType<CrusoChestplate>()
                   && Player.armor[2].type == ModContent.ItemType<CrusoGreaves>();
        }

        [AutoloadEquip(EquipType.Head)]
        public class CrusoHelmet : ModItem
        {
            
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Crusolium Helmet");
                // Tooltip.SetDefault("Increased Crit Chance and Armor Pen by 5%  For both Ranged and Meele");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }
            
            public override void SetDefaults()
            {
                Item.width = 26;
                Item.height = 30;
                Item.defense = 5;
                Item.value = ValueHelper.GetCoinValue(0, 3, 47, 0);
                Item.rare = ItemRarityID.Green;
            }
            
            public override void UpdateEquip(Player player)
            {
                player.GetCritChance(DamageClass.Melee) += 0.05f;
                player.GetCritChance(DamageClass.Ranged) += 0.05f;
                player.GetArmorPenetration(DamageClass.Default) += 0.05f;
            }
            
            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return head.type == ModContent.ItemType<CrusoChestplate>() && body.type == ModContent.ItemType<CrusoGreaves>();
            }
            
            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Allows you to Charge when fully charged you become Empowered";
            }
            
            
        }

        [AutoloadEquip(EquipType.Body)]
        public class CrusoChestplate: ModItem
        {
            
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Crusolium Chestplate");
                // Tooltip.SetDefault("3% Increased Meele and Ranged Damage");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }
            
            public override void SetDefaults()
            {
                Item.width = 34;
                Item.height = 20;
                Item.defense = 7;
                Item.value = ValueHelper.GetCoinValue(0, 5, 25, 0);
                Item.rare = ItemRarityID.Green;
            }
            
            public override void UpdateEquip(Player player)
            {
                player.GetDamage(DamageClass.Melee) += 0.03f;
                player.GetDamage(DamageClass.Ranged) += 0.03f;
            }
            
            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return head.type == ModContent.ItemType<CrusoHelmet>() && body.type == ModContent.ItemType<CrusoGreaves>();
            }
            
            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Allows you to Charge when fully charged you become Empowered";
            }
            
        }
        
        [AutoloadEquip(EquipType.Legs)]
        public class CrusoGreaves : ModItem
        {
            
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Crusolium Greaves");
                // Tooltip.SetDefault("Increased Damage Reduction by 2%");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }
            
            public override void SetDefaults()
            {
                Item.width = 26;
                Item.height = 18;
                Item.defense = 3;
                Item.value = ValueHelper.GetCoinValue(0, 2, 10, 0);
                Item.rare = ItemRarityID.Green;
            }
            
            public override void UpdateEquip(Player player)
            {
                player.endurance += 0.02f;
            }
            
            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return head.type == ModContent.ItemType<CrusoHelmet>() && body.type == ModContent.ItemType<CrusoChestplate>();
            }
            
            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "Allows you to Charge when fully charged you become Empowered";
            }
            
        }

        class Empowered : ModBuff
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Empowered");
                // Description.SetDefault("Makes you Stronger and Faster");
            }

            public override void Update(Player player, ref int buffIndex)
            {
                player.aggro = 100;
                charge -= 0.1f;
                player.GetArmorPenetration(DamageClass.Generic) += 100f;
                player.GetCritChance(DamageClass.Melee) += 25f;
                player.GetCritChance(DamageClass.Ranged) += 25f;
                player.GetDamage(DamageClass.Generic) += 0.10f;
                player.maxRunSpeed += 2;
                if (charge <= 0f)
                {
                    charge = 0;
                    player.DelBuff(buffIndex);
                }
            }
            
            
            
        }
        
    }
}