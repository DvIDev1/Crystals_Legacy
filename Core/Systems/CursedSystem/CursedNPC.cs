using Crystals.Content.Foresta.Items;
using Crystals.Content.Other.Dust;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Light;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.CursedSystem
{
    internal class CursedNPC : GlobalNPC
    {
        public bool IsNPCCursed = false;
        public override bool InstancePerEntity => true;

        public override void Load()
        {
            Ref<Effect> screenRef = new Ref<Effect>(ModContent
                .Request<Effect>("Crystals/Assets/Foresta/Shaders/Misc/Outline", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["Edge"] = new MiscShaderData(screenRef, "Edge");
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.friendly != true)
            {
                IsNPCCursed = Main.rand.NextBool(1, 10);
            }

            if (IsNPCCursed)
            {
                npc.life *= 2;
                npc.lifeMax *= 2;
                npc.defense *= 2;
                npc.damage *= 2;
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            if (IsNPCCursed)
            {
                var Pulse = GameShaders.Misc["Edge"];
                npc.color = Lighting.GetColor(screenPos.ToPoint());

                Pulse.Apply(null);
            }

            /*if (IsNPCCursed)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

                var Pulse = GameShaders.Misc["Edge"];
                
                Pulse.Apply(null);
            }*/
            return true;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (IsNPCCursed)
            {
                if (Main.rand.NextFloat() < 0.24f)
                {
                    var dust = Dust.NewDustDirect(npc.Center, npc.width, 1, ModContent.DustType<PixelDust>(), 0, -4f);
                    dust.color = Color.Purple;
                    dust.noGravity = true;
                }
                
            }
            
            Main.spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
        }

        public override void OnKill(NPC npc)
        {
            if (IsNPCCursed && npc.value != 0)
            {
                npc.DropItemInstanced(npc.position, Vector2.Zero, ModContent.ItemType<CursedEnergy>(),
                    Main.rand.Next(1, 6));
            }
        }
    }
}