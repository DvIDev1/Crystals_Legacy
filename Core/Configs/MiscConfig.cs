using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Crystals.Core.Configs;

public class MiscConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Header("DiscordRPC")]
    [DefaultValue(true)]
    public bool Enabled;
}