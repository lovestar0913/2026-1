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

    [Header("旋轉")]
    public float rotateAngle = 360f;

    [Header("參考")]
    public Transform visualRoot; // ⭐ 拖 VisualRoot 進來
    public Transform graphics;   // ⭐ 拖 Graphics（給翻面用）

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isDodging;
    private float lastDodgeTime;
    private Vector3 graphicsOriginalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        graphicsOriginalScale = graphics.localScale;
    }

    void Update()
    {
        HandleInput();
        HandleFlip();
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // 輸入
    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        // 右鍵閃避
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 dodgeDir = moveInput == Vector2.zero
                ? Vector2.right * Mathf.Sign(graphics.localScale.x)
                : moveInput;

            StartDodge(dodgeDir);
        }
    }

    // 翻面
    void HandleFlip()
    {
        if (isDodging) return;

        if (moveInput.x > 0)
            graphics.localScale = new Vector3(
                -Mathf.Abs(graphicsOriginalScale.x),
                graphicsOriginalScale.y,
                graphicsOriginalScale.z
            );
        else if (moveInput.x < 0)
            graphics.localScale = new Vector3(
                Mathf.Abs(graphicsOriginalScale.x),
                graphicsOriginalScale.y,
                graphicsOriginalScale.z
            );
    }

    // 閃避
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
        while (t < dodgeTime)
        {
            // 位移
            rb.linearVelocity = dir * dodgeSpeed;

            // ⭐ 只旋轉 VisualRoot（Animator 碰不到）
            float delta = (rotateAngle / dodgeTime) * Time.deltaTime;
            visualRoot.Rotate(0f, 0f, delta);

            t += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        visualRoot.localRotation = Quaternion.identity;
        isDodging = false;
    }
}
