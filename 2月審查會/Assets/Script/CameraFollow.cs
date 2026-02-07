using UnityEngine;

public class CameraFollowClamp : MonoBehaviour
{
    public Transform target;

    [Header("跟隨")]
    public float followDelay = 0.3f;
    public Vector3 offset;

    [Header("地圖邊界")]
    public Vector2 minBounds; // 左下角
    public Vector2 maxBounds; // 右上角

    private Vector3 velocity;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!target) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 targetPos = target.position + offset;

        float clampX = Mathf.Clamp(
            targetPos.x,
            minBounds.x + camWidth,
            maxBounds.x - camWidth
        );

        float clampY = Mathf.Clamp(
            targetPos.y,
            minBounds.y + camHeight,
            maxBounds.y - camHeight
        );

        Vector3 clampedPos = new Vector3(clampX, clampY, transform.position.z);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            clampedPos,
            ref velocity,
            followDelay
        );
    }
}
