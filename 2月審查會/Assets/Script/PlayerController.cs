using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // ===============================
    // 移動
    // ===============================
    [Header("移動")]
    public float moveSpeed = 7f;

    // ===============================
    // 閃避
    // ===============================
    [Header("閃避")]
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldownTime = 1f;
    public float invincibleDuration = 0.5f;

    // ===============================
    // 血量
    // ===============================
    [Header("玩家血量")]
    public int maxHP = 10;

    // ===============================
    // 武器系統
    // ===============================
    [Header("武器系統")]
    public Transform weaponHoldPoint;

    private Weapon[] weapons = new Weapon[2];
    private int currentWeaponIndex = -1;

    // ===============================
    // Graphics
    // ===============================
    [Header("玩家模型 Graphics")]
    public Transform graphics;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.left;

    private int currentHP;
    private bool isDead = false;
    private bool isLocked = false;
    private bool isDodging = false;
    private bool isInvincible = false;

    private float dodgeCooldown = 0f;
    private float invincibleTimer = 0f;

    // ===============================
    // 初始化
    // ===============================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHP = maxHP;
        isDead = false;

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
        if (isLocked || isDead) return;

        HandleInput();
        HandleCooldowns();
        HandleWeaponSwitch();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // ===============================
    // 移動控制
    // ===============================
    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput;
    }

    private void HandleMovement()
    {
        if (!isDodging)
        {
            rb.linearVelocity = moveInput * moveSpeed;

            if (graphics != null && moveInput.x != 0)
            {
                graphics.localScale = new Vector3(
                    moveInput.x > 0 ? -Mathf.Abs(graphics.localScale.x) : Mathf.Abs(graphics.localScale.x),
                    graphics.localScale.y,
                    graphics.localScale.z
                );

                graphics.localRotation = Quaternion.identity;
            }
        }
    }

    // ===============================
    // 閃避
    // ===============================
    private void HandleCooldowns()
    {
        dodgeCooldown -= Time.deltaTime;

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
                isInvincible = false;
        }

        if (!isDodging && Input.GetKeyDown(KeyCode.Space) && dodgeCooldown <= 0f)
        {
            StartCoroutine(DodgeCoroutine());
        }
    }
    public void SetActiveWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;
        currentWeaponIndex = index;
        UpdateActiveWeapon();
    }


    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        isInvincible = true;

        float elapsed = 0f;
        float startRotation = graphics != null ? graphics.eulerAngles.z : 0f;
        float endRotation = startRotation + 720f;

        dodgeCooldown = dodgeCooldownTime;
        invincibleTimer = invincibleDuration;

        while (elapsed < dodgeDuration)
        {
            rb.linearVelocity = lastMoveDir.normalized * dodgeSpeed;

            if (graphics != null)
            {
                float t = elapsed / dodgeDuration;
                float angle = Mathf.Lerp(startRotation, endRotation, t);
                graphics.rotation = Quaternion.Euler(0, 0, angle);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (graphics != null)
            graphics.rotation = Quaternion.identity;

        isDodging = false;
    }

    // ===============================
    // 武器系統
    // ===============================
    public void AddWeapon(Weapon weaponPrefab)
    {
        if (weaponHoldPoint == null || weaponPrefab == null) return;

        Weapon newWeapon = Instantiate(weaponPrefab, weaponHoldPoint);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        // 初始化 owner，讓武器能射擊
        newWeapon.Initialize(this.transform);

        // 找空位
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = newWeapon;
                currentWeaponIndex = i;
                UpdateActiveWeapon();
                return;
            }
        }

        // 滿了就替換手上武器
        Destroy(weapons[currentWeaponIndex].gameObject);
        weapons[currentWeaponIndex] = newWeapon;
        UpdateActiveWeapon();
    }




    private void HandleWeaponSwitch()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (weapons[0] != null && weapons[1] != null)
            {
                currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
                UpdateActiveWeapon();
            }
        }
    }

    private void UpdateActiveWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].gameObject.SetActive(i == currentWeaponIndex);
        }
    }

    // ===============================
    // 受傷
    // ===============================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;
        if (isInvincible) return;

        currentHP -= dmg;

        if (currentHP <= 0)
            Die();
    }

    // ===============================
    // 死亡
    // ===============================
    void Die()
    {
        isDead = true;
        SetLock(true);

        if (graphics != null)
            graphics.gameObject.SetActive(false);

        rb.linearVelocity = Vector2.zero;

        Debug.Log("玩家死亡");

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }

    // ===============================
    // 工具
    // ===============================
    public void SetLock(bool value)
    {
        isLocked = value;
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsInvincible() => isInvincible;
    public bool IsDead() => isDead;
    public int GetCurrentHP() => currentHP;
    public Vector2 GetMoveDirection() => moveInput;
}
