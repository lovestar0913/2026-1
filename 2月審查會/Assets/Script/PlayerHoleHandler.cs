using UnityEngine;
using System.Collections;

public class PlayerHoleHandler : MonoBehaviour
{
    [Header("參考")]
    public Transform graphics;
    public Rigidbody2D rb;

    [Header("掉洞動畫")]
    public float fallDuration = 0.6f;
    public float rotateSpeed = 720f;
    public float shrinkSpeed = 3f;

    [Header("復活")]
    public float respawnOffset = 1.2f;

    [Header("傷害")]
    public int holeDamage = 3;

    private bool isFalling;
    private Vector3 originalScale;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        originalScale = graphics.localScale;
    }

    public void FallIntoHole(Vector3 holePos)
    {
        if (isFalling) return;

        PlayerController health = GetComponent<PlayerController>();
        if (health != null && health.IsDead()) return; // ⭐ 死亡不執行

        StartCoroutine(FallCoroutine(holePos));
    }

    IEnumerator FallCoroutine(Vector3 holePos)
    {
        isFalling = true;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        float t = 0f;

        while (t < fallDuration)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                holePos,
                Time.deltaTime * 8f
            );

            graphics.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            graphics.localScale = Vector3.Lerp(
                graphics.localScale,
                Vector3.zero,
                Time.deltaTime * shrinkSpeed
            );

            t += Time.deltaTime;
            yield return null;
        }

        // ⭐ 扣血
        PlayerController health = GetComponent<PlayerController>();
        if (health != null)
        {
            health.TakeDamage(holeDamage);

            // 如果死亡就不復活
            if (health.IsDead())
            {
                isFalling = false;
                yield break;
            }
        }

        // ====== 傳送復活 ======
        Vector3 respawnPos = holePos + Vector3.up * respawnOffset;
        transform.position = respawnPos;

        graphics.localScale = originalScale;
        graphics.localRotation = Quaternion.identity;

        rb.simulated = true;
        isFalling = false;
    }

    public bool IsFalling()
    {
        return isFalling;
    }
}
