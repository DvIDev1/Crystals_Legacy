using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Crystals.Core.Systems
{
    public class PPlayer : ModPlayer
    {
        public bool ShowCurse = false;
        public int frame = 0;
        public int framecount;
        public bool happened = false;
        public int happen = 0;
        public bool retract = false;
        public bool anim2 = false;
        public int happens = 0;
        public int cursedcounter = 0;
        public int CurseShake = 0;
        public int screaming = 0;
        public bool ShowSlot = false;

        public override void ModifyScreenPosition()
        {
            if (CurseShake > 0)
            {
                Main.screenPosition += new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
                CurseShake--;
            }
        }

        public override void PostUpdate()
        {
            if (screaming > 0)
            {
                screaming--;
            }

            if (ShowCurse == true)
            {
                Main.LocalPlayer.AddBuff(BuffID.Obstructed, 1 * 1);
            }

            if (!Main.playerInventory)
            {
                if (ShowCurse == true)
                {
                    Main.playerInventory = true;
                    ShowCurse = false;
                    Main.hidePlayerCraftingMenu = false;
                    Main.craftingHide = false;
                }
            }
        }
    }
}
