using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        PlayerWeapon pw = player.GetComponent<PlayerWeapon>();
        if (pw == null) return;

        WeaponBase newWeapon = Instantiate(weaponPrefab);

        // 放入武器庫第一個空位
        int emptyIndex = -1;
        for (int i = 0; i < pw.allWeapons.Length; i++)
        {
            if (pw.allWeapons[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }

        if (emptyIndex == -1)
        {
            // 武器庫已滿 → 換手上武器
            emptyIndex = 0; // 手上武器位置
        }

        pw.allWeapons[emptyIndex] = newWeapon;
        pw.EquipWeapon(emptyIndex, true, player);

        Destroy(gameObject);
    }
}
