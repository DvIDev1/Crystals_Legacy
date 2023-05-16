using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Npcs.Enemies.Warriors
{
    public class OvergrownWarriorsGore
    {
        public class WarriorHead : ModGore
        {
            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class WarriorLeg : ModGore
        {
            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class WarriorTorso : ModGore
        {
            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class WarriorArm : ModGore
        {
            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class WarriorArm2 : ModGore
        {
            public override void OnSpawn(Gore gore, IEntitySource source)
            {
                gore.behindTiles = false;
            }

            public override bool Update(Gore gore)
            {
                return true;
            }
        }

        public class WarriorSword : ModGore
        {
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