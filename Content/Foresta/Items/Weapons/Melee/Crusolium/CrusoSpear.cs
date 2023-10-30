using System.Threading.Channels;
using Crystals.Core;
using Crystals.Core.Systems.PlayerAbilitySystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Weapons.Melee.Crusolium
{
	public class CrusoSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
			ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
		}
		
		public override string Texture => AssetDirectory.Melee + "CrusoSpear";

		public override void SetDefaults()
		{
			// Common Properties
			Item.rare = ItemRarityID.Lime; // Assign this item a rarity level of Pink
			Item.value = Item.sellPrice(gold: 3); // The number and type of coins item can be sold for to an NPC

			// Use Properties
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.useAnimation = 35; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useTime = 35; // The length of the item's use time in ticks (60 ticks == 1 second.)
			Item.UseSound = SoundID.DD2_GhastlyGlaivePierce; // The sound that this item plays when used.
			Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			// Weapon Properties
			Item.damage = 29;
			Item.knockBack = 6.5f;
			Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            Item.channel = true;
            // Projectile Properties
            Item.shootSpeed = 12f; // The speed of the projectile measured in pixels per frame.
			Item.shoot = ModContent.ProjectileType<CrusoSpearProj>(); // The projectile that is fired from this weapon
		}
		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			// Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			
			Vector2 dest = Main.MouseWorld;
			Vector2 startPos = player.Center;
			if (player.altFunctionUse == 2 && 
			    Framing.GetTileSafely(Main.MouseWorld.ToTileCoordinates16()).LiquidAmount == 0 && 
			    !player.ZoneLihzhardTemple &&
			    Framing.GetTileSafely(Main.MouseWorld.ToTileCoordinates16()) != null 
			    && CrusoAbility.HitCount == 5)
			{
				PlayerSliceDash.active = true;
				PlayerSliceDash.time = 15;
				PlayerSliceDash.dest = dest;
				PlayerSliceDash.startPos = startPos;
				PlayerSliceDash.damage = Item.damage * 2;
				CrusoAbility.HitCount = 0;
			}

			return null;
		}
		
		/*public override bool CanShoot(Player player)
		{
			return player.altFunctionUse != 2;
		}*/


		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public class CrusoAbility : ModPlayer
		{

			public static int HitCount = 0;

		}
		
	}
}


