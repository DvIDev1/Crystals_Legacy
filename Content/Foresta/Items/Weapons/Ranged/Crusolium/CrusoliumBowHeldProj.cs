using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics;

namespace Crystals.Content.Foresta.Items.Weapons.Ranged.Crusolium
{
    /// <summary>
    /// ai0 is the max charge, being how much time it take to charge the bow. It's similar to use time. Lower = faster.
    /// ai1 is how much the bow is charged by. It's a counter. Spawn it with higher than 0 to make it charge faster the first time.
    /// </summary>
    public class CrusoliumBowStringless : ModProjectile
    {
        public override string Texture => "Crystals/Content.Foresta/Items/Weapons/Ranged/CrusoliumbowStringless";
";
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public float ChargeAmount { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
        public float MaxCharge { get => Projectile.ai[0]; }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (ChargeAmount < MaxCharge)
                ChargeAmount = !player.channel ? 0 : ChargeAmount + 1 > MaxCharge ? MaxCharge : ChargeAmount + 1;
            if (!player.channel && ChargeAmount >= MaxCharge)
            {
                //fire arrow here
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="controlPoints"> THIS NEEDS TO BE AT LEAST 4 IN LENGTH OR YOU'LL GET INDEX OUT OF BOUNDS!!! It won't factor in points after the 4th one</param>
        /// <param name="t"> from 0 to 1</param>
        /// <returns>returns an interpolatd point</returns>
        static Vector2 CubicBezier(Vector2[] controlPoints, float t)
        {
            float tSquared = t * t;
            float tCubed = t * t * t;
            return
                -(controlPoints[0] * (-tCubed + (3 * tSquared) - (3 * t) - 1) +
                controlPoints[1] * ((3 * tCubed) - (6 * tSquared) + (3 * t)) +
                controlPoints[2] * ((-3 * tCubed) + (3 * tSquared)) +
                controlPoints[3] * tCubed);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 offsetToStringTip = new(0, 16);
            Vector2[] bezierPoints = new Vector2[4];
            List<Vector2> listForPoints = new();
            List<float> listForRotations;
            for (float i = 0; i < 4; i++)
            {
                //yes this is unreadable on purpose
                bezierPoints[(int)i] = new Vector2((1 - ((i * i + i * 3) / 2) - 1) * ChargeAmount / MaxCharge, i * 8 - 16) + Projectile.Center;
            }
            for (float i = 0; i < 100; i++)//probably doesn't need to be this high but uh idk
            {
                listForPoints.Add(CubicBezier(bezierPoints, (float)i / 100f));
                //listForRotations.Add(CubicBezier(bezierPoints, ((float)i + 0.01f) / 100f)).ToRotation());//THIS IS AN UNFINISHED LINE
            }
            VertexStrip bowString = new();
            //bowString.PrepareStrip
            return false;
        }
    }
}
