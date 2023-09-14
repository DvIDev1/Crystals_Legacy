using System;
using System.Collections.Generic;
using Crystals.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.EventSystem;

public static class EventRegister
{

    public static List<Event> events = new List<Event>();

    public static Event CreateEvent(int TimeStart , int TimeEnd , Priority priority , List<Condition>  conditions)
    {
        Event e = new Event(TimeStart, TimeEnd, events.Count + 1, priority , conditions);
        events.Add(e);
        return e;
    }
    
    

}