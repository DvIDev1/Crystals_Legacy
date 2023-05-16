
using Crystals.Content.Foresta.Buffs.NaturePower;
using Crystals.Content.Foresta.Items.Armors.Magic.Gaia.Items.PickUps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Crystals.GlobalBuffs.ShakingBuff
{
    public class ShakingBuff : GlobalBuff
    {
        public override bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
        {
            if (type == ModContent.BuffType<NaturePower>())
            {
                var shake = new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1));

                drawParams.Position += shake;
                drawParams.TextPosition += shake;
            }
            
            if (type == ModContent.BuffType<NaturaPower.NatureBoost>())
            {
                var shake = new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1));

                drawParams.Position += shake;
                drawParams.TextPosition += shake;
            }

            return true;
        }
    }
}