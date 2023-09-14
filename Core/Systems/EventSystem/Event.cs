using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Crystals.Core.Systems.EventSystem;

public class Event
{
    public bool StartDay = false;

    protected int ID;

    public List<Condition> Conditions = new List<Condition>();

    public String StartMessage;

    public String EndMessage;

    public Color color;

    public float Chance;

    public string eventShader;

    public int MusicID;

    public List<Condition> ActiveConditions = new List<Condition>();

    public Event(bool startDay, int id, string startMessage, string endMessage, Color color, float chance,
        List<Condition> conditions, string eventShader, int musicId, List<Condition> activeConditions)
    {
        StartDay = startDay;
        ID = id;
        StartMessage = startMessage;
        EndMessage = endMessage;
        this.color = color;
        Chance = chance;
        Conditions = conditions;
        this.eventShader = eventShader;
        MusicID = musicId;
        ActiveConditions = activeConditions;
    }
}