using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Crystals.Core.Systems.EventSystem;

public class Event
{
    public bool StartDay = false;

    public int ID;

    public List<Condition> Conditions = new List<Condition>();

    public String name;
    

    public Color color;

    public float Chance;

    public string eventShader;

    public int MusicID;

    public List<Condition> ActiveConditions = new List<Condition>();

    public float SpawnRateMultiplier;

    public Event(bool startDay, int id, string name, Color color, float chance,
        List<Condition> conditions, string eventShader, int musicId, List<Condition> activeConditions , float spawnRateMultiplier)
    {
        StartDay = startDay;
        ID = id;
        this.name = name;
        this.color = color;
        Chance = chance;
        Conditions = conditions;
        this.eventShader = eventShader;
        MusicID = musicId;
        ActiveConditions = activeConditions;
        SpawnRateMultiplier = spawnRateMultiplier;
    }
}