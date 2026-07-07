using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HoldData
{
    public float appearTime;

    public float hitTime;

    public float endTime;

    public Lane startLane;

    public HoldColor color;

    public List<HoldRotatePoint> rotatePoints = new();

    [HideInInspector]
    public bool spawned = false;
}