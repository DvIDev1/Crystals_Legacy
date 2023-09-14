using System;
using System.Collections.Generic;
using Terraria;

namespace Crystals.Core.Systems.EventSystem;

public class Event
{

    public int TimeStart;

    public int TimeEnd;

    protected int ID;

    public Priority priority = Priority.None;

    public List<Condition> conditions = new List<Condition>();

    public Event(int timeStart = default, int timeEnd = default, int id = default, Priority priority = default, List<Condition> conditions = null)
    {
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        ID = id;
        this.priority = priority;
        this.conditions = conditions;
    }

    public bool Active = false;

}