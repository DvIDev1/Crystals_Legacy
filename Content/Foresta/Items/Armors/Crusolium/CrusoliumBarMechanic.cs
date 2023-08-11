using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace Crystals.Content.Foresta.Items.Armors.Crusolium
{
    public class CrusoliumBarMechanic : UIState
    {
        private UIImage panel;
        private Color gradientA;
        private Color gradientB;
        private Color gradientC;
        public override void OnInitialize()
        {
            panel = new UIImage(Request<Texture2D>("Crystals/Content/Foresta/Items/Armors/Crusolium/CrusoliumBarMechanic", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            panel.SetPadding(0);
            panel.HAlign = panel.VAlign = 0.5f;
            panel.Width.Set(32, 0f);
            panel.Height.Set(8, 0f);
            panel.Top.Set(40, 0f);
            panel.Left.Set(-4, 0f);

            // SetRectangle(panel, left: 900f, top: 700f, width: 32, height: 8);

            gradientA = new Color(159, 174, 39);
            gradientB = new Color(221, 226, 78);
            gradientC = new Color(56, 57, 63);

            Append(panel);
        }
        /* this is useful, but not now
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
        */
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.LocalPlayer.GetModPlayer<Cruso_ArmorSet>().hasCrusoSet())
                return;
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            // Calculate quotient
            float quotient = Cruso_ArmorSet.charge / 100f; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
            quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

            float quotient2 = Cruso_ArmorSet.charge; // Lazy way to draw the gray background
            quotient2 = Utils.Clamp(quotient2, 1f, 1f);

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = panel.GetInnerDimensions().ToRectangle();
            hitbox.X += 4;
            hitbox.Width += 1;
            hitbox.Y += 4;
            hitbox.Height += 1;

            int left2 = hitbox.Left;
            int right2 = hitbox.Right;
            int steps2 = (int)((right2 - left2) * quotient2);
            for (int i = 0; i < steps2; i += 1)
            {
                // Lazy way to draw the gray background
                float percent = (float)i / (right2 - left2);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left2 + i + 2, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientC, gradientC, percent));
            }
            // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                //float percent = (float)i / steps; // Alternate Gradient Approach
                float percent = (float)i / (right - left);

                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i + 2, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.GetModPlayer<Cruso_ArmorSet>().hasCrusoSet())
                return;
            base.Update(gameTime);
        }
    }
}
