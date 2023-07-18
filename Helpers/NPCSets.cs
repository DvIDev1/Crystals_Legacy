using Crystals.Content.Foresta.Npcs.Enemies.CursedSpirit;
using Crystals.Content.Foresta.Npcs.Enemies.Forest_Spirit;
using Crystals.Content.Foresta.Npcs.Enemies.Warriors;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Helpers;

public class NPCSets
{

    public static bool[] Spirit = NPCID.Sets.Factory.CreateBoolSet(false, ModContent.NPCType<ForestSpirit>(),
        ModContent.NPCType<CursedSpirit>(), NPCID.DungeonSpirit);
    
    public static bool[] OvergrownWarrior = NPCID.Sets.Factory.CreateBoolSet(false, ModContent.NPCType<OvergrownWarrior>(),
        ModContent.NPCType<OvergrownArcher>(), ModContent.NPCType<OvergrownPaladin>() , ModContent.NPCType<OvergrownKnight>());

}