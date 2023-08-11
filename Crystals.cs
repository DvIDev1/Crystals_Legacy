using System;
using Crystals.Content.Foresta.Events;
using Crystals.Content.Foresta.Items.Armors.Crusolium;
using Crystals.Core.Systems.ParticleSystemAttempt;
using DiscordRPC;
using DiscordRPC.Logging;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
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
        internal UserInterface _CrusoliumUIInterface;
        internal CrusoliumBarMechanic CrusoliumUI;
        public Crystals()
		{
			Instance = this;
		}
        public override void Load()
        {
            if (!Main.dedServ)
            {
				CrusoliumUI = new CrusoliumBarMechanic();
                _CrusoliumUIInterface = new UserInterface();
                _CrusoliumUIInterface.SetState(CrusoliumUI);
            }
        }
        public override void Unload()
		{
			for (int i = 0; i < ParticleSystem.particles.Length; i++)
			{
				ParticleSystem.particles[i] = null;
			}
			if (!Main.dedServ)
			{
				Instance = null;
			}
		}


	}
}