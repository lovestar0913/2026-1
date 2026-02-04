using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 5f;

    [Header("閃避")]
    public float dodgeSpeed = 12f;
    public float dodgeTime = 0.25f;
    public float dodgeCooldown = 0.8f;
    public float dodgeRotateAngle = 360f;

    [Header("參考")]
    public Transform visualRoot; // ⭐ 拖 VisualRoot

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool isDodging;
    private bool isLocked;
    private float lastDodgeTime;

    private Vector3 originalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = visualRoot.localScale;
    }

    void Update()
    {
        if (isLocked) return;

        HandleInput();
        HandleFlip();
    }

    void FixedUpdate()
    {
        if (isLocked || isDodging) return;

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // =====================
    // 輸入
    // =====================
    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 dir = moveInput == Vector2.zero
                ? new Vector2(Mathf.Sign(visualRoot.localScale.x), 0)
                : moveInput;

            StartDodge(dir);
        }
    }

    // =====================
    // 翻面（只翻 Scale）
    // =====================
    void HandleFlip()
    {
        if (moveInput.x > 0)
            visualRoot.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        else if (moveInput.x < 0)
            visualRoot.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
    }

    // =====================
    // 閃避（身體中心旋轉）
    // =====================
    void StartDodge(Vector2 dir)
    {
        if (isDodging) return;
        if (Time.time < lastDodgeTime + dodgeCooldown) return;

        StartCoroutine(DodgeCoroutine(dir.normalized));
    }

    IEnumerator DodgeCoroutine(Vector2 dir)
    {
        isDodging = true;
        lastDodgeTime = Time.time;

        float t = 0f;
        Quaternion startRot = visualRoot.localRotation;

        while (t < dodgeTime)
        {
            rb.linearVelocity = dir * dodgeSpeed;

            float delta = (dodgeRotateAngle / dodgeTime) * Time.deltaTime;
            visualRoot.localRotation *= Quaternion.Euler(0, 0, delta);

            t += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        visualRoot.localRotation = startRot;
        isDodging = false;
    }

    // =====================
    // 給掉洞用
    // =====================
    public void SetLock(bool value)
    {
        isLocked = value;
        if (value)
        {
            rb.linearVelocity = Vector2.zero;
            isDodging = false;
        }
    }
}
