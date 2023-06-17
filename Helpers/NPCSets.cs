using Crystals.Content.Foresta.Npcs.Enemies.CursedSpirit;
using Crystals.Content.Foresta.Npcs.Enemies.Forest_Spirit;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Helpers;

public class NPCSets
{

    public static bool[] Spirit = NPCID.Sets.Factory.CreateBoolSet(false, ModContent.NPCType<ForestSpirit>(),
        ModContent.NPCType<CursedSpirit>(), NPCID.DungeonSpirit);

}