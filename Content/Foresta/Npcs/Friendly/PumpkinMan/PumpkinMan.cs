
using Crystals.Core.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Crystals.Core;

namespace Crystals.Content.Foresta.Npcs.Friendly.PumpkinMan
{
    public class PumpkinMan : ModNPC
	{
        public override string Texture => AssetDirectory.PumpkinMan + Name;

        const int X = 958;
		const int Y = 530;

        Vector2 locc = new Vector2(X, Y);

        public const string name = "[c/5B33FF:Devastate]";

        private static Profiles.StackedNPCProfile NPCProfile;
	    
	    private static int ShimmerHeadIndex;
        
        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; 

			NPCID.Sets.ExtraFramesCount[Type] = 9; 
			NPCID.Sets.AttackFrameCount[Type] = 4; 
			NPCID.Sets.DangerDetectRange[Type] = 500; 
			NPCID.Sets.AttackType[Type] = 2; 
			NPCID.Sets.AttackTime[Type] = 90; 
			NPCID.Sets.AttackAverageChance[Type] = 15; 
			NPCID.Sets.HatOffsetY[Type] = 4; 
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true; 
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) {
				Velocity = 1f,
				Direction = 1 
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Hate) 
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike) 
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like) 
				.SetBiomeAffection<JungleBiome>(AffectionLevel.Love)
				.SetNPCAffection(NPCID.Princess, AffectionLevel.Hate) 
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Dislike) 
				.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Like) 
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Like)
			;
			
			
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
			);
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = name;
		}

		public override void AI()
		{

			if (NPC.Distance(Main.LocalPlayer.Center) > 170)
			{
				Main.LocalPlayer.GetModPlayer<PPlayer>().ShowCurse = false;
				Main.LocalPlayer.GetModPlayer<PPlayer>().ShowSlot = false;
			}
			
			if (Main.LocalPlayer.GetModPlayer<PPlayer>().ShowCurse == true)
			{
				NPC.velocity.X = 0;
				NPC.velocity.Y = 0;
			}

		}


		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
            if (firstButton)
            {
                if (Main.LocalPlayer.GetModPlayer<PPlayer>().ShowCurse == false)
                {
					Main.CloseNPCChatOrSign();
					Main.playerInventory = true;
					Main.LocalPlayer.GetModPlayer<PPlayer>().ShowCurse = true;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().ShowSlot = true;
                    Main.craftingHide = true;


                    //SoundEngine.PlaySound(Audio. , player.position);
                    return;
                }
            }

        }

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}
        
    }
}