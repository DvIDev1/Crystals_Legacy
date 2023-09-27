using System;
using System.Collections.Generic;
using Crystals.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.EventSystem;

public static class EventRegister
{
    public static List<Event> events = new List<Event>();

    public static Event CreateEvent(bool startDay, string name, Color color, float chance,
        List<Condition> conditions, string screenShader , int musicID , List<Condition> activeCondtions , float SpawnMultiplier)
    {
        Event e = new Event(startDay, Main.worldID, name , color, chance, conditions , screenShader , musicID , activeCondtions , SpawnMultiplier);
        events.Add(e);
        return e;
    }
}