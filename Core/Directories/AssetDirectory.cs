namespace Crystals.Core
{
	//How to use AssetDirectory:
	//public ovverride string Texture => Assetdirectory.NameOfFolder + Name;
	//(terraria automatically detects "name" as the name of the class so you can leave it like this but if you need to add a subfolder make sure to add + "/NameOfSubFolder" before name)
	public static class AssetDirectory
	{
		public const string Assets = "Crystals/Assets/";
		public const string ForestaAssets = Assets + "Foresta/";
		public const string MiscAssets = Assets + "Misc/";
		public const string OtherAssets = Assets + "Other/";

        #region Items
        public const string Items = ForestaAssets + "Items/";
		public const string Accessories = Items + "Accessories/";
		public const string Armors = Items + "Armors/";
		public const string CrusoliumArmor = Armors + "Crusolium/";
		public const string GaiaArmor = Armors + "Gaia/";
		public const string GaiaItems = GaiaArmor + "Items/";
		public const string Banners = Items + "Banners/";
		public const string Consumables = Items + "Consumables/";
		public const string Tools = Items + "Tools/";
        #endregion

        #region Weapons
		public const string Weapons = Items + "Weapons/";
        public const string Magic = Weapons + "Magic/";
		public const string Melee = Weapons + "Melee/";
		public const string Ranged = Weapons + "Ranged/";
		public const string Summoner = Weapons + "Summoner/";
        #endregion

        #region NPCs
        public const string NPCs = ForestaAssets + "NPCs/";
		public const string CursedSpirit = NPCs + "CursedSpirit/";
        public const string EnemyPumpkin = NPCs + "EnemyPumpkin/";
        public const string Forest_Spirit = NPCs + "Forest_Spirit/";
        public const string Leafling = NPCs + "Leafling/";
        public const string Gold_Leafling = Leafling + "Gold/";
        public const string NatureSlime = NPCs + "Nature_Slime/";
        public const string NatureZombie = NPCs + "Nature_Zombie/";
        public const string Sunny = NPCs + "Sunny/";
        public const string Warriors = NPCs + "Warriors/";
		public const string PumpkinMan = NPCs + "PumpkinMan/";
        #endregion

        #region Tiles
        public const string Tiles = MiscAssets + "Tiles/";
        #endregion

        #region Other
        public const string OtherItems = OtherAssets + "OtherItems/";
		public const string Pet = OtherItems + "Pet/";
		public const string Vanity = OtherAssets + "Vanity/";
		public const string Maid = Vanity + "Maid";
        #endregion

        #region Buffs
        public const string Buffs = ForestaAssets + "Buffs/";
        #endregion
    }
}