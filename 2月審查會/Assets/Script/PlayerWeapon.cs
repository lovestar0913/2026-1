using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("武器欄位（最多2把）")]
    public WeaponBase[] weapons = new WeaponBase[2];

    [Header("手持 / 背部掛點")]
    public Transform handPoint;
    public Transform backPoint;

    public WeaponBase CurrentWeapon => weapons[currentIndex];

    private int currentIndex = 0;

    // =========================
    public void TryFire()
    {
        if (CurrentWeapon == null) return;
        CurrentWeapon.TryFire();
    }

    // =========================
    public void SwitchWeapon()
    {
        if (weapons[1] == null) return;

        int other = 1 - currentIndex;

        SetWeaponTransform(weapons[currentIndex], backPoint);
        SetWeaponTransform(weapons[other], handPoint);

        currentIndex = other;
    }

    // =========================
    public void AddWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null) return;

        if (weapons[0] == null)
        {
            Equip(newWeapon, 0);
            return;
        }

        if (weapons[1] == null)
        {
            Equip(newWeapon, 1);
            return;
        }

        Debug.Log("已經有兩把武器了");
        Destroy(newWeapon.gameObject);
    }

    void Equip(WeaponBase weapon, int slot)
    {
        weapons[slot] = weapon;

        weapon.OnEquip(GetComponent<PlayerController>());

        weapon.gameObject.SetActive(true);

        Transform parent = (slot == 0) ? handPoint : backPoint;

        weapon.transform.SetParent(parent);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        if (slot == 0)
            currentIndex = 0;
    }

    void SetWeaponTransform(WeaponBase weapon, Transform parent)
    {
        if (weapon == null) return;

        weapon.transform.SetParent(parent);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }
}
