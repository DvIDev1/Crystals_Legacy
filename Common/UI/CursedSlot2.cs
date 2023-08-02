using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Crystals.Common.Systems;

namespace Crystals.Common.UI
{
    internal class CursedSlot2 : UIState
    {

        private UIElement area;
        private UIImage loc;

        int frame = 0;
        int framecount;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(851, 0f);
            area.Top.Set(350, 0f);
            area.Width.Set(0, 0f);
            area.Height.Set(0, 0f);


            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            const int X = 958;
            const int Y = 530;

            Vector2 locc = new Vector2(X, Y);

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Crystals/Common/UI/CursedSlot2");


            Rectangle sourceRect = new Rectangle(0, frame, texture.Width, 136);

            Main.EntitySpriteDraw(texture, locc, sourceRect, Color.White, 0f, sourceRect.Size() / 2f, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (framecount > 0)
            {
                framecount--;
            }

            if (framecount == 0)
            {
                if (frame < 816)
                {
                    frame += 136;
                }
                else
                {
                    frame = 0;
                }

                framecount = 5;
            }

            

            base.Update(gameTime);
        }
    }



    class CursedSlotSyst2 : ModSystem
    {
        public UserInterface CurseInterface1;

        internal CursedSlot2 Curse1;

        public override void Load()
        {

            if (!Main.dedServ)
            {
                Curse1 = new();
                CurseInterface1 = new();
                CurseInterface1.SetState(Curse1);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            CurseInterface1?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (resourceBarIndex != -1)
            {
                var player = Main.LocalPlayer;

                if (player.GetModPlayer<PPlayer>().ShowCurse == true)
                {
                    layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                        "Crystals: CursingUIBG",
                        delegate
                        {
                            CurseInterface1.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }
    }

}
