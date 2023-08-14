
using System;
using Crystals.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Consumables.Food.CursedSalad
{
    public class CursedSalad : ModItem
    {
        public override string Texture => AssetDirectory.Consumables + Name; 
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cursed Salad");
        }

        public override void Load()
        {
            //Translation Texts 
            //Todo Fix localization
            LocalizedText SaladWarning = Language.GetOrRegister(Crystals.Instance, "SaladWarning" , () => "You shouldn't have done this");
            // SaladWarning.SetDefault("You shouldn't have done this");
            
            LocalizedText SaladWarning1 = Language.GetOrRegister(Crystals.Instance, "SaladWarning1" ,  () => "This was on the Ground");
            // SaladWarning1.SetDefault("This was on the Ground");
            
            LocalizedText SaladWarning2 = Language.GetOrRegister(Crystals.Instance, "SaladWarning2" ,  () => "Someone's Kid Could have been in there");
            // SaladWarning2.SetDefault("Someone's Kid Could have been in there");
            
            LocalizedText SaladWarning3 = Language.GetOrRegister(Crystals.Instance, "SaladWarning3" ,  () => "I Hope you choke on that");
            // SaladWarning3.SetDefault("I Hope you choke on that");
            
            LocalizedText SaladWarning4 = Language.GetOrRegister(Crystals.Instance, "SaladWarning4" , () => "This salad doesn't belong to you, you will pay for that!");
            // SaladWarning4.SetDefault("This salad doesn't belong to you, you will pay for that!");
        }

        public override void SetDefaults()
        {
            Item.DefaultToFood(36 , 20 , BuffID.Venom , 3600 , false , 17);
        }

        public override bool? UseItem(Player player)
        {
            int chance = new Random().Next(5);
            Rectangle rec = player.Hitbox;
            switch (chance)
            {
                case 0:
                    CombatText.NewText(rec, Color.Purple, Language.GetTextValue("Mods.Crystals.SaladWarning"), false, false);
                    break;
                case 1: 
                    CombatText.NewText(rec, Color.Purple, Language.GetTextValue("Mods.Crystals.SaladWarning1"), false, false);
                    break;
                case 2:
                    CombatText.NewText(rec, Color.Purple, Language.GetTextValue("Mods.Crystals.SaladWarning2"), false, false);
                    break;
                case 3:
                    CombatText.NewText(rec, Color.Purple, Language.GetTextValue("Mods.Crystals.SaladWarning3"), false, false);
                    break;
                case 4:
                    CombatText.NewText(rec, Color.Purple, Language.GetTextValue("Mods.Crystals.SaladWarning4"), false, false);
                    break;
                default:
                    CombatText.NewText(rec, Color.Purple, Language.GetTextValue("Mods.Crystals.SaladWarning"), false, false);
                    break;
            }
            return true;
        }
    }
}