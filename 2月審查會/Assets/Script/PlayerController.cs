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
    public Transform graphics;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.left;

    private bool isLocked = false;
    private bool isDodging = false;
    private bool isInvincible = false;
    private float dodgeCooldown = 0f;
    private float invincibleTimer = 0f;

    private PlayerWeapon playerWeapon;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerWeapon = GetComponent<PlayerWeapon>();

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

        HandleInput();
        HandleCooldowns();
        HandleWeaponFire();
        HandleWeaponSwitch();

        // 每幀更新手上武器旋轉
        playerWeapon?.handWeapon?.AimWeapon();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput;
    }

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

    private void HandleWeaponFire()
    {
        if (Input.GetMouseButton(0))
            playerWeapon?.TryFireHandWeapon();
    }

    private void HandleWeaponSwitch()
    {
        if (Input.mouseScrollDelta.y < 0)
            playerWeapon?.SwitchWeapon(true);
    }

    private void HandleMovement()
    {
        if (isLocked) return;

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

    private IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        isInvincible = true;

        float dodgeTime = dodgeDuration;
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
            graphics.rotation = Quaternion.identity;

        isDodging = false;
    }

    // 玩家瞄準方向（Boss優先，沒Boss用滑鼠）
    public Vector2 GetAimDirection()
    {
        GameObject boss = GameObject.FindWithTag("Boss");
        if (boss != null)
            return (boss.transform.position - transform.position).normalized;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mouseWorld - transform.position).normalized;
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

    public Vector2 GetMoveDirection()
    {
        return moveInput;
    }

    // 🔹 新增：玩家死亡
    public void Die()
    {
        SetLock(true);

        if (graphics != null)
            graphics.gameObject.SetActive(false);

        rb.linearVelocity = Vector2.zero;

        Debug.Log("玩家死亡");
    }
}
