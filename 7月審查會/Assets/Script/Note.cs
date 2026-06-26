using UnityEngine;

public enum Lane
{
    Q,
    W,
    E,
    D,
    S,
    A
}

public class Note : MonoBehaviour
{
    public Lane lane;

    public float hitTime;

    // 是否已經判定
    public bool judged = false;
}