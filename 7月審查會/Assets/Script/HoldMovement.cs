using UnityEngine;

public class HoldMovement : MonoBehaviour
{
    [Header("Move")]
    public Vector3 startPosition;
    public Vector3 targetPosition;

    [Header("生成到判定時間")]
    public float approachTime = 2f;

    private float spawnTime;
    private float hitTime;

    private bool initialized = false;

    public void Initialize(float hitTime, Vector3 startPos, Vector3 targetPos)
    {
        this.hitTime = hitTime;
        this.spawnTime = hitTime - approachTime;

        startPosition = startPos;
        targetPosition = targetPos;

        transform.position = startPosition;

        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            return;

        float currentTime = GameManager.Instance.MusicTime;

        float t = (currentTime - spawnTime) / approachTime;

        transform.position = Vector3.Lerp(
            startPosition,
            targetPosition,
            Mathf.Clamp01(t)
        );
    }
}