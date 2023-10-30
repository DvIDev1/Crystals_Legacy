using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Crystals.Core.Systems
{
    internal class NewLocalisationSys : ModSystem
    {
        public override void PostSetupContent()
        {
            MethodInfo refreshInfo = typeof(LocalizationLoader).GetMethod("UpdateLocalizationFilesForMod", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(Mod), typeof(string), typeof(GameCulture) });
            refreshInfo.Invoke(null, new object[] { Mod, null, Language.ActiveCulture });

        }
    }

    internal static class NewLocalisationRoundabout
    {
        public static void SetDefault(this LocalizedText text, string value)
        {
            PropertyInfo valueProp = typeof(LocalizedText).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

            LanguageManager.Instance.GetOrRegister(text.Key, () => value);
            valueProp.SetValue(text, value);
        }
    }
}
