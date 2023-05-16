using System;
using Crystals.Content.Foresta.Events;
using DiscordRPC;
using DiscordRPC.Logging;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Crystals
{
	public class Crystals : Mod
	{
		//Todo Recode item Tooltips and Names 

		public static Crystals Instance { get; set; }

		public Crystals()
		{
			Instance = this;
		}

		public override void Unload()
		{
			if (!Main.dedServ)
			{
				Instance = null;
			}


		}

	}
}