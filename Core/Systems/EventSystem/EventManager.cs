using System;
using System.Collections.Generic;
using System.Linq;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.EventSystem;

public class EventManager : ModSystem
{
    public override void Load()
    {
        Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>(AssetDirectory.ForestaShadersMisc + "GreenScreen", AssetRequestMode.ImmediateLoad).Value);
        Filters.Scene["GreenScreen"] = new Filter(new ScreenShaderData(screenRef, "GreenScreen"), EffectPriority.VeryHigh);
        Filters.Scene["GreenScreen"].Load();
    }

    public override void PreUpdateInvasions()
    {
        /*if (EventRegister.events.Count != 0)
        {
            foreach (var e in EventRegister.events)
            {   
                if (e.conditions.All( b => b.IsMet()))
                {
                    e.Active = true;
                }
                else
                {
                    e.Active = false;
                }
            }
        }*/
    }

    public static Event? CurrentEvent = null;

    public override void PreUpdateTime()
    {
        if (EventRegister.events.Count != 0)
        {
            foreach (var e in EventRegister.events)
            {
                if (Main.time == 0 && Main.dayTime)
                {
                    if (e.StartDay)
                    {
                        if (CurrentEvent == null)
                        {
                            if (e.Conditions.All(b => b.IsMet()) && Main.rand.NextFloat() <= e.Chance)
                            {
                                Main.NewText(e.StartMessage, e.color);
                                CurrentEvent = e;
                            }
                        }
                    }
                    else if (CurrentEvent == e && !e.StartDay)
                    {
                        Main.NewText(e.EndMessage, e.color);
                        CurrentEvent = null;
                    }
                }
                else if (Main.time == 0 && !Main.dayTime)
                {
                    if (!e.StartDay)
                    {
                        if (CurrentEvent == null)
                        {
                            if (e.Conditions.All(b => b.IsMet()) && Main.rand.NextFloat() <= e.Chance)
                            {
                                Main.NewText(e.StartMessage, e.color);
                                CurrentEvent = e;
                            }
                        }
                    }
                    else if (CurrentEvent == e && e.StartDay)
                    {
                        Main.NewText(e.EndMessage, e.color);
                        CurrentEvent = null;
                    }
                }

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                /*if (Main.time == e.TimeStart)
                {
                    
                    Main.NewText(e.StartMessage , e.color);
                    CurrentEvent = e;
                    if (Main.rand.NextFloat() < e.Chance)
                    {
                        
                    }
                }*/
            }
        }
    }

    public override void OnModLoad()
    {
        List<Condition> conditions = new List<Condition>();
        conditions.AddRange(new[] { Condition.DownedEyeOfCthulhu });
        List<Condition> activeConditions = new List<Condition>();
        activeConditions.AddRange(new []{Condition.TimeNight , ConditionHelper.InForest});
        EventRegister.CreateEvent(false, "The Spiritual Night has Started", "The Spiritual Night has Ended",
            Color.Green, 1f, conditions , "GreenScreen" , MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Opening") , activeConditions);
    }

    class EventShader : ModSceneEffect
    {
        public EventShader(string mapBackground)
        {
            MapBackground = mapBackground;
        }

        public override bool IsSceneEffectActive(Player player)
        {
            if (CurrentEvent != null)
            {
                if (CurrentEvent.ActiveConditions.All(condition => condition.IsMet()))
                {
                    return true;
                }
            }
            return false;
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            
            if (CurrentEvent != null)
            {
                if (CurrentEvent.eventShader != null)
                {
                    if (isActive)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            for (float opacity = 0; opacity < 1; opacity++)
                            {
                                Filters.Scene.Activate(CurrentEvent.eventShader).GetShader().UseOpacity(opacity);

                            }
                        }
                    }
                    else
                    {
                        Filters.Scene.Deactivate(CurrentEvent.eventShader);
                    }
                    
                }
            }
        }

        public override ModWaterStyle WaterStyle { get; }
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle { get; }
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle { get; }
        public override int Music => PickMusic();
        public override string MapBackground { get; }
        public override CaptureBiome.TileColorStyle TileColorStyle { get; }

        private int PickMusic()
        {
            if (CurrentEvent != null)
            {
                return CurrentEvent.MusicID;
            }

            return 0;
        }
        
    }

}