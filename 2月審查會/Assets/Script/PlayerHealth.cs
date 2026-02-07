using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 10;
    public int currentHP;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        if (currentHP <= 0)
            Debug.Log("Player Dead");
    }
}
