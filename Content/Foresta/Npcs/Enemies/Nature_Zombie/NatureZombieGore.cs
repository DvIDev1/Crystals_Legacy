using Crystals.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Nature_Zombie
{
    public class NatureZombieGore
    {
        public class NatureZombieHead : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class NatureZombieHead2 : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class NatureZombieHead3 : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class NatureZombieLeg : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class NatureZombieArm : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class NatureZombieArm2 : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class NatureZombieTorso : ModGore
        {
            public override string Texture => AssetDirectory.NatureZombie + Name;

            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }
    }
}