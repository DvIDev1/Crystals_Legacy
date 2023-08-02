using Crystals.Helpers;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors;

public class OvergrownMage : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 15;
    }

    private int xFrame;

    private int yFrame;

    private Entity target;
    
    public override void SetDefaults()
    {
        NPC.width = 60;
        NPC.height = 50;
        NPC.defense = 4;
        NPC.damage = 21;
        NPC.lifeMax = 120;
        NPC.value = ValueHelper.GetCoinValue(0, 0, 15, 89);
        NPC.aiStyle = -1;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath2;
        NPC.noGravity = false;
        NPC.noTileCollide = false;
        NPC.knockBackResist = 0.3f;
    }
    
    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
        {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
            new FlavorTextBestiaryInfoElement(
                "With pure intentions once, their heart ablaze, " +
                "Using light magic to mend and amaze. Yet darkness seeped in, " +
                "and their judgment blurred, Now striking down the wrong, their light obscured.")
        });
    }
    
}