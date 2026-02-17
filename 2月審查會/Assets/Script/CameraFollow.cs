using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollowClamp : MonoBehaviour
{
    public Transform target;
    public float followDelay = 0.3f;
    public Vector3 offset = new Vector3(0, 1, -10);
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 velocity;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // 每次切換場景都重新抓玩家
        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
            target = player.transform;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector3 targetPos = target.position + offset;

        float clampX = Mathf.Clamp(targetPos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampY = Mathf.Clamp(targetPos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        Vector3 clampedPos = new Vector3(clampX, clampY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followDelay);
    }
}
