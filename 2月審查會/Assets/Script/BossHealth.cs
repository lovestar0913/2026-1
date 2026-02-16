using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private BossHealthBar healthBar;
    private Canvas healthBarCanvas;

    void Awake()
    {
        if (!Application.isPlaying) return;

        // Unity 2023+ 使用 FindFirstObjectByType
        healthBar = Object.FindFirstObjectByType<BossHealthBar>();
        if (healthBar != null)
            healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (healthBar != null)
            healthBar.Initialize(100, 100); // maxHealth 與 health 自行設定
        if (healthBarCanvas != null)
            healthBarCanvas.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!Application.isPlaying) return;

        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            // 這裡改成暫時只是暫停或顯示死亡，避免找不到 Die()
            player.SetLock(true);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!Application.isPlaying) return;

        // 更新血量，假設有 health/maxHealth
        if (healthBar != null)
            healthBar.UpdateStats(50, 100); // 測試用
    }
}
