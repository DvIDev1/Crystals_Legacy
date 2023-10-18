using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Crystals.Core.Systems.TitleSystem;

public class Title : ModSystem
{
    public static string text = "Title";
    public static string subtext = "Subtitle";
    public static Color color = Color.White;
    public static float ActiveTime  = 0;

    public override void PostDrawInterface(SpriteBatch spriteBatch)
    {

        if (ActiveTime > 0) {
            ActiveTime--;
            float max = 30f;
            float num = ActiveTime;
            float alpha = 0f;
            if (num > 210f) {
                alpha = (num - 210f)/30f;
                alpha = 1f - alpha;
            }
            else
            {

                if (num > max)
                {
                    num = max;
                }

                alpha = (num / max);
            }

            var font = FontAssets.DeathText.Value;
            
            if (text == "") {return;}
            TextSnippet[] snippets = ChatManager.ParseMessage(text, (color*alpha)).ToArray();
            Vector2 messageSize = ChatManager.GetStringSize(font, snippets, Vector2.One);
            Vector2 pos = new Vector2(Main.screenWidth/2f,Main.screenHeight/2f);
            float offset = messageSize.Y / 2f;
            pos = pos.Floor();
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, snippets, pos, 0f, new Vector2(messageSize.X / 2f,messageSize.Y / 2f), new Vector2(1,1), out int hover);

            if (subtext != "") {
                snippets = ChatManager.ParseMessage(subtext, (color*alpha)).ToArray();
                messageSize = ChatManager.GetStringSize(font, snippets, Vector2.One);
                pos = new Vector2(Main.screenWidth/2f,Main.screenHeight/2f);
                pos.Y += offset + 5f;
                pos = pos.Floor();
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, snippets, pos, 0f, new Vector2(messageSize.X / 2f,messageSize.Y / 2f), new Vector2(0.5f,0.5f), out hover);
            }
        }

    }
}