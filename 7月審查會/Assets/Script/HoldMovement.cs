using UnityEngine;

public class HoldMovement : MonoBehaviour
{
    [Header("Move")]
    public Vector3 startPosition;

    [HideInInspector]
    public Transform target;

    [Header("生成到判定時間")]
    public float approachTime = 2f;

    private float spawnTime;
    private float hitTime;

    private bool initialized = false;

    public void Initialize(float hitTime, Vector3 startPos, Transform targetTransform)
    {
        this.hitTime = hitTime;
        this.spawnTime = hitTime - approachTime;

        startPosition = startPos;
        target = targetTransform;

        transform.position = startPosition;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (GameManager.Instance == null)
            return;

        if (target == null)
            return;

        float currentTime = GameManager.Instance.MusicTime;

        float t = Mathf.Clamp01(
            (currentTime - spawnTime) / approachTime
        );

        // 每幀取得最新判定位置
        Vector3 targetPosition = target.position;

        transform.position = Vector3.Lerp(
            startPosition,
            targetPosition,
            t
        );
    }
}