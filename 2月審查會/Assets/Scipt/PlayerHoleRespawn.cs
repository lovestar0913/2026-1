using UnityEngine;
using System.Collections;

public class PlayerHoleRespawn : MonoBehaviour
{
    [Header("Layer")]
    public LayerMask holeLayer;
    public LayerMask groundLayer;

    [Header("掉洞動畫")]
    public float fallDuration = 0.6f;
    public float rotateSpeed = 720f;
    public float shrinkScale = 0.1f;

    [Header("重生")]
    public float respawnRadius = 2f;
    public int fallDamage = 5;

    [Header("參考")]
    public Transform visualRoot; // ⭐ 拖 VisualRoot

    private bool isFalling;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private Rigidbody2D rb;
    private PlayerController controller;
    private PlayerHealth health;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();

        originalScale = visualRoot.localScale;
        originalRotation = visualRoot.localRotation;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & holeLayer) != 0)
        {
            if (!isFalling)
                StartCoroutine(FallAndRespawn());
        }
    }

    IEnumerator FallAndRespawn()
    {
        isFalling = true;

        controller.SetLock(true);
        rb.linearVelocity = Vector2.zero;

        health.TakeDamage(fallDamage);

        float t = 0f;

        while (t < fallDuration)
        {
            visualRoot.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            float lerp = t / fallDuration;
            visualRoot.localScale = Vector3.Lerp(
                originalScale,
                originalScale * shrinkScale,
                lerp
            );

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = FindGroundPosition();

        visualRoot.localScale = originalScale;
        visualRoot.localRotation = originalRotation;

        controller.SetLock(false);
        isFalling = false;
    }

    Vector2 FindGroundPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector2 pos = (Vector2)transform.position
                + Random.insideUnitCircle * respawnRadius;

            if (Physics2D.OverlapCircle(pos, 0.2f, groundLayer))
                return pos;
        }
        return transform.position;
    }
}
