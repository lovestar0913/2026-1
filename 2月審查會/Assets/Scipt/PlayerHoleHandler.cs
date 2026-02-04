using UnityEngine;
using System.Collections;

public class PlayerHoleHandler : MonoBehaviour
{
    [Header("參考")]
    public Transform graphics;          // 身體中心（SPUM Graphics）
    public Rigidbody2D rb;

    [Header("掉洞動畫")]
    public float fallDuration = 0.6f;
    public float rotateSpeed = 720f;
    public float shrinkSpeed = 3f;

    [Header("復活")]
    public float respawnOffset = 1.2f;

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
        StartCoroutine(FallCoroutine(holePos));
    }

    IEnumerator FallCoroutine(Vector3 holePos)
    {
        isFalling = true;

        // 🔒 鎖玩家
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // TODO：這裡可以扣血
        // GetComponent<PlayerHealth>().TakeDamage(5);

        float t = 0f;

        while (t < fallDuration)
        {
            // 吸向洞中心
            transform.position = Vector3.Lerp(
                transform.position,
                holePos,
                Time.deltaTime * 8f
            );

            // ⭐ 以身體中心旋轉
            graphics.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            // 縮小
            graphics.localScale = Vector3.Lerp(
                graphics.localScale,
                Vector3.zero,
                Time.deltaTime * shrinkSpeed
            );

            t += Time.deltaTime;
            yield return null;
        }

        // ====== 傳送復活 ======
        Vector3 respawnPos = FindSafeRespawnPos(holePos);
        transform.position = respawnPos;

        // 重置外觀
        graphics.localScale = originalScale;
        graphics.localRotation = Quaternion.identity;

        rb.simulated = true;
        isFalling = false;
    }

    Vector3 FindSafeRespawnPos(Vector3 holePos)
    {
        // 簡單版：往上復活（保證不是洞）
        return holePos + Vector3.up * respawnOffset;
    }

    public bool IsFalling()
    {
        return isFalling;
    }
}
