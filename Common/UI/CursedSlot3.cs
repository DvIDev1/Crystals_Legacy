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
    internal class CursedSlot3 : UIState
    {

        private UIElement area;
        private UIImage loc;

        

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
            const int X = 1075;
            const int Y = 660;

            Vector2 locc = new Vector2(X, Y);

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Crystals/Common/UI/CursedSlot3");

            

            Rectangle sourceRect = new Rectangle(0, Main.LocalPlayer.GetModPlayer<PPlayer>().frame, texture.Width, 132);

            Main.EntitySpriteDraw(texture, locc, sourceRect, Color.White, 0f, sourceRect.Size() * 1.5f, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.GetModPlayer<PPlayer>().framecount > 0)
            {
                Main.LocalPlayer.GetModPlayer<PPlayer>().framecount--;
            }

            if (Main.LocalPlayer.GetModPlayer<PPlayer>().happened == true)
            {
                if (Main.LocalPlayer.GetModPlayer<PPlayer>().happen == 3)
                {
                    Main.LocalPlayer.GetModPlayer<PPlayer>().frame = 396;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happen += 1;
                }

                if (Main.LocalPlayer.GetModPlayer<PPlayer>().happen < 3)
                {

                    if (Main.LocalPlayer.GetModPlayer<PPlayer>().framecount == 0)
                    {
                        if (Main.LocalPlayer.GetModPlayer<PPlayer>().frame < 396)
                        {
                            Main.LocalPlayer.GetModPlayer<PPlayer>().frame += 132;
                        }
                        else
                        {
                            Main.LocalPlayer.GetModPlayer<PPlayer>().frame += 396;
                        }

                        Main.LocalPlayer.GetModPlayer<PPlayer>().framecount = 6;
                        Main.LocalPlayer.GetModPlayer<PPlayer>().happen += 1;
                    }
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<PPlayer>().cursedcounter = 0;

                    
                }
            }
            
            if (Main.LocalPlayer.GetModPlayer<PPlayer>().happened == false && Main.LocalPlayer.GetModPlayer<PPlayer>().retract == true)
            {
                if (Main.LocalPlayer.GetModPlayer<PPlayer>().happens == 2)
                {
                    Main.LocalPlayer.GetModPlayer<PPlayer>().retract = false;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happens += 1;
                    
                }

                if (Main.LocalPlayer.GetModPlayer<PPlayer>().happens < 2)
                {
                    if (Main.LocalPlayer.GetModPlayer<PPlayer>().framecount == 0)
                    {
                        if (Main.LocalPlayer.GetModPlayer<PPlayer>().frame < 796)
                        {
                            Main.LocalPlayer.GetModPlayer<PPlayer>().frame += 132;
                        }
                        else
                        {
                            Main.LocalPlayer.GetModPlayer<PPlayer>().frame = 660;
                        }

                        Main.LocalPlayer.GetModPlayer<PPlayer>().framecount = 6;
                        Main.LocalPlayer.GetModPlayer<PPlayer>().happens += 1;
                    }
                }
            }



            if (Main.LocalPlayer.GetModPlayer<PPlayer>().retract == false && Main.LocalPlayer.GetModPlayer<PPlayer>().happened == false)
            {

                if (Main.LocalPlayer.GetModPlayer<PPlayer>().framecount == 0)
                {
                    if (Main.LocalPlayer.GetModPlayer<PPlayer>().frame < 132)
                    {
                        Main.LocalPlayer.GetModPlayer<PPlayer>().frame += 132;
                    }
                    else
                    {
                        Main.LocalPlayer.GetModPlayer<PPlayer>().frame = 0;
                    }

                    Main.LocalPlayer.GetModPlayer<PPlayer>().framecount = 5;
                }
            }


            base.Update(gameTime);
        }
    }



    class CursedSlotSyst3 : ModSystem
    {
        public UserInterface CurseInterface2;

        internal CursedSlot3 Curse2;

        public override void Load()
        {

            if (!Main.dedServ)
            {
                Curse2 = new();
                CurseInterface2 = new();
                CurseInterface2.SetState(Curse2);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            CurseInterface2?.Update(gameTime);
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
                            CurseInterface2.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }
    }

}
