using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("玩家狀態")]
    public int maxHP = 5;
    public int currentHP;

    public List<WeaponBase> equippedWeapons = new List<WeaponBase>(); // 最多兩把武器

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentHP = maxHP;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 裝備武器
    public void EquipWeapon(WeaponBase weapon)
    {
        if (weapon == null) return;

        if (!equippedWeapons.Contains(weapon))
        {
            if (equippedWeapons.Count >= 2)
            {
                // 替換第一把武器
                equippedWeapons[0] = weapon;
            }
            else
            {
                equippedWeapons.Add(weapon);
            }
        }
    }
}
