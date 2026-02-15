using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (weaponPrefab == null)
        {
            Debug.LogError("weaponPrefab 是 null！");
            return;
        }

        PlayerWeapon pw = other.GetComponent<PlayerWeapon>();
        if (pw == null) return;

        WeaponBase newWeapon = Instantiate(weaponPrefab);
        pw.AddWeapon(newWeapon);

        Destroy(gameObject);
    }
}
