using Terraria.Audio;

namespace Crystals.Core.Systems.SoundSystem
{
    public class SoundSystem
    {
        public static readonly SoundStyle FeracorShot;
        
        public static readonly SoundStyle ChargeBow;
        
        public static readonly SoundStyle Cursed;

        static SoundSystem()
        {
            FeracorShot = new SoundStyle("Crystals/Sounds/Items/Feracor/FeracorShot", (SoundType)0);
            ChargeBow = new SoundStyle("Crystals/Sounds/NPCs/Warriors/SFX_CrossbowLoad", (SoundType)0);
            Cursed = new SoundStyle("Crystals/Sounds/UI/Cursed", (SoundType)0);
        }

    }
}
