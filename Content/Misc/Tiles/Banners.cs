using Crystals.Content.Foresta.Items.Banners;
using Crystals.Content.Foresta.Npcs.Enemies.CursedSpirit;
using Crystals.Content.Foresta.Npcs.Enemies.EnemyPumpkin;
using Crystals.Content.Foresta.Npcs.Enemies.Forest_Spirit;
using Crystals.Content.Foresta.Npcs.Enemies.Leafling;
using Crystals.Content.Foresta.Npcs.Enemies.Nature_Slime;
using Crystals.Content.Foresta.Npcs.Enemies.Nature_Zombie;
using Crystals.Content.Foresta.Npcs.Enemies.Sunny;
using Crystals.Content.Foresta.Npcs.Enemies.Warriors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Crystals.Content.Misc.Tiles
{
    public class Banners : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] {16, 16, 16};
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.AnchorTop =
                new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom,
                    TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;
            //LocalizedText name = CreateMapEntryName();
            var name = CreateMapEntryName();
            // name.SetDefault("Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            var style = frameX / 18;
            switch (style)
            {
                case 0:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<NatureSlimeBanner>());
                    break;
                case 1:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<LeaflingBanner>());
                    break;
                case 2:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<NatureZombieBanner>());
                    break;
                case 3:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<OvergrownWarriorBanner>());
                    break;
                case 4:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<OvergrownKnightBanner>());
                    break;
                case 5:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<OvergrownArcherBanner>());
                    break;
                case 6:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<OvergrownPaladinBanner>());
                    break;
                case 7:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<EnemyPumpkinBanner>());
                    break;
                case 8:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<ForestSpiritBanner>());
                    break;
                case 9:
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<CursedSpiritBanner>());
                    break;
                case 10: 
                    Item.NewItem(new EntitySource_TileBreak(i * 16, j * 16), i * 16, j * 16, 16, 48,
                        ModContent.ItemType<SunnyBanner>());
                    break;
                default:
                    return;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                var player = Main.LocalPlayer;
                var style = Main.tile[i, j].TileFrameX / 18;
                switch (style)
                {
                    case 0:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<Nature_Slime>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 1:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<Leafling>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 2:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<Nature_Zombie>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 3:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<OvergrownWarrior>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 4:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<OvergrownKnight>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 5:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<OvergrownArcher>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 6:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<OvergrownPaladin>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 7:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<EnemyPumpkin>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 8:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<ForestSpirit>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 9:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<CursedSpirit>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    case 10:
                        Main.SceneMetrics.NPCBannerBuff[ModContent.NPCType<Sunny>()] = true;
                        Main.SceneMetrics.hasBanner = true;
                        break;
                    default:
                        return;
                }
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1) spriteEffects = SpriteEffects.FlipHorizontally;
        }
    }
}