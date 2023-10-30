using System.Linq;
using Crystals.Content.Foresta.Items;
using Crystals.Content.Foresta.Npcs.Enemies.CursedSpirit;
using Crystals.Content.Foresta.Npcs.Enemies.Forest_Spirit;
using Crystals.Content.Other.Dust;
using Crystals.Core.Systems.EventSystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Light;
using Terraria.Graphics.Shaders;
using Terraria.ID;
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
            if (npc.friendly != true && npc.dontTakeDamage == false && npc.type != NPCID.TargetDummy && !NPCSets.Spirit[npc.type])
            {
                IsNPCCursed = Main.rand.NextBool(1, 20);
            }

            if (IsNPCCursed)
            {
                npc.life += npc.life / 4;
                npc.lifeMax += npc.lifeMax / 4;
                npc.defense += npc.defense / 4;
                npc.damage += npc.damage / 4;
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            
            if (IsNPCCursed)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend , SamplerState.LinearClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                
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
                
                Main.spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);
                
            }

        }

        public override void OnKill(NPC npc)
        {
            if (IsNPCCursed && npc.value != 0)
            {
                npc.DropItemInstanced(npc.position, Vector2.Zero, ModContent.ItemType<CursedEnergy>(),
                    Main.rand.Next(1, 6));
            }

            if (IsNPCCursed && Main.rand.NextBool(1, 5))
            {
                if (EventManager.CurrentEvent != null)
                {
                    if (EventManager.CurrentEvent.name == "Spiritual Night" && EventManager.CurrentEvent.ActiveConditions.All(condition => condition.IsMet()) )
                    {
                        NPC.NewNPC(new EntitySource_Death(npc), (int)npc.Center.X, (int)npc.Center.Y,
                            ModContent.NPCType<ForestSpirit>());
                        return;
                    }
                }
                
                NPC.NewNPC(new EntitySource_Death(npc), (int)npc.Center.X, (int)npc.Center.Y,
                    ModContent.NPCType<CursedSpirit>());
            }
        }
    }
}