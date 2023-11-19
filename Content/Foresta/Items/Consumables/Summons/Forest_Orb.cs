using System.Linq;
using Crystals.Core;
using Crystals.Core.Systems;
using Crystals.Core.Systems.CameraShake;
using Crystals.Core.Systems.EventSystem;
using Crystals.Core.Systems.TitleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Content.Foresta.Items.Consumables.Summons;

public class Forest_Orb : ModItem
{
    public override string Texture => AssetDirectory.Consumables + Name;

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Forest Orb");
        Tooltip.SetDefault("Throw this at night in a forest for a Nice Surprise");
    }

    public override void SetDefaults()
    {
        Item.Size = new Vector2(40, 34);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = Item.useAnimation = 15;
        Item.shootSpeed = 10f;
        Item.shoot = ModContent.ProjectileType<Forest_Orb_Projectile>();
        Item.rare = ItemRarityID.Green;
    }

    public override bool CanUseItem(Player player)
    {
        return player.ownedProjectileCounts[ModContent.ProjectileType<Forest_Orb_Projectile>()] == 0;
    }

    class Forest_Orb_Projectile : ModProjectile
    {
        public override string Texture => AssetDirectory.Consumables + Name;

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(32, 32);
            Projectile.tileCollide = true;
            Projectile.friendly = true;
        }

        public bool StartSummmon = false;

        public bool showStar = false;

        public override void AI()
        {
            if (!StartSummmon)
            {
                Projectile.velocity.Y = Projectile.velocity.Y + 0.15f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
            else
            {
                foreach (var player in Main.player)
                {
                    player.ZoneGraveyard = true;
                }

                Projectile.ai[0]++;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Projectile.ai[0] == 250)
                {
                    SoundEngine.PlaySound(SoundID.ScaryScream with { MaxInstances = 1 }, Projectile.Center);
                    Shake.time = 60 * 5;
                    Shake.power = 2;
                    Shake.active = true;
                    showStar = true;
                }

                if (Projectile.ai[0] <= 250)
                {
                    Projectile.velocity = new Vector2(0, -1.5f);

                    for (int i = 0; i < Projectile.ai[0] / 60f; i++)
                    {
                        Vector2 pos = Projectile.Center +
                                      Vector2.One.RotatedBy((MathHelper.TwoPi / 4 * i) + Projectile.ai[0] / 4f) *
                                      (Projectile.width + Projectile.height);
                        Dust.NewDustPerfect(pos, 61);
                    }
                }
                else
                {
                    Projectile.velocity = new Vector2(0, 0f);
                }

                if (Projectile.ai[0] >= 550)
                {
                    SoundEngine.PlaySound(SoundID.Roar, Projectile.position);
                    if (EventManager.CurrentEvent == null)
                    {
                        Event Spiritual = EventRegister.events.Find(@event => @event.name == "Spiritual Night");
                        if (Spiritual.Conditions.All(b => b.IsMet()))
                        {
                            Title.color = Spiritual.color;
                            Title.subtext = "Has started";
                            Title.text = "The " + Spiritual.name;
                            Title.ActiveTime = 180;
                            EventManager.CurrentEvent = Spiritual;
                        }
                    }

                    Projectile.Kill();
                }


                /*SoundEngine.PlaySound(SoundID.Roar, Projectile.position);
                if (EventManager.CurrentEvent == null)
                {
                    Event Spiritual = EventRegister.events.Find(@event => @event.name == "Spiritual Night");
                    if (Spiritual.Conditions.All(b => b.IsMet()) && Main.rand.NextFloat() <= Spiritual.Chance)
                    {
                        Title.color = Spiritual.color;
                        Title.subtext = "Has started";
                        Title.text = "The " + Spiritual.name;
                        Title.ActiveTime = 180;
                        EventManager.CurrentEvent = Spiritual;
                    }
                }
                        
                Projectile.Kill();*/
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (showStar)
            {
                Projectile.light = 1f;
                /*Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Texture , BlendState.Additive , null ,null , null  ,null , Main.GameViewMatrix.ZoomMatrix);*/

                Texture2D texture = ModContent.Request<Texture2D>(AssetDirectory.FX + "star_02").Value;
                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.ForestGreen,
                    0, drawOrigin, 1f, SpriteEffects.None);

                /*Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Texture , BlendState.Additive , null ,null , null  ,null , Main.GameViewMatrix.ZoomMatrix);*/
            }

            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Main.player[Projectile.owner].ZoneForest && !Main.dayTime)
            {
                StartSummmon = true;
                return false;
            }

            return true;
        }
    }
}