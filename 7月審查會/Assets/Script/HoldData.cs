using UnityEngine;

[System.Serializable]
public class HoldData
{
    public float hitTime;
    public float endTime;

    // 出生角度
    [Range(0, 360)]
    public float startAngle;

    // 判定角度
    [Range(0, 360)]
    public float endAngle;

    public HoldColor color;

    [HideInInspector]
    public bool spawned;
}