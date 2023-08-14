﻿using Crystals.Content.Foresta.Items.Accessories;
using Crystals.Core;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Content.Other.Items.Pet.SagisPet;

// You can find a simple pet example in ExampleMod\Content\Pets\ExamplePet
public class Sagibuff : ModBuff
{
    public override string Texture => AssetDirectory.Pet + Name;

    public override void SetStaticDefaults()
	{
		Main.buffNoTimeDisplay[Type] = true;
		Main.vanityPet[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex)
	{ // This method gets called every frame your buff is active on your player.
		bool unused = false;
		player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<Sagi>());
	}
}