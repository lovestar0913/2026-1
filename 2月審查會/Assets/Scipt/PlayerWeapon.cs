using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("武器點")]
    public Transform handPoint;
    public Transform backPoint;

    [Header("最多 2 把")]
    public WeaponBase[] weapons = new WeaponBase[2];

    private int currentIndex = 0;

    void Update()
    {
        HandleScroll();
    }

    // ⭐ 給 WeaponPickup 呼叫
    public bool AddWeapon(WeaponBase weaponPrefab)
    {
        // Item1
        if (weapons[0] == null)
        {
            weapons[0] = Instantiate(weaponPrefab, handPoint);
            Equip(0);
            return true;
        }

        // Item2
        if (weapons[1] == null)
        {
            weapons[1] = Instantiate(weaponPrefab, backPoint);
            weapons[1].gameObject.SetActive(false);
            return true;
        }

        // 滿了
        return false;
    }

    void HandleScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) Switch(1);
        else if (scroll < 0) Switch(-1);
    }

    void Switch(int dir)
    {
        int next = (currentIndex + dir + 2) % 2;
        if (weapons[next] == null) return;

        Equip(next);
    }

    void Equip(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;

            bool active = (i == index);
            weapons[i].gameObject.SetActive(active);
            weapons[i].transform.SetParent(active ? handPoint : backPoint);
            weapons[i].transform.localPosition = Vector3.zero;
            weapons[i].transform.localRotation = Quaternion.identity;
        }

        currentIndex = index;
    }
}
