using System;
using System.Collections.Generic;
using System.Linq;
using Crystals.Helpers;
using Terraria;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.EventSystem;

public class EventManager : ModSystem
{
    public override void PreUpdateInvasions()
    {
        if (EventRegister.events.Count != 0)
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
        }
        

    }

    public override void OnModLoad()
    {
        EventRegister.CreateEvent(TimeValues.NightBegin, TimeValues.NightEnd, Priority.None,
            new List<Condition>() { Condition.TimeDay });
    }
}