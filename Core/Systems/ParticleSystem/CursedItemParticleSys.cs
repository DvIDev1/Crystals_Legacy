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
        public override void Load()
        {

            Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("Crystals/Assets/Foresta/Shaders/Misc/compiler/CursedFlames", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["CursedFlamesPass"] = new MiscShaderData(screenRef, "CursedFlamesPass");

        }
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (item.prefix == ModContent.PrefixType<Cursed>() || item.prefix == ModContent.PrefixType<Bane>() || item.prefix == ModContent.PrefixType<Blasphemed>() || item.prefix == ModContent.PrefixType<Devastating>() || item.prefix == ModContent.PrefixType<Fulminated>() || item.prefix == ModContent.PrefixType<Hexed>() || item.prefix == ModContent.PrefixType<Maledicted>() || item.prefix == ModContent.PrefixType<Obscenited>() || item.prefix == ModContent.PrefixType<Overwhelming>() || item.prefix == ModContent.PrefixType<Sacrileged>() || item.prefix == ModContent.PrefixType<Whammy>())
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
                var Pulse = GameShaders.Misc["CursedFlamesPass"];
                Pulse.UseImage2(ModContent.Request<Texture2D>("Crystals/Assets/Foresta/Shaders/Misc/PerlinNoise"));
                Pulse.Apply(null);
            }
            return true;
        }
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
        
        }


    }
}
