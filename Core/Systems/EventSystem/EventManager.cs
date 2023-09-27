using System;
using System.Collections.Generic;
using System.Linq;
using Crystals.Core.Systems.TitleSystem;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Crystals.Core.Systems.EventSystem;

public class EventManager : ModSystem
{
    public override void Load()
    {
        Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>(AssetDirectory.ForestaShadersMisc + "GreenScreen", AssetRequestMode.ImmediateLoad).Value);
        Filters.Scene["GreenScreen"] = new Filter(new ScreenShaderData(screenRef, "GreenScreen"), EffectPriority.VeryHigh);
        Filters.Scene["GreenScreen"].Load();
    }

    public static Event? CurrentEvent = null;

    public override void PreUpdateTime()
    {
        if (EventRegister.events.Count != 0)
        {

            foreach (var e in EventRegister.events)
            {


                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (CurrentEvent != null && e.StartDay && !Main.dayTime)
                {
                    Title.color = e.color;
                    Title.subtext = "Has Ended";
                    Title.text = "The " + e.name;
                    Title.ActiveTime = 180;
                    CurrentEvent = null;
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                }else if (CurrentEvent != null && !e.StartDay && Main.dayTime)
                {
                    Title.color = e.color;
                    Title.subtext = "Has Ended";
                    Title.text = "The " + e.name;
                    Title.ActiveTime = 180;
                    CurrentEvent = null;
                }
                
                
                if (Main.time == 0 && Main.dayTime)
                {
                    if (e.StartDay)
                    {
                        if (CurrentEvent == null)
                        {
                            if (e.Conditions.All(b => b.IsMet()) && Main.rand.NextFloat() <= e.Chance)
                            {
                                Title.color = e.color;
                                Title.subtext = "Has started";
                                Title.text = "The " + e.name;
                                Title.ActiveTime = 180;
                                CurrentEvent = e;
                            }
                        }
                    }
                    else if (CurrentEvent == e && !e.StartDay)
                    {
                        Title.color = e.color;
                        Title.subtext = "Has Ended";
                        Title.text = "The " + e.name;
                        Title.ActiveTime = 180;
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
                                Title.color = e.color;
                                Title.subtext = "Has started";
                                Title.text = "The " + e.name;
                                Title.ActiveTime = 180;
                                CurrentEvent = e;
                            }
                        }
                    }
                    else if (CurrentEvent == e && e.StartDay)
                    {
                        Title.color = e.color;
                        Title.subtext = "Has Ended";
                        Title.text = "The " + e.name;
                        Title.ActiveTime = 180;
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

    /*public override void SaveWorldData(TagCompound tag)
    {
        if (CurrentEvent != null)
        {
            tag.Set(CurrentEvent.name.Replace(" " , "_"), CurrentEvent, true);
        }
    }

    
    
    public override void LoadWorldData(TagCompound tag)
    {
        foreach (var save in tag)
        {
            if (save.Key.StartsWith("Crystals/Events"))
            {
                CurrentEvent = (Event)save.Value;
            }
        }
    }*/

    public override void OnModLoad()
    {
        Event e;
        List<Condition> conditions = new List<Condition>();
        conditions.AddRange(new[] { Condition.DownedEyeOfCthulhu });
        List<Condition> activeConditions = new List<Condition>();
        activeConditions.AddRange(new []{Condition.TimeNight , ConditionHelper.InForest});
        EventRegister.CreateEvent(false, "Spiritual Night",
            Color.Green, 1f, conditions , "GreenScreen" , MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Event") , activeConditions ,  1.5f);
    }

    class SpawnRate : GlobalNPC 
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (CurrentEvent != null)
            {
                if (CurrentEvent.ActiveConditions.All(condition => condition.IsMet()))
                {
                    spawnRate = (int)(spawnRate / CurrentEvent.SpawnRateMultiplier);
                    maxSpawns = (int)(maxSpawns * CurrentEvent.SpawnRateMultiplier);   
                }
            }
        }
    }

    class EventShader : ModSceneEffect
    {
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
        
        public float opti = 0;

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
                            if (!Filters.Scene[CurrentEvent.eventShader].Active)
                            {
                                Filters.Scene.Activate(CurrentEvent.eventShader).GetShader().UseOpacity(opti);
                                //Main.curMusic = CurrentEvent.MusicID;
                            }
                            else
                            {
                                
                                // ReSharper disable once CompareOfFloatsByEqualityOperator
                                if (opti < 1f)
                                {
                                    opti += 0.01f;
                                }
                                
                                Filters.Scene.Activate(CurrentEvent.eventShader).GetShader().UseOpacity(opti);
                            }
                        }
                    }
                    else
                    {
                        
                        //Main.curMusic = -1;
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
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Jungle;

        public override SceneEffectPriority Priority => SceneEffectPriority.Event;

        private int PickMusic()
        {
            if (CurrentEvent != null)
            {
                return CurrentEvent.MusicID;
            }
            return -1;
        }
        
    }

}