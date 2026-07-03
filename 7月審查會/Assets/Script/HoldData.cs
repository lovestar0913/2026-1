using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HoldData
{
    public float startTime;
    public float endTime;

    public HoldColor color;

    public Lane startLane;

    public List<HoldRotatePoint> rotatePoints = new();

    [HideInInspector]
    public bool spawned = false;
}