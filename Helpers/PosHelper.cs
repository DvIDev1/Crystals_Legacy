using Microsoft.Xna.Framework;
using Terraria;

namespace Crystals.Helpers
{
    public class PosHelper
    {
        public static Vector2 GetPlayerArmPosition(Player player)
        {
            Vector2 vector = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector.X = player.bodyFrame.Width - vector.X;
            }
            if (player.gravDir != 1f)
            {
                vector.Y = player.bodyFrame.Height - vector.Y;
            }
            vector -= new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f;
            Vector2 pos = player.MountedCenter - new Vector2(20f, 42f) / 2f + vector + Vector2.UnitY * player.gfxOffY;
            if (player.mount.Active && player.mount.Type == 52)
            {
                pos.Y -= player.mount.PlayerOffsetHitbox;
                pos += new Vector2(12 * player.direction, -12f);
            }
            return player.RotatedRelativePoint(pos);
        }
        
    }
}