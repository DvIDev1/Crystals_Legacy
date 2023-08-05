using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace Crystals.Core.Systems.Globals
{
    public class GlobaItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.damage >= 1)
            {
                item.prefix = -3;
            }

            base.SetDefaults(item);
        }

        
    }
}
