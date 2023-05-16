using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Crystals.Helpers
{
    public class VisualHelper
    {
        public static void Yoraiz0rEye(Player player)
        {
            int num = 0;
            num += player.bodyFrame.Y / 56;
            if (num >= Main.OffsetsPlayerHeadgear.Length)
            {
                num = 0;
            }

            Vector2 vector = Main.OffsetsPlayerHeadgear[num];
            vector *= player.Directions;
            Vector2 vector2 = new Vector2(player.width / 2, player.height / 2) + vector +
                              (player.MountedCenter - player.Center);
            player.sitting.GetSittingOffsetInfo(player, out var posOffset, out var seatAdjustment);
            vector2 += posOffset + new Vector2(0f, seatAdjustment);
            if (player.face == 19)
            {
                vector2.Y -= 5f * player.gravDir;
            }

            if (player.head == 276)
            {
                vector2.X += 2.5f * (float) player.direction;
            }

            if (player.mount.Active && player.mount.Type == 52)
            {
                vector2.X += 14f * (float) player.direction;
                vector2.Y -= 2f * player.gravDir;
            }

            float y = -11.5f * player.gravDir;
            Vector2 vector3 = new Vector2(3 * player.direction - ((player.direction == 1) ? 1 : 0), y) +
                              Vector2.UnitY * player.gfxOffY +
                              vector2;
            Vector2 vector4 = new Vector2(3 * player.shadowDirection[1] - ((player.direction == 1) ? 1 : 0), y) +
                              vector2;
            Vector2 vector5 = Vector2.Zero;
            if (player.mount.Active && player.mount.Cart)
            {
                int num2 = Math.Sign(player.velocity.X);
                if (num2 == 0)
                {
                    num2 = player.direction;
                }

                vector5 = new Vector2(MathHelper.Lerp(0f, -8f, player.fullRotation / ((float) Math.PI / 4f)),
                        MathHelper.Lerp(0f, 2f, Math.Abs(player.fullRotation / ((float) Math.PI / 4f))))
                    .RotatedBy(player.fullRotation);
                if (num2 == Math.Sign(player.fullRotation))
                {
                    vector5 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / ((float) Math.PI / 4f)));
                }
            }

            if (player.fullRotation != 0f)
            {
                vector3 = vector3.RotatedBy(player.fullRotation, player.fullRotationOrigin);
                vector4 = vector4.RotatedBy(player.fullRotation, player.fullRotationOrigin);
            }

            float num3 = 0f;
            Vector2 vector6 = player.position + vector3 + vector5;
            Vector2 vector7 = player.oldPosition + vector4 + vector5;
            vector7.Y -= num3 / 2f;
            vector6.Y -= num3 / 2f;
            float num4 = 1f;
            switch (player.yoraiz0rEye % 10)
            {
                case 1:
                    return;
                case 2:
                    num4 = 0.5f;
                    break;
                case 3:
                    num4 = 0.625f;
                    break;
                case 4:
                    num4 = 0.75f;
                    break;
                case 5:
                    num4 = 0.875f;
                    break;
                case 6:
                    num4 = 1f;
                    break;
                case 7:
                    num4 = 1.1f;
                    break;
            }

            if (player.yoraiz0rEye < 7)
            {
                DelegateMethods.v3_1 =
                    Main.hslToRgb(Main.rgbToHsl(player.eyeColor).X, 1f, 0.5f).ToVector3() * 0.5f * num4;
                if (player.velocity != Vector2.Zero)
                {
                    Utils.PlotTileLine(player.Center, player.Center + player.velocity * 2f, 4f,
                        DelegateMethods.CastLightOpen);
                }
                else
                {
                    Utils.PlotTileLine(player.Left, player.Right, 4f, DelegateMethods.CastLightOpen);
                }
            }

            int num5 = (int) Vector2.Distance(vector6, vector7) / 3 + 1;
            if (Vector2.Distance(vector6, vector7) % 3f != 0f)
            {
                num5++;
            }

            for (float num6 = 1f; num6 <= (float) num5; num6 += 1f)
            {
                Dust obj = Main.dust[Dust.NewDust(player.Center, 0, 0, 182)];
                obj.position = Vector2.Lerp(vector7, vector6, num6 / (float) num5);
                obj.noGravity = true;
                obj.velocity = Vector2.Zero;
                obj.customData = player;
                obj.scale = num4;
                obj.shader = GameShaders.Armor.GetSecondaryShader(player.cYorai, player);
            }
        }

        public static void Yoraiz0rEye(Player player, int dustID)
        {
            int num = 0;
            num += player.bodyFrame.Y / 56;
            if (num >= Main.OffsetsPlayerHeadgear.Length)
            {
                num = 0;
            }

            Vector2 vector = Main.OffsetsPlayerHeadgear[num];
            vector *= player.Directions;
            Vector2 vector2 = new Vector2(player.width / 2, player.height / 2) + vector +
                              (player.MountedCenter - player.Center);
            player.sitting.GetSittingOffsetInfo(player, out var posOffset, out var seatAdjustment);
            vector2 += posOffset + new Vector2(0f, seatAdjustment);
            if (player.face == 19)
            {
                vector2.Y -= 5f * player.gravDir;
            }

            if (player.head == 276)
            {
                vector2.X += 2.5f * (float) player.direction;
            }

            if (player.mount.Active && player.mount.Type == 52)
            {
                vector2.X += 14f * (float) player.direction;
                vector2.Y -= 2f * player.gravDir;
            }

            float y = -11.5f * player.gravDir;
            Vector2 vector3 = new Vector2(3 * player.direction - ((player.direction == 1) ? 1 : 0), y) +
                              Vector2.UnitY * player.gfxOffY +
                              vector2;
            Vector2 vector4 = new Vector2(3 * player.shadowDirection[1] - ((player.direction == 1) ? 1 : 0), y) +
                              vector2;
            Vector2 vector5 = Vector2.Zero;
            if (player.mount.Active && player.mount.Cart)
            {
                int num2 = Math.Sign(player.velocity.X);
                if (num2 == 0)
                {
                    num2 = player.direction;
                }

                vector5 = new Vector2(MathHelper.Lerp(0f, -8f, player.fullRotation / ((float) Math.PI / 4f)),
                        MathHelper.Lerp(0f, 2f, Math.Abs(player.fullRotation / ((float) Math.PI / 4f))))
                    .RotatedBy(player.fullRotation);
                if (num2 == Math.Sign(player.fullRotation))
                {
                    vector5 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / ((float) Math.PI / 4f)));
                }
            }

            if (player.fullRotation != 0f)
            {
                vector3 = vector3.RotatedBy(player.fullRotation, player.fullRotationOrigin);
                vector4 = vector4.RotatedBy(player.fullRotation, player.fullRotationOrigin);
            }

            float num3 = 0f;
            Vector2 vector6 = player.position + vector3 + vector5;
            Vector2 vector7 = player.oldPosition + vector4 + vector5;
            vector7.Y -= num3 / 2f;
            vector6.Y -= num3 / 2f;
            float num4 = 1f;
            switch (player.yoraiz0rEye % 10)
            {
                case 1:
                    return;
                case 2:
                    num4 = 0.5f;
                    break;
                case 3:
                    num4 = 0.625f;
                    break;
                case 4:
                    num4 = 0.75f;
                    break;
                case 5:
                    num4 = 0.875f;
                    break;
                case 6:
                    num4 = 1f;
                    break;
                case 7:
                    num4 = 1.1f;
                    break;
            }

            if (player.yoraiz0rEye < 7)
            {
                DelegateMethods.v3_1 =
                    Main.hslToRgb(Main.rgbToHsl(player.eyeColor).X, 1f, 0.5f).ToVector3() * 0.5f * num4;
                if (player.velocity != Vector2.Zero)
                {
                    Utils.PlotTileLine(player.Center, player.Center + player.velocity * 2f, 4f,
                        DelegateMethods.CastLightOpen);
                }
                else
                {
                    Utils.PlotTileLine(player.Left, player.Right, 4f, DelegateMethods.CastLightOpen);
                }
            }

            int num5 = (int) Vector2.Distance(vector6, vector7) / 3 + 1;
            if (Vector2.Distance(vector6, vector7) % 3f != 0f)
            {
                num5++;
            }

            for (float num6 = 1f; num6 <= (float) num5; num6 += 1f)
            {
                Dust obj = Main.dust[Dust.NewDust(player.Center, 0, 0, dustID)];
                obj.position = Vector2.Lerp(vector7, vector6, num6 / (float) num5);
                obj.noGravity = true;
                obj.velocity = Vector2.Zero;
                obj.customData = player;
                obj.scale = num4;
                obj.shader = GameShaders.Armor.GetSecondaryShader(player.cYorai, player);
            }
        }

        public static void Yoraiz0rEye(Player player, int dustID, int shaderID)
        {
            int num = 0;
            num += player.bodyFrame.Y / 56;
            if (num >= Main.OffsetsPlayerHeadgear.Length)
            {
                num = 0;
            }

            Vector2 vector = Main.OffsetsPlayerHeadgear[num];
            vector *= player.Directions;
            Vector2 vector2 = new Vector2(player.width / 2, player.height / 2) + vector +
                              (player.MountedCenter - player.Center);
            player.sitting.GetSittingOffsetInfo(player, out var posOffset, out var seatAdjustment);
            vector2 += posOffset + new Vector2(0f, seatAdjustment);
            if (player.face == 19)
            {
                vector2.Y -= 5f * player.gravDir;
            }

            if (player.head == 276)
            {
                vector2.X += 2.5f * (float) player.direction;
            }

            if (player.mount.Active && player.mount.Type == 52)
            {
                vector2.X += 14f * (float) player.direction;
                vector2.Y -= 2f * player.gravDir;
            }

            float y = -11.5f * player.gravDir;
            Vector2 vector3 = new Vector2(3 * player.direction - ((player.direction == 1) ? 1 : 0), y) +
                              Vector2.UnitY * player.gfxOffY +
                              vector2;
            Vector2 vector4 = new Vector2(3 * player.shadowDirection[1] - ((player.direction == 1) ? 1 : 0), y) +
                              vector2;
            Vector2 vector5 = Vector2.Zero;
            if (player.mount.Active && player.mount.Cart)
            {
                int num2 = Math.Sign(player.velocity.X);
                if (num2 == 0)
                {
                    num2 = player.direction;
                }

                vector5 = new Vector2(MathHelper.Lerp(0f, -8f, player.fullRotation / ((float) Math.PI / 4f)),
                        MathHelper.Lerp(0f, 2f, Math.Abs(player.fullRotation / ((float) Math.PI / 4f))))
                    .RotatedBy(player.fullRotation);
                if (num2 == Math.Sign(player.fullRotation))
                {
                    vector5 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(player.fullRotation / ((float) Math.PI / 4f)));
                }
            }

            if (player.fullRotation != 0f)
            {
                vector3 = vector3.RotatedBy(player.fullRotation, player.fullRotationOrigin);
                vector4 = vector4.RotatedBy(player.fullRotation, player.fullRotationOrigin);
            }

            float num3 = 0f;
            Vector2 vector6 = player.position + vector3 + vector5;
            Vector2 vector7 = player.oldPosition + vector4 + vector5;
            vector7.Y -= num3 / 2f;
            vector6.Y -= num3 / 2f;
            float num4 = 1f;
            switch (player.yoraiz0rEye % 10)
            {
                case 1:
                    return;
                case 2:
                    num4 = 0.5f;
                    break;
                case 3:
                    num4 = 0.625f;
                    break;
                case 4:
                    num4 = 0.75f;
                    break;
                case 5:
                    num4 = 0.875f;
                    break;
                case 6:
                    num4 = 1f;
                    break;
                case 7:
                    num4 = 1.1f;
                    break;
            }

            if (player.yoraiz0rEye < 7)
            {
                DelegateMethods.v3_1 =
                    Main.hslToRgb(Main.rgbToHsl(player.eyeColor).X, 1f, 0.5f).ToVector3() * 0.5f * num4;
                if (player.velocity != Vector2.Zero)
                {
                    Utils.PlotTileLine(player.Center, player.Center + player.velocity * 2f, 4f,
                        DelegateMethods.CastLightOpen);
                }
                else
                {
                    Utils.PlotTileLine(player.Left, player.Right, 4f, DelegateMethods.CastLightOpen);
                }
            }

            int num5 = (int) Vector2.Distance(vector6, vector7) / 3 + 1;
            if (Vector2.Distance(vector6, vector7) % 3f != 0f)
            {
                num5++;
            }

            for (float num6 = 1f; num6 <= (float) num5; num6 += 1f)
            {
                Dust obj = Main.dust[Dust.NewDust(player.Center, 0, 0, dustID)];
                obj.position = Vector2.Lerp(vector7, vector6, num6 / (float) num5);
                obj.noGravity = true;
                obj.velocity = Vector2.Zero;
                obj.customData = player;
                obj.scale = num4;
                obj.shader = GameShaders.Armor.GetSecondaryShader(shaderID, player);
            }
        }

        public static void DrawDustLine(Vector2 startPos, Vector2 endPos, int size, int type)
        {
            float num1 = Vector2.Distance(startPos, endPos);
            Vector2 v = (endPos - startPos) / num1;
            Vector2 vector2 = startPos;
            Vector2 screenPosition = Main.screenPosition;
            float rotation = v.ToRotation();
            for (float num2 = 0.0f; (double) num2 <= (double) num1; num2 += 4f)
            {
                float num3 = num2 / num1;
                Dust.NewDust(vector2, size, size, type);
                vector2 = startPos + num2 * v;
            }
        }

        public static void AABBLineVisualizer(Vector2 lineStart, Vector2 lineEnd, float lineWidth)
        {
            Texture2D blankTexture = Terraria.GameContent.TextureAssets.Extra[195].Value;
            Vector2 texScale =
                new Vector2((lineStart - lineEnd).Length(), lineWidth) * 0.00390625f; //1/256, texture is 256x256
            Main.EntitySpriteDraw(blankTexture, lineStart - Main.screenPosition, null, Color.Red * 0.5f,
                (lineEnd - lineStart).ToRotation(), new Vector2(0, 128), texScale, SpriteEffects.None);
        }

        public static void CreateGroundExplosion(Entity entity , float MAX_SPREAD, int fluff, int distFluff, int layerStart, int layerEnd,
            int layerJump)
        {
            Point point = entity.TopLeft.ToTileCoordinates();
            Point point2 = entity.BottomRight.ToTileCoordinates();
            point.X -= fluff;
            point.Y -= fluff;
            point2.X += fluff;
            point2.Y += fluff;
            float num = point.X / 2f + point2.X / 2f;
            float num2 = entity.width / 2 + distFluff;
            //float num2 = (entity.width / 2f + distFluff) / 16f;
            for (int i = layerStart; i < layerEnd; i += layerJump)
            {
                int num3 = i;
                for (int j = point.X; j <= point2.X; j++)
                {
                    for (int k = point.Y; k <= point2.Y; k++)
                    {
                        if (!WorldGen.InWorld(j, k, 10))
                        {
                            return;
                        }

                        /*if (Vector2.DistanceSquared(entity.Center, new Vector2(num2 * num2)) > num2)
                        {
                            Main.NewText(Vector2.DistanceSquared(entity.Center, new Vector2(num2 * num2)) > num2);
                            Main.NewText(2);
                            continue;
                        }*/
                        
                        if (Vector2.Distance(entity.Center, new Vector2(j * 16, k * 16)) > num2)
                        {
                            continue;
                        }


                        Tile tileSafely = Framing.GetTileSafely(j, k);
                        if (!tileSafely.HasTile || !Main.tileSolid[tileSafely.TileType] ||
                            Main.tileSolidTop[tileSafely.TileType] || Main.tileFrameImportant[tileSafely.TileType])
                        {
                            continue;
                        }

                        Tile tileSafely2 = Framing.GetTileSafely(j, k - 1);
                        if (tileSafely2.HasTile && Main.tileSolid[tileSafely2.TileType] &&
                            !Main.tileSolidTop[tileSafely2.TileType])
                        {
                            continue;
                        }

                        int num4 = WorldGen.KillTile_GetTileDustAmount(fail: true , tileSafely , j , k);
                        for (int l = 0; l < num4; l++)
                        {
                            Dust obj = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
                            obj.velocity.Y -= 3f + (float) num3 * 1.5f;
                            obj.velocity.Y *= Main.rand.NextFloat();
                            obj.scale += (float) num3 * 0.03f;
                        }

                        if (num3 >= 2)
                        {
                            for (int m = 0; m < num4 - 1; m++)
                            {
                                Dust obj2 = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
                                obj2.velocity.Y -= 1f + (float) num3;
                                obj2.velocity.Y *= Main.rand.NextFloat();
                            }
                        }

                        if (num4 > 0 && Main.rand.Next(3) != 0)
                        {
                            float num5 = (float) Math.Abs(num - j) / (MAX_SPREAD / 2f);
                            Gore gore = Gore.NewGoreDirect(entity.GetSource_Misc("Tile Hit") , entity.position, Vector2.Zero, 61 + Main.rand.Next(3),
                                1f - (float) num3 * 0.15f + num5 * 0.5f);
                            gore.velocity.Y -= 0.1f + (float) num3 * 0.5f + num5 * (float) num3 * 1f;
                            gore.velocity.Y *= Main.rand.NextFloat();
                            gore.position = new Vector2(j * 16 + 20, k * 16 + 20);
                        }
                    }
                }
            }
        }
    }
}