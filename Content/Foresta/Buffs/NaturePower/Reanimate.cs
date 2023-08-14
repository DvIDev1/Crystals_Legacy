using Crystals.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Buffs.NaturePower
{
    public class Reanimate : ModBuff
    {
        public override string Texture => AssetDirectory.Buffs + Name;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal of the Forest");
            // Description.SetDefault("Keeps you alive for as Long as Possible");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLife = player.statLifeMax2;
            player.statMana = player.statManaMax2;
        }
    }
}