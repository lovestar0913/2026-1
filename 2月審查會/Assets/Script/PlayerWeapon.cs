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
    // 給 PlayerController 呼叫
    // =========================
    public void TryFire()
    {
        CurrentWeapon?.TryFire();
    }

    // =========================
    // 滾輪切換（給 PlayerController 呼叫）
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
    // WeaponPickup 呼叫
    // =========================
    public void AddWeapon(WeaponBase newWeapon)
    {
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
    }

    void Equip(WeaponBase weapon, int slot)
    {
        weapons[slot] = weapon;

        weapon.OnEquip();
        weapon.gameObject.SetActive(true);

        weapon.transform.SetParent(slot == 0 ? handPoint : backPoint);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        if (slot == 0)
            currentIndex = 0;
    }

    void SetWeaponTransform(WeaponBase weapon, Transform parent)
    {
        weapon.transform.SetParent(parent);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }
}
