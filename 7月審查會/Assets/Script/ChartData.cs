using System;
using System.Collections.Generic;
using UnityEngine;

public class ChartData : MonoBehaviour
{
    public List<NoteData> notes = new List<NoteData>();
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