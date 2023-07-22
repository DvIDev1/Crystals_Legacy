using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Friendly.PumpkinMan
{
    public class PumpkinMan : ModNPC
    {
	    
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

		public override void SetDefaults() {
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
		
		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}
        
    }
}