using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;

namespace Crystals.Helpers
{
    public class Pathfinding
    {
        public static Vector2 GetNearestPos(Vector2 OriginalPos , List<Vector2> Positions)
        {
            IEnumerable<Vector2> query = Positions.OrderBy(pos => pos.Distance(OriginalPos));

            return query.First();
        }

        public static Entity GetNearestEntity(Entity entity, List<Entity> entities)
        {
            IEnumerable<Entity> query = entities.OrderBy(ent => ent.Distance(entity.Center));

            return query.First();
        }

        public static NPC GetNearestNPC(NPC npc, List<NPC> npcs)
        {
            IEnumerable<NPC> query = npcs.OrderBy(np => np.Distance(npc.Center));

            return query.First();
        }

    }
}