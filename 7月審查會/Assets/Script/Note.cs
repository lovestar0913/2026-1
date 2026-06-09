using UnityEngine;

public class Note : MonoBehaviour
{
    public Lane lane;

    // 這顆音符應該到達判定點的時間
    public float hitTime;
}

public enum Lane
{
    Q,
    W,
    E,
    D,
    S,
    A
}