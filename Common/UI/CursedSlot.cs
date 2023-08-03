using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Crystals.Common.Systems;
using System.Text;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI.Chat;
using System.Linq;
using System;
using Crystals.Common.Prefixes;
using Crystals.Content.Foresta.Items;
using Crystals.Common.Sfx;
using ReLogic.Utilities;

namespace Crystals.Common.UI
{
    internal class CursedSlot : UIState
    {
        private UIElement area;
        private Slot slot;

        int gold;
        int gold1;
        int curse;
        int timer = 0;
        int sound;

        Item item;

        int active = 0;
        int smallDelay = 0;


        public string Messag1e1;

        public override void OnInitialize()
        {
            gold = Main.rand.Next(0, 99);
            if (Main.rand.NextBool(7))
            {
                gold1 = 2;
                gold = 0;
            }
            else
            {
                if (Main.rand.Next(100) < 60)
                {
                    gold = Main.rand.Next(80, 99);
                    gold1 = 0;
                }
                else
                {
                    gold1 = 1;
                }
                
            }
            curse = Main.rand.Next(8, 12);

            area = new UIElement();
            area.Left.Set(851, 0f);
            area.Top.Set(350, 0f);
            area.Width.Set(0, 0f);
            area.Height.Set(0, 0f);


            slot = new Slot(ItemSlot.Context.TrashItem, 0.85f)
            {
                Left = { Pixels = 937 },
                Top = { Pixels = 500 },
                ValidItemFunc = item => item.IsAir || !item.IsAir && item.Prefix(-3)
            };

            Append(slot);
            Append(area);
        }

        public override void OnDeactivate()
        {
            if (slot.Item.IsAir)
            {
                return;
            }
            Main.LocalPlayer.QuickSpawnItem(slot.Item.GetSource_FromThis(), slot.Item);
            slot.Item.TurnToAir();


        }

        public override void Update(GameTime gameTime)
        {


            if (Main.LocalPlayer.GetModPlayer<PPlayer>().ShowCurse == false && active == 1 && !slot.Item.IsAir)
            {
                active = 0;
                Main.LocalPlayer.QuickSpawnItem(slot.Item.GetSource_FromThis(), slot.Item);
                slot.Item.TurnToAir();

                //SoundEngine.PlaySound(Audio. , player.position);
            }

            if (Main.LocalPlayer.GetModPlayer<PPlayer>().ShowCurse == true)
            {

                if (Main.LocalPlayer.GetModPlayer<PPlayer>().screaming == 255)
                {
                    slot.MaxWidth = StyleDimension.FromPixels(0);
                    slot.MaxHeight = StyleDimension.FromPixels(0);
                }
                else if (Main.LocalPlayer.GetModPlayer<PPlayer>().screaming == 1)
                {
                    slot.MaxWidth = StyleDimension.FromPixels(50);
                    slot.MaxHeight = StyleDimension.FromPixels(50);
                }

                if (timer > 0)
                {
                    timer--;
                }

                if (timer == 0)
                {
                    timer = 500;
                    sound = Main.rand.Next(1, 7);
                    if (sound == 1)
                    {
                        //SoundEngine.PlaySound(Audio.Cursed, player.position);
                    }
                    if (sound == 2)
                    {
                        //SoundEngine.PlaySound(Audio., player.position);
                    }
                    if (sound == 3)
                    {
                        //SoundEngine.PlaySound(Audio. , player.position);
                    }
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Main.hidePlayerCraftingMenu = true;

            const int SlotX = 960;
            const int SlotY = 530;

            if (slot.Item.IsAir)
            {
                active = 0;

                if (Main.LocalPlayer.GetModPlayer<PPlayer>().cursedcounter == 0)
                {
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happened = false;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happen = 0;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happens = 0;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().cursedcounter = 1;
                }


                const string Message = "[c/7D33FF:Select an item to devastate..]";

                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, Message, new Vector2(SlotX - 130, SlotY - 85), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                return;
            }
            else
            {
                active = 1;
                if (Main.LocalPlayer.GetModPlayer<PPlayer>().screaming == 0)
                {
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happened = true;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().retract = true;
                }

            }

            bool hoveringSlot = Main.mouseX > 960 - 22 && Main.mouseX < 960 + 22 && Main.mouseY > 530 - 26 && Main.mouseY < 530 + 22 && !PlayerInput.IgnoreMouseInterface;
            

            if (Main.mouseLeft && hoveringSlot == true && !slot.Item.IsAir && !Main.mouseItem.IsAir)
            {
                Main.LocalPlayer.QuickSpawnItem(slot.Item.GetSource_FromThis(), slot.Item);
            }
            

            int price = Item.buyPrice(0, gold1, gold, 0);
            

            const string Messag1e = "[c/5E0BEE:The Curse demands]";

            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, Messag1e, new Vector2(SlotX - 85, SlotY - 115), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);

            if (gold1 == 0)
            {
                Messag1e1 = $"[c/D6D6D6:{gold} silver], [c/5833FF:{curse} Cursed Energy]";
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, Messag1e1, new Vector2(SlotX - 110, SlotY - 85), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else if (gold == 0)
            {
                Messag1e1 = $"[c/FAD21E:{gold1} gold], [c/5833FF:{curse} Cursed Energy]";
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, Messag1e1, new Vector2(SlotX - 110, SlotY - 85), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }
            else
            {
                Messag1e1 = $"[c/FAD21E:{gold1} gold], [c/D6D6D6:{gold} silver], [c/5833FF:{curse} Cursed Energy]";
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, Messag1e1, new Vector2(SlotX - 142, SlotY - 85), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
            }



            ItemSlot.DrawSavings(Main.spriteBatch, SlotX + 130, Main.instance.invBottom, true);

            int refoX = SlotX + 5;
            int refoY = SlotY + 85;
            bool hovering = Main.mouseX > refoX - 15 && Main.mouseX < refoX + 15 && Main.mouseY > refoY - 15 && Main.mouseY < refoY + 15 && !PlayerInput.IgnoreMouseInterface;
            Texture2D refotexture = TextureAssets.Reforge[hovering ? 1 : 0].Value;
            Main.spriteBatch.Draw(refotexture, new Vector2(refoX, refoY), null, Color.White, 0f, refotexture.Size() / 2f, 0.8f, SpriteEffects.None, 0f);

            if (!Main.mouseReforge)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
            int pricer = Item.buyPrice(0, gold1, gold, 0);
            Main.mouseReforge = true;
            Main.player[Main.myPlayer].mouseInterface = true;
            

            if (Main.mouseLeftRelease && Main.mouseLeft && Main.player[Main.myPlayer].CanAfford(price) && ItemLoader.CanReforge(slot.Item) && hovering == true && Main.LocalPlayer.CountItem(ModContent.ItemType<CursedEnergy>(), curse) >= curse)
            {

                Main.LocalPlayer.GetModPlayer<PPlayer>().screaming = 255;
                Main.LocalPlayer.GetModPlayer<PPlayer>().happened = false;
                Main.LocalPlayer.GetModPlayer<PPlayer>().happen = 0;
                Main.LocalPlayer.GetModPlayer<PPlayer>().happens = 0;
                Main.LocalPlayer.GetModPlayer<PPlayer>().cursedcounter = 1;
                Main.player[Main.myPlayer].BuyItem(price);
                var player = Main.LocalPlayer;
                for (int i = 0; i < 58; i++)
                {
                    Item item = player.inventory[i];
                    if (item.type == ModContent.ItemType<CursedEnergy>())
                    {
                        int stackk = Math.Min(item.stack, curse);
                        item.stack -= stackk;
                        curse -= stackk;
                        if (item.stack <= 0)
                            item.TurnToAir();
                        if (curse <= 0)
                            break;
                    }
                }

                ItemLoader.PreReforge(slot.Item);
                slot.Item.ResetPrefix();
                int selection;
                if (slot.Item.DamageType == DamageClass.Melee || slot.Item.DamageType == DamageClass.Ranged || slot.Item.DamageType == DamageClass.Magic)
                {
                    selection = Main.rand.Next(1, 10);
                }
                else
                {
                    selection = Main.rand.Next(1, 9);
                }
                if (selection == 1)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Cursed>());
                }
                if (selection == 2)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Overwhelming>());
                }
                if (selection == 3)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Bane>());
                }
                if (selection == 4)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Devastating>());
                }
                if (selection == 5)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Hexed>());
                }
                if (selection == 6)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Obscenited>());
                }
                if (selection == 7)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Sacrileged>());
                }
                if (selection == 8)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Whammy>());
                }
                if (selection == 9 && slot.Item.DamageType == DamageClass.Melee)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Maledicted>());
                }
                if (selection == 9 && slot.Item.DamageType == DamageClass.Magic)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Blasphemed>());
                }
                if (selection == 9 && slot.Item.DamageType == DamageClass.Ranged)
                {
                    slot.Item.Prefix(ModContent.PrefixType<Fulminated>());
                }
                gold = Main.rand.Next(0, 99);
                if (Main.rand.NextBool(7))
                {
                    gold1 = 2;
                    gold = 0;
                }
                else
                {
                    if (Main.rand.Next(100) < 60)
                    {
                        gold = Main.rand.Next(80, 99);
                        gold1 = 0;
                    }
                    else
                    {
                        gold1 = 1;
                    }

                }
                curse = Main.rand.Next(8, 12);
                bool favor = slot.Item.favorited;
                int stack = slot.Item.stack;

                slot.Item.position.X = Main.LocalPlayer.position.X + (float)(Main.LocalPlayer.width / 2) - (float)(slot.Item.width / 2);
                slot.Item.position.Y = Main.LocalPlayer.position.Y + (float)(Main.LocalPlayer.height / 2) - (float)(slot.Item.height / 2);
                slot.Item.favorited = favor;
                slot.Item.stack = stack;
                ItemLoader.PostReforge(slot.Item);
                PopupText.NewText(PopupTextContext.RegularItemPickup, slot.Item, slot.Item.stack, true, false);
                SoundEngine.PlaySound(Audio.Cursed, player.position);
                if (Main.LocalPlayer.GetModPlayer<PPlayer>().screaming == 1)
                {
                    Main.LocalPlayer.GetModPlayer<PPlayer>().happened = true;
                    Main.LocalPlayer.GetModPlayer<PPlayer>().retract = true;
                }
            }
        }


    }

    class CursedSlotSyst : ModSystem
    {
        public UserInterface CurseInterface;

        internal CursedSlot Curse;

        public override void Load()
        {

            if (!Main.dedServ)
            {
                Curse = new();
                CurseInterface = new();
                CurseInterface.SetState(Curse);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            CurseInterface?.Update(gameTime);
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
                        "Crystals: CursingUI",
                        delegate
                        {
                            CurseInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }
    }

}
