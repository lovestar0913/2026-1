using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("武器庫 (預設3把)")]
    public WeaponBase[] allWeapons = new WeaponBase[3];

    [Header("掛點")]
    public Transform handPoint;
    public Transform backPoint;

    [HideInInspector] public WeaponBase handWeapon;
    [HideInInspector] public WeaponBase backWeapon;

    // 裝備武器
    public void EquipWeapon(int weaponIndex, bool equipToHand, PlayerController player)
    {
        if (!Application.isPlaying) return; // 編輯模式不執行

        if (weaponIndex < 0 || weaponIndex >= allWeapons.Length) return;
        WeaponBase weapon = allWeapons[weaponIndex];
        if (weapon == null) return;

        if (equipToHand)
        {
            if (handWeapon != null)
            {
                backWeapon = handWeapon;
                backWeapon.transform.SetParent(backPoint);
                backWeapon.transform.localPosition = Vector3.zero;
                backWeapon.transform.localRotation = Quaternion.identity;
            }

            handWeapon = weapon;
            handWeapon.gameObject.SetActive(true);
            handWeapon.OnEquip(player);
            handWeapon.transform.SetParent(handPoint);
            handWeapon.transform.localPosition = Vector3.zero;
            handWeapon.transform.localRotation = Quaternion.identity;
        }
        else
        {
            if (backWeapon != null)
                backWeapon.gameObject.SetActive(false);

            backWeapon = weapon;
            backWeapon.gameObject.SetActive(true);
            backWeapon.OnEquip(player);
            backWeapon.transform.SetParent(backPoint);
            backWeapon.transform.localPosition = Vector3.zero;
            backWeapon.transform.localRotation = Quaternion.identity;
        }
    }

    // 手上武器開火
    public void TryFireHandWeapon()
    {
        if (!Application.isPlaying) return;
        handWeapon?.TryFire();
    }

    // 滾輪切換武器
    public void SwitchWeapon(bool scrollDown)
    {
        if (!Application.isPlaying) return;
        if (!scrollDown || backWeapon == null) return;

        WeaponBase temp = handWeapon;
        handWeapon = backWeapon;
        backWeapon = temp;

        if (handWeapon != null)
        {
            handWeapon.gameObject.SetActive(true);
            handWeapon.transform.SetParent(handPoint);
            handWeapon.transform.localPosition = Vector3.zero;
            handWeapon.transform.localRotation = Quaternion.identity;
        }

        if (backWeapon != null)
        {
            backWeapon.gameObject.SetActive(true);
            backWeapon.transform.SetParent(backPoint);
            backWeapon.transform.localPosition = Vector3.zero;
            backWeapon.transform.localRotation = Quaternion.identity;
        }
    }
}
