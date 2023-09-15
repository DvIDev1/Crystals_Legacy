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

    public static Event CreateEvent(bool startDay, string startMessage, string endMessage, Color color, float chance,
        List<Condition> conditions, string screenShader , int musicID , List<Condition> activeCondtions)
    {
        Event e = new Event(startDay, events.Count + 1, startMessage, endMessage, color, chance, conditions , screenShader , musicID , activeCondtions);
        events.Add(e);
        return e;
    }
}