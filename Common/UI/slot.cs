using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.GameContent;
using System.Collections.Generic;

namespace Crystals.Common.UI
{
    internal class Slot : UIElement
    {
        internal Item Item;
        private readonly int contex;
        private readonly float scal;
        internal Func<Item, bool> ValidItemFunc;
        private Color Color;

        public override void OnInitialize()
        {
            Color = new Color(0, 0, 0);

            base.OnInitialize();
        }
        public Slot(int context = ItemSlot.Context.TrashItem, float scale = 1f)
        {
            contex = context;
            scal = scale;
            Item = new Item();
            Item.SetDefaults();


            Width.Set(50 * scale, 0f);
            Height.Set(50 * scale, 0f);

        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = scal;
            Rectangle rectangle = GetDimensions().ToRectangle();

            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ValidItemFunc == null || ValidItemFunc(Main.mouseItem))
                {
                    
                    ItemSlot.Handle(ref Item, contex);
                }
            }
            
            ItemSlot.Draw(spriteBatch, ref Item, contex, rectangle.TopLeft());
            Main.inventoryScale = oldScale;
        }
    }
}