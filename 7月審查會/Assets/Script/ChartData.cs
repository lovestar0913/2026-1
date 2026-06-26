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
    public Lane lane;

    public float hitTime;

    [HideInInspector]
    public bool spawned = false;
}