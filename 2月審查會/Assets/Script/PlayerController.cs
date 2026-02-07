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

    [Header("武器")]
    public WeaponBase currentWeapon;

    [Header("參考")]
    public Transform visualRoot; // 拖角色圖像根節點

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool isDodging;
    private bool isLocked;
    private float lastDodgeTime;

    private Vector3 originalScale;

    // =====================
    // 初始化
    // =====================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = visualRoot.localScale;
    }

    // =====================
    // Update（只處理 Input）
    // =====================
    void Update()
    {
        if (isLocked) return;

        HandleMoveInput();
        HandleFireInput();
        HandleFlip();
    }

    // =====================
    // FixedUpdate（物理移動）
    // =====================
    void FixedUpdate()
    {
        if (isLocked || isDodging) return;

        rb.MovePosition(
            rb.position + moveInput * moveSpeed * Time.fixedDeltaTime
        );
    }

    // =====================
    // 輸入：移動 + 閃避
    // =====================
    void HandleMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        // 右鍵閃避
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 dir = moveInput == Vector2.zero
                ? new Vector2(Mathf.Sign(visualRoot.localScale.x), 0)
                : moveInput;

            StartDodge(dir);
        }
    }

    // =====================
    // 輸入：射擊（唯一入口）
    // =====================
    void HandleFireInput()
    {
        if (currentWeapon == null) return;
        if (isDodging) return; // 閃避中不能射

        if (Input.GetMouseButtonDown(0))
        {
            currentWeapon.TryFire();
        }
    }

    // =====================
    // 翻面（只翻 Scale）
    // =====================
    void HandleFlip()
    {
        if (moveInput.x > 0)
        {
            visualRoot.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (moveInput.x < 0)
        {
            visualRoot.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }

    // =====================
    // 閃避
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
    // 武器裝備 / 卸下
    // =====================
    public void EquipWeapon(WeaponBase weapon)
    {
        if (currentWeapon != null)
            currentWeapon.OnUnequip();

        currentWeapon = weapon;

        if (currentWeapon != null)
            currentWeapon.OnEquip();
    }

    public void UnequipWeapon()
    {
        if (currentWeapon == null) return;

        currentWeapon.OnUnequip();
        currentWeapon = null;
    }

    // =====================
    // 掉洞 / 強制鎖定
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
