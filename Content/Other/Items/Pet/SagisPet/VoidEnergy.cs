using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Other.Items.Pet.SagisPet;

	public class VoidEnergy : ModItem
	{
		public override void SetDefaults()
        {
			Item.DefaultToVanitypet(ModContent.ProjectileType<Sagi>(), ModContent.BuffType<Sagibuff>()); // Vanilla has many useful methods like these, use them! It sets rarity and value aswell, so we have to overwrite those after

			//TODO Tooltip & Dedicated Item
			
			Item.width = 28;
			Item.height = 20;
			Item.rare = ItemRarityID.Master;
			Item.value = Item.sellPrice(0, 5);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2); 

			return false;
		}
	}
