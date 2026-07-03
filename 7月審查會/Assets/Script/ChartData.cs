using System;
using System.Collections.Generic;
using UnityEngine;

public class ChartData : MonoBehaviour
{
    // Tap
    public List<NoteData> notes = new();

    // Hold
    public List<HoldData> holds = new();
}

[Serializable]
public class NoteData
{
    [Header("基本資料")]
    public Lane lane;

    public float hitTime;

    [Header("音符種類")]
    public NoteType noteType = NoteType.Tap;

    [Header("判定環")]
    public JudgeShape judgeShape = JudgeShape.Circle;

    [HideInInspector]
    public bool spawned = false;
}