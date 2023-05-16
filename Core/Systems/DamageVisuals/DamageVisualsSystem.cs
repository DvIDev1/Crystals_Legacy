using Crystals.Content.Foresta.Items.Armors.Magic.Gaia.Items.PickUps;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.DamageVisuals
{
    public class DamageVisualsSystem : ModPlayer
    {
        public override void OnHitAnything(float x, float y, Entity victim)
        {
            if (Player.HasBuff<NaturaPower.NatureBoost>())
            {
                Vector2 position = new Vector2(x , y);
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(position, 5, 5, 74,
                        Main.rand.NextVector2Circular(5 , 5).X * 2, 
                        Main.rand.NextVector2Circular(5 , 5).Y * 2, 0,
                        new Color(255, 255, 255), 1f);
                }
            }
        }
        
    }
}