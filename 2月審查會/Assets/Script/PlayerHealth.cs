using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;

    private bool isDead;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        Debug.Log("玩家受傷，剩餘血量: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("玩家死亡");

        // 通知 GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();

        // 鎖定玩家控制
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.SetLock(true);

        // 停止物理
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
