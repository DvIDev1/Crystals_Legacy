using Terraria;
using Terraria.ModLoader;

namespace Crystals.Core.Globals
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
