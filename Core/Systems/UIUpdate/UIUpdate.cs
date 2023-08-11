using Crystals;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace Crystals
{
    public class UIUpdate : ModSystem
    {
        public override void UpdateUI(GameTime gameTime)
        {
            GetInstance<Crystals>()._CrusoliumUIInterface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "Crystals: Crusolium Armor Bar",
                    delegate
                    {
                        GetInstance<Crystals>()._CrusoliumUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}