using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Crystals.Common.Sfx
{
    public class Audio : ModSystem
    {
        public static readonly SoundStyle Cursed;

        static Audio()
        {
            Cursed = new SoundStyle("Crystals/Common/Sfx/Cursed", (SoundType)0);

        }
    }
}
