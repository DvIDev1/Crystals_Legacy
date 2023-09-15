using Terraria;

namespace Crystals.Helpers;

public class ConditionHelper
{
    
    public static Condition InForest = new ("Mods.Crystals.Conditions.InForest" , () => Main.LocalPlayer.ZoneForest);
    
}