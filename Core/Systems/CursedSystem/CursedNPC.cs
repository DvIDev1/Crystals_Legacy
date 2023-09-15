using Crystals.Content.Foresta.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
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
            Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("Crystals/Assets/Foresta/Shaders/Misc/Outline", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["Edge"] = new MiscShaderData(screenRef, "Edge");

        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.friendly != true)
            {
                IsNPCCursed = Main.rand.NextBool(1, 10);
            }
            if (IsNPCCursed == true)
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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            var Pulse = GameShaders.Misc["Edge"];
            if (IsNPCCursed == true)
            {
                Pulse.Apply(null);
            }
            return true;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Main.spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

        }

        public override void OnKill(NPC npc)
        {

            if (IsNPCCursed == true)
            {
                npc.DropItemInstanced(npc.position, Vector2.Zero, ModContent.ItemType<CursedEnergy>(), Main.rand.Next(1, 6));
            }
        }
    }
}
