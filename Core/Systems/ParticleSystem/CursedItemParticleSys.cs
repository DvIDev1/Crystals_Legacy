using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Crystals.Common.UI;
using Crystals.Content.Other.Misc.Prefixes;
using ReLogic.Content;
using Terraria.Graphics.Shaders;
using DiscordRPC;
using Terraria.WorldBuilding;

namespace Crystals.Core.Systems.ParticleSystem
{
    internal class CursedItemParticleSys : GlobalItem
    {
        /*the shader uses 3 values for colors,i called these values R_value;G_value;B_value (Red,Green,Blue) these values are float so you can use anything ranging from 0 to 1 i left this note so if enyone wants to add more prefixes they can do it without my intervention 
        fuck around and find out what color u need :skull:
        TODO add a color for each prefix */
        public override void Load()
        {
            Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("Crystals/Assets/Foresta/Shaders/Misc/CursedFlames", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["CursedFlamesPass"] = new MiscShaderData(screenRef, "CursedFlamesPass");
        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (item.prefix == ModContent.PrefixType<Cursed>() || item.prefix == ModContent.PrefixType<Bane>() || item.prefix == ModContent.PrefixType<Blasphemed>() || item.prefix == ModContent.PrefixType<Devastating>() || item.prefix == ModContent.PrefixType<Fulminated>() || item.prefix == ModContent.PrefixType<Hexed>() || item.prefix == ModContent.PrefixType<Maledicted>() || item.prefix == ModContent.PrefixType<Obscenited>() || item.prefix == ModContent.PrefixType<Overwhelming>() || item.prefix == ModContent.PrefixType<Sacrileged>() || item.prefix == ModContent.PrefixType<Whammy>())
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                var Pulse = GameShaders.Misc["CursedFlamesPass"];
                Pulse.UseColor(Color.DarkGreen);
                Pulse.UseImage2(ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Shaders/Misc/PerlinNoise"));
                Pulse.Apply(null);
            }
            return true;
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        
        }


    }
}
