using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponPrefab;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerWeapon pw = other.GetComponent<PlayerWeapon>();
            if (pw == null) return;

            bool success = pw.AddWeapon(weaponPrefab);
            if (success)
            {
                Destroy(gameObject); // 撿到就消失
            }
        }
    }
}
