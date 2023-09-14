using System.Linq;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.PlayerAbilitySystem;

public class PlayerSliceDash : ModPlayer
{
    
    public static bool active;
    public static float time = 60;
    public static Vector2 dest = new Vector2(0);
    public static Vector2 startPos = new Vector2(0);
    public static int damage;

    private float timer;

    public override void PreUpdateMovement()
    {
        if (active)
        {
            timer++;
            Player.velocity =   
                Vector2.SmoothStep(Player.velocity, Player.DirectionTo(dest) * 15, MathFunctions.EaseFunctions.EaseInBack(timer / time));
        }
        
        if (timer >= time)
        {
            VisualHelper.DrawDustLine(startPos , dest , 8 , 107);
            Player.Teleport(dest , TeleportationStyleID.DebugTeleport , 0);

            foreach (var npcs in Main.npc)
            {
                if (Collision.CheckAABBvLineCollision(npcs.TopLeft , npcs.Size , startPos , dest))
                {
                    npcs.StrikeNPC(npcs.CalculateHitInfo(damage , 0 , true , KnockbackValue.LowAverageknockback , DamageClass.Melee , true) , false , false);
                }
            }
            timer = 0;
            active = false;
        }
    }

    public override void ModifyScreenPosition()
    {
        Vector2 centerScreen = new Vector2(Main.screenWidth/2f,Main.screenHeight/2f);
        
        if (active && Collision.CanHitLine(startPos, Player.width, Player.height, dest, Player.width, Player.height))
        {
            Main.screenPosition = Vector2.SmoothStep(startPos - centerScreen, dest - centerScreen, timer / time);
        }
    }

    /*public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (Collision.CanHitLine(startPos, Player.width, Player.height, dest, Player.width, Player.height))
        {
            foreach (var npcs in Main.npc)
            {
                VisualHelper.AABBLineVisualizer(startPos , dest , npcs.width);
                if (Collision.CheckAABBvLineCollision(npcs.TopLeft, npcs.Size, startPos, dest))
                {
                    Main.NewText("Yes");
                    npcs.HitEffect(0, 50, null);
                }
            }

            
        }
    }*/

    public override void ResetEffects()
    {
        if (!active)
        {
            time = 60;
            dest = new Vector2(0);
            startPos = new Vector2(0);
        }
    }
    
}