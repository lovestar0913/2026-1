using UnityEngine;
using System.Collections;

public class PlayerDodge : MonoBehaviour
{
    public float dodgeSpeed = 12f;
    public float dodgeTime = 0.25f;
    public float cooldown = 0.8f;

    [Header("旋轉設定")]
    public float rotateAngle = 360f;

    [Header("身體圖形")]
    public Transform graphics; // 拖 Graphics 進來

    private Rigidbody2D rb;
    private bool isDodging = false;
    private float lastDodgeTime;

    public bool IsDodging => isDodging;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Dodge(Vector2 dir)
    {
        if (isDodging) return;
        if (Time.time < lastDodgeTime + cooldown) return;

        StartCoroutine(DodgeCoroutine(dir));
    }

    IEnumerator DodgeCoroutine(Vector2 dir)
    {
        isDodging = true;
        lastDodgeTime = Time.time;

        float t = 0f;
        while (t < dodgeTime)
        {
            // 移動
            rb.linearVelocity = dir * dodgeSpeed;

            // ⭐ 以身體中心旋轉
            float deltaRotation = (rotateAngle / dodgeTime) * Time.deltaTime;
            graphics.localRotation *= Quaternion.Euler(0f, 0f, deltaRotation);

            t += Time.deltaTime;
            yield return null;
        }

        // 停止
        rb.linearVelocity = Vector2.zero;

        // 旋轉歸零（避免歪）
        graphics.localRotation = Quaternion.identity;

        isDodging = false;
    }
}
