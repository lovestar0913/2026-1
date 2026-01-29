using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // 玩家
    public float followDelay = 0.3f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;

        // SmoothDamp 會自動做「延遲感」
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            followDelay
        );
    }
}
