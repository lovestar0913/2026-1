using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // ===============================
    // 移動
    // ===============================
    [Header("移動")]
    public float moveSpeed = 7f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.left;

    // ===============================
    // 閃避
    // ===============================
    [Header("閃避")]
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldownTime = 1f;
    public float invincibleDuration = 0.5f;

    private bool isDodging = false;
    private bool isInvincible = false;
    private float dodgeCooldown = 0f;
    private float invincibleTimer = 0f;

    // ===============================
    // 血量
    // ===============================
    [Header("玩家血量")]
    public int maxHP = 10;
    private int currentHP;
    private bool isDead = false;

    // ===============================
    // 武器系統
    // ===============================
    [Header("武器系統")]
    public Transform weaponHoldPoint;
    private Weapon[] weapons = new Weapon[2];
    private int currentWeaponIndex = -1;

    // ===============================
    // 玩家模型 Graphics
    // ===============================
    [Header("玩家模型 Graphics")]
    public Transform graphics;

    // ===============================
    // UI - 受傷閃紅光
    // ===============================
    [Header("UI - 受傷閃紅光")]
    public DamageFlash damageFlash;

    // ===============================
    // 屬性
    // ===============================
    private bool isLocked = false;
    public float DodgeCooldownTime
    {
        get => dodgeCooldownTime;
        set => dodgeCooldownTime = value;
    }

    // ===============================
    // 初始化
    // ===============================
    void Awake()
    {
        // 保持單一玩家
        if (Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

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

    void OnEnable()
    {
        // 防萬一，如果 damageFlash 尚未設定，自行找 Play 場景 UI
        if (damageFlash == null)
            damageFlash = Object.FindFirstObjectByType<DamageFlash>();
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
    // 移動
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

        // 滿了就替換
        Destroy(weapons[currentWeaponIndex].gameObject);
        weapons[currentWeaponIndex] = newWeapon;
        UpdateActiveWeapon();
    }

    private void HandleWeaponSwitch()
    {
        if (Input.mouseScrollDelta.y != 0 && weapons[0] != null && weapons[1] != null)
        {
            currentWeaponIndex = currentWeaponIndex == 0 ? 1 : 0;
            UpdateActiveWeapon();
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
    // 受傷 / 死亡
    // ===============================
    public void TakeDamage(int dmg, bool ignoreInvincible = false)
    {
        if (isDead) return;
        if (isInvincible && !ignoreInvincible) return;

        currentHP -= dmg;

        // 受傷閃紅
        if (damageFlash != null)
            damageFlash.Flash();

        if (currentHP <= 0)
            Die();
    }

    public void FallIntoHole()
    {
        TakeDamage(maxHP, true);
    }

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
