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

namespace Crystals.Core.Systems.ParticleSystem
{
    internal class CursedItemParticleSys : GlobalItem
    {
        /*
        public bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale,Item item)
        {
            if (Main.netMode != NetmodeID.Server && item.prefix == ModContent.PrefixType<Cursed>() || item.prefix == ModContent.PrefixType<Overwhelming>() || item.prefix == ModContent.PrefixType<Bane>() || item.prefix == ModContent.PrefixType<Devastating>() || item.prefix == ModContent.PrefixType<Hexed>())
            {

                for (int i = 0; i < 50; i++)
                {
                    Vector2 speed = Utils.RandomVector2(Main.rand, -1f, 1f);
                    Dust dust = Dust.NewDustPerfect(position, DustID.PurpleTorch, speed * 2);
                    dust.noGravity = true;
                    Main.NewText("cursed");
                }
            }
            return false;
        }
        */
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

           if (Main.netMode != NetmodeID.Server && item.prefix == ModContent.PrefixType<Cursed>() || item.prefix == ModContent.PrefixType<Overwhelming>() || item.prefix == ModContent.PrefixType<Bane>() || item.prefix == ModContent.PrefixType<Devastating>() || item.prefix == ModContent.PrefixType<Hexed>())
            {
                
                for (int i = 0; i < 50; i++)
                {
                    Vector2 speed = Utils.RandomVector2(Main.rand, -1f, 1f);
                    Dust dust = Dust.NewDustPerfect(frame.Center(), DustID.PurpleTorch, speed * 2);
                    dust.noGravity = true;
                }
            }
        }
    }
}
