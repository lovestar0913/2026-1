using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weapon;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerWeapon pw = other.GetComponent<PlayerWeapon>();
            if (pw == null) return;

            pw.AddWeapon(weapon);

            // ®³°_«á²¾°£ Pickup
            Destroy(this);
        }
    }
}
