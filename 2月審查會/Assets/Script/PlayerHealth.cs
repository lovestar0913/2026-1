using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("玩家血量")]
    public int maxHP = 5;
    private int currentHP;

    private bool isDead = false;
    private PlayerController controller;

    void Awake()
    {
        // 嘗試在自己或子物件找到 PlayerController
        controller = GetComponent<PlayerController>();
        if (controller == null)
            controller = GetComponentInChildren<PlayerController>();

        if (controller == null)
            Debug.LogWarning("PlayerController 尚未掛載或找不到，SetLock 將無法使用");

        // 初始化血量，但不呼叫 SetLock，等 PlayerController 確定生成後再初始化
        currentHP = maxHP;
        isDead = false;
    }

    /// <summary>
    /// 初始化玩家血量與控制器狀態（生成玩家或重生時呼叫）
    /// </summary>
    public void InitHealth(int hp)
    {
        maxHP = hp;
        currentHP = maxHP;
        isDead = false;

        // 安全呼叫 SetLock
        if (controller != null)
            controller.SetLock(false);
        else
            Debug.LogWarning("PlayerController 為 null，無法解鎖玩家操作");
    }

    /// <summary>
    /// 玩家受傷
    /// </summary>
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    void Die()
    {
        isDead = true;

        // 鎖定玩家操作
        if (controller != null)
            controller.SetLock(true);

        // 玩家死亡時觸發遊戲結束
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }

    /// <summary>
    /// 判斷是否死亡
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }

    /// <summary>
    /// 取得當前血量
    /// </summary>
    public int GetCurrentHP()
    {
        return currentHP;
    }

    /// <summary>
    /// 設定 PlayerController（例如 PlayerSpawner 生成後再指定）
    /// </summary>
    public void SetController(PlayerController pc)
    {
        controller = pc;
    }
}
