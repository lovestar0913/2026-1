using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    private int currentHP;

    private bool isDead = false;

    private PlayerController controller;

    void Start()
    {
        currentHP = maxHP;
        controller = GetComponent<PlayerController>();
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (controller != null)
            controller.SetLock(true);

        Debug.Log("Player Dead");
    }

    // ⭐ 新增這個方法
    public bool IsDead()
    {
        return isDead;
    }
}
