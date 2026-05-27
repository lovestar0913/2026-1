using UnityEngine;

public class Note : MonoBehaviour
{
    public int targetSector;

    // 這顆Note應該被打中的時間
    public float hitTime;

    // 是否已經被判定
    public bool isJudged;
}