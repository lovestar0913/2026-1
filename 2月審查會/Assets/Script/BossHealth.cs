using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Boss 血量設定")]
    public int maxHealth = 100;
    private int currentHealth;

    private BossHealthBar healthBar;
    private Canvas healthBarCanvas;

    void Awake()
    {
        if (!Application.isPlaying) return;

        currentHealth = maxHealth;

        // Unity 2023+ 尋找血條
        healthBar = Object.FindFirstObjectByType<BossHealthBar>();

        if (healthBar != null)
        {
            healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
            healthBar.Initialize(maxHealth, currentHealth);
        }
    }

    void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (healthBarCanvas != null)
            healthBarCanvas.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!Application.isPlaying) return;

        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.TakeDamage(1); // 改成讓玩家扣血
        }
    }

    // ===============================
    // 受傷
    // ===============================
    public void TakeDamage(int damage)
    {
        if (!Application.isPlaying) return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        // 更新血條
        if (healthBar != null)
            healthBar.UpdateStats(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ===============================
    // 死亡
    // ===============================
    void Die()
    {
        Debug.Log("Boss 死亡");

        if (healthBarCanvas != null)
            healthBarCanvas.enabled = false;

        Destroy(gameObject);
    }
}
