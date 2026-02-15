using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 5f;

    [Header("閃避")]
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldownTime = 1f;
    public float invincibleDuration = 0.5f;

    [Header("Graphics")]
    public Transform graphics; // 玩家模型，用來旋轉

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.left; // 預設面向左

    private bool isLocked = false;
    private bool isDodging = false;
    private bool isInvincible = false;
    private float dodgeCooldown = 0f;
    private float dodgeTimer = 0f;
    private float invincibleTimer = 0f;

    private PlayerWeapon playerWeapon;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerWeapon = GetComponent<PlayerWeapon>();

        // 玩家預設面向左
        if (graphics != null)
        {
            Vector3 scale = graphics.localScale;
            scale.x = -Mathf.Abs(scale.x);
            graphics.localScale = scale;
        }

        lastMoveDir = Vector2.left;
    }

    void Update()
    {
        if (isLocked) return;

        // ===== 移動輸入 =====
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput;

        // ===== 攻擊 =====
        if (Input.GetMouseButton(0))
            playerWeapon?.TryFire();

        // ===== 滾輪切換武器 =====
        if (Input.mouseScrollDelta.y != 0)
            playerWeapon?.SwitchWeapon();

        // ===== 閃避輸入 =====
        dodgeCooldown -= Time.deltaTime;

        if (!isDodging && Input.GetKeyDown(KeyCode.Space) && dodgeCooldown <= 0f)
        {
            StartCoroutine(DodgeCoroutine());
        }

        // 無敵倒計時
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
                isInvincible = false;
        }
    }

    void FixedUpdate()
    {
        if (isLocked) return;

        if (!isDodging)
        {
            rb.linearVelocity = moveInput * moveSpeed;

            // 左右翻轉 Graphics
            if (graphics != null)
            {
                if (moveInput.x > 0)
                    graphics.localScale = new Vector3(-1, 1, 1);
                else if (moveInput.x < 0)
                    graphics.localScale = new Vector3(1, 1, 1);

                graphics.localRotation = Quaternion.identity; // 恢復旋轉
            }
        }
    }

    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        isInvincible = true;

        // 將翻滾持續時間同步為無敵時間
        float dodgeTime = invincibleDuration;
        dodgeCooldown = dodgeCooldownTime;
        invincibleTimer = invincibleDuration;

        float elapsed = 0f;
        float startRotation = graphics != null ? graphics.eulerAngles.z : 0f;
        float endRotation = startRotation + 720f;

        while (elapsed < dodgeTime)
        {
            rb.linearVelocity = lastMoveDir.normalized * dodgeSpeed;

            if (graphics != null)
            {
                float t = elapsed / dodgeTime;
                float angle = Mathf.Lerp(startRotation, endRotation, t);
                graphics.rotation = Quaternion.Euler(0, 0, angle);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (graphics != null)
            graphics.rotation = Quaternion.identity; // 翻滾結束恢復

        isDodging = false;
    }


    // =========================
    // 給 WeaponBase 使用
    // =========================
    public Vector2 GetAimDirection()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null)
            return (boss.transform.position - transform.position).normalized;

        return lastMoveDir.normalized;
    }

    public void SetLock(bool value)
    {
        isLocked = value;
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
