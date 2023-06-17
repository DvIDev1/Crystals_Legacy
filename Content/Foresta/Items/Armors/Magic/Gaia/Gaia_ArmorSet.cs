using System;
using System.Collections.Generic;
using Crystals.Content.Foresta.Items.Armors.Magic.Gaia.Items.PickUps;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Armors.Magic.Gaia
{
    public class Gaia_ArmorSet : ModPlayer
    {
        public Projectile Flower;
        public bool hasGaiaArmorSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Gaia_Hat>()
                   && Player.armor[1].type == ModContent.ItemType<Gaia_Robe>()
                   && Player.armor[2].type == ModContent.ItemType<Gaia_Skirt>();
        }

        public override void PreUpdate()
        {
            if (hasGaiaArmorSet())
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<Flore>()] == 0)
                {
                    Flower = Projectile.NewProjectileDirect(Terraria.Entity.GetSource_None(),
                        Player.Center - new Vector2(0, Player.height), Vector2.Zero, ModContent.ProjectileType<Flore>(),
                        0, 0, Player.whoAmI);
                }
                else
                {

                    if (Player.dead) Flower.alpha = 400;
                    Flower.active = true;
                    Flower.timeLeft += 10;
                }
            }
            else
            {
                if (Flower?.type == ModContent.ProjectileType<Flore>() && Player.ownedProjectileCounts[ModContent.ProjectileType<Flore>()] == 1)
                {
                    Flower?.Kill();
                }
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (hasGaiaArmorSet())
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<Flore>()] == 1)
                {
                    if (hit.Crit)
                        if (Flore.FlowerOpened == false)
                        {
                            Flower.ai[0] = 0;
                            VisualHelper.DrawDustLine(target.Center , Flower.Center + Player.velocity , 1 , 107);
                            for (int i = 0; i < 12; i++)
                            {
                                Dust.NewDust(Flower.position + Player.velocity , Flower.width, Flower.height, 107,
                                    Main.rand.NextVector2Circular(4 , 4).X * 2, 
                                    Main.rand.NextVector2Circular(4 , 4).Y * 2, 0,
                                    new Color(255, 255, 255), 1f);
                            }
                            Flower.ai[1] += (float)damageDone;
                        }
                }
            }
                
            
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (hasGaiaArmorSet())
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<Flore>()] == 1)
                {
                    if (hit.Crit)
                        if (Flore.FlowerOpened == false)
                        {
                            Flower.ai[0] = 0;
                            VisualHelper.DrawDustLine(target.Center , Flower.Center + Player.velocity , 1 , 107);
                            for (int i = 0; i < 12; i++)
                            {
                                Dust.NewDust(Flower.position + Player.velocity, Flower.width, Flower.height, 107,
                                    Main.rand.NextVector2Circular(4 , 4).X * 2, 
                                    Main.rand.NextVector2Circular(4 , 4).Y * 2, 0,
                                    new Color(255, 255, 255), 1f);
                            }
                            Flower.ai[1] += (float)damageDone;
                        }
                }
            }
        }

        [AutoloadEquip(EquipType.Head)]
        public class Gaia_Hat : ModItem
        {
            public float armorPen = 0.03f; //Armor Penetration the Player gets when this Armor Piece gets Equipped
            public float MagicDamage = 0.02f; //Increased Magic damage by Percent
            public int manaIncrease = 10; //Increased Mana when Armor is Equipped

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Gaia Hat");
                // Tooltip.SetDefault("Increased Armor Penetration by 3% \nIncreased maximum mana by 10 \nIncreased Magic damage by 2%");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }

            public override void SetDefaults()
            {
                Item.width = 40;
                Item.height = 40;
                Item.defense = 3;
                Item.rare = ItemRarityID.Green;
            }

            public override void UpdateEquip(Player player)
            {
                player.statManaMax2 += manaIncrease;
                player.GetArmorPenetration(DamageClass.Generic) += armorPen;
                player.GetDamage(DamageClass.Magic) += MagicDamage;
            }

            public override bool MagicPrefix()
            {
                return true;
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "The Armor gives you a Flower to Assist you Crit a bit and the Flower will Bloom you a reward";
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return body.type == ModContent.ItemType<Gaia_Robe>() && legs.type == ModContent.ItemType<Gaia_Skirt>();
            }

            /*public override void AddRecipes()
            {
                Recipe mod = CreateRecipe(1);
                mod.AddIngredient<Leaf>(15);
                mod.AddIngredient(ItemID.JungleHat, 1);
                mod.AddIngredient(ItemID.Wood, 20);
                mod.AddTile(TileID.LivingLoom);
                mod.Register();
            }*/
        }

        [AutoloadEquip(EquipType.Body)]
        public class Gaia_Robe : ModItem
        {
            public float armorPen = 0.05f; //Armor Penetration the Player gets when this Armor Piece gets Equipped
            public float MagicDamage = 0.05f; //Increased Magic damage by Percent
            public int manaIncrease = 15; //Increased Mana when Armor is Equipped

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Gaia Robe");
                // Tooltip.SetDefault("Increased Armor Penetration by 5% \nIncreased maximum mana by 15 \nIncreased Magic damage by 5%");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }


            public override void SetDefaults()
            {
                Item.width = 40;
                Item.height = 40;
                Item.defense = 4;
                Item.rare = ItemRarityID.Green;
            }

            public override bool MagicPrefix()
            {
                return true;
            }

            public override void UpdateEquip(Player player)
            {
                player.statManaMax2 += manaIncrease;
                player.GetArmorPenetration(DamageClass.Generic) += armorPen;
                player.GetDamage(DamageClass.Magic) += MagicDamage;
            }


            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "The Armor gives you a Flower to Assist you Crit a bit and the Flower will Bloom you a reward";
            }

            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return head.type == ModContent.ItemType<Gaia_Hat>() && legs.type == ModContent.ItemType<Gaia_Skirt>();
            }

            /*public override void AddRecipes()
            {
                Recipe mod = CreateRecipe(1);
                mod.AddIngredient<Leaf>(15);
                mod.AddIngredient(ItemID.JungleHat, 1);
                mod.AddIngredient(ItemID.Wood, 20);
                mod.AddTile(TileID.LivingLoom);
                mod.Register();
            }*/
        }

        [AutoloadEquip(EquipType.Legs)]
        public class Gaia_Skirt : ModItem
        {
            public float armorPen = 0.05f; //Armor Penetration the Player gets when this Armor Piece gets Equipped
            public float MagicDamage = 0.03f; //Increased Magic damage by Percent
            public int manaIncrease = 15; //Increased Mana when Armor is Equipped

            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Gaia Skirt");
                // Tooltip.SetDefault("Increased Armor Penetration by 1% \nIncreased maximum mana by 20 \nIncreased Magic damage by 3%");

                CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            }


            public override void SetDefaults()
            {
                Item.width = 32;
                Item.height = 32;
                Item.defense = 3;
                Item.rare = ItemRarityID.Green;
            }

            public override void UpdateEquip(Player player)
            {
                player.statManaMax2 += manaIncrease;
                player.GetArmorPenetration(DamageClass.Generic) += armorPen;
                player.GetDamage(DamageClass.Magic) += MagicDamage;
            }

            public override bool MagicPrefix()
            {
                return true;
            }

            public override void UpdateArmorSet(Player player)
            {
                player.setBonus = "The Armor gives you a Flower to Assist you Crit a bit and the Flower will Bloom you a reward";
            }


            public override bool IsArmorSet(Item head, Item body, Item legs)
            {
                return head.type == ModContent.ItemType<Gaia_Hat>() && body.type == ModContent.ItemType<Gaia_Robe>();
            }

            /*public override void AddRecipes()
            {
                Recipe mod = CreateRecipe(1);
                mod.AddIngredient<Leaf>(15);
                mod.AddIngredient(ItemID.JungleHat, 1);
                mod.AddIngredient(ItemID.Wood, 20);
                mod.AddTile(TileID.LivingLoom);
                mod.Register();
            }*/
        }

        private class LeafCooldown : ModBuff
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Gaia Passive Cooldown");
                // Description.SetDefault("Its a Cooldown");
                Main.debuff[Type] = true;
                BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            }
        }

        private class LeafActive : ModBuff
        {
            public override void SetStaticDefaults()
            {
                // DisplayName.SetDefault("Gaia Passive");
                // Description.SetDefault("Increases Damage by 1% of your current Health");
                BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            }
        }

        private class Flore : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                Main.projFrames[Projectile.type] = 6;
            }

            public override void SetDefaults()
            {
                Projectile.Size = new Vector2(66, 66);
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.friendly = true;
            }

            public static bool FlowerOpened;
            
            public override void AI()
            {
                if (FlowerOpened)
                {
                    Projectile.alpha = 0;
                    if (Projectile.ai[1] >= 1)
                    {
                        Projectile.ai[1]--;
                    }
                }

                Terraria.Player player = Main.player[Projectile.owner];
                Projectile.Center = player.Center - new Vector2(0, player.height + 20);
                if (!FlowerOpened)
                {
                    Projectile.position.Y +=
                        (float) MathFunctions.SineWave(4, 1f, (int) Projectile.ai[2]++ / 15f);
                }
                else
                {
                    Projectile.position.Y +=
                        (float) MathFunctions.SineWave(4, 0.5f, (int) Projectile.ai[2]++ / 25f);
                }

                Projectile.position.Y += player.gfxOffY;

                if(Projectile.ai[0] >= 60*5)
                {
                    if (Projectile.alpha <= 255)
                    {
                        Projectile.alpha++;
                    }
                }
                else
                {
                    Projectile.alpha -= 5;
                    if (Projectile.alpha <= 0 && Projectile.alpha - 5 <= 0)
                    {
                        Projectile.alpha = 0;
                    }
                }

                Projectile.ai[0]++;
                
                
                if (Projectile.ai[1] >= 200f & !FlowerOpened)
                {
                    if (++Projectile.frameCounter >= 4.5f)
                    {
                        Projectile.frameCounter = 0;
                        if (++Projectile.frame >= Main.projFrames[Projectile.type] - 1)
                        {
                            OpenFlower();
                        }
                    }
                }

                if (Projectile.ai[1] == 0 && FlowerOpened)
                {
                    if (++Projectile.frameCounter >= 4.5f)
                    {
                        Projectile.frameCounter = 0;
                        if (--Projectile.frame <= 1)
                        {
                            CloseFlower();
                        }
                    }
                }
            }

            public void OpenFlower()
            {
                Vector2 position = Projectile.position;
                for (int i = 0; i < 20; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, 107,
                        Main.rand.NextVector2Circular(8 , 8).X * 2, 
                        Main.rand.NextVector2Circular(8 , 8).Y * 2, 0,
                        new Color(255, 255, 255), 1f);
                }

                for (int i = 0; i < 3; i++)
                {
                    switch (Main._rand.Next(3))
                    {
                        case 0: 
                            Item.NewItem(Projectile.GetSource_Misc("Armor"), Projectile.Center, ModContent.ItemType<NatureHeart>(),
                                1);
                            break;
                        case 1:
                            Item.NewItem(Projectile.GetSource_Misc("Armor"), Projectile.Center, ModContent.ItemType<StarNature>(),
                                1);
                            break;
                        case 2:
                            Item.NewItem(Projectile.GetSource_Misc("Armor"), Projectile.Center, ModContent.ItemType<NaturaPower>(),
                                1);
                            break;
                    }
                }
                
                FlowerOpened = true;
            }

            public void CloseFlower()
            {
                FlowerOpened = false;
            }

            public override bool? CanCutTiles()
            {
                return false;
            }

            public override bool PreKill(int timeLeft)
            {
                return false;
            }

            public override bool? CanHitNPC(NPC target)
            {
                return false;
            }

            public override bool CanHitPvp(Player target)
            {
                return false;
            }

            public override bool CanHitPlayer(Player target)
            {
                return false;
            }
        }
    }
}