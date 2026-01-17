using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("最多裝備 2 把")]
    public WeaponBase[] weapons = new WeaponBase[2];

    private int currentIndex = 0;

    void Start()
    {
        EquipWeapon(0);
    }

    void Update()
    {
        HandleScrollSwitch();
    }

    public void Fire()
    {
        if (weapons[currentIndex] != null)
            weapons[currentIndex].Fire();
    }

    void HandleScrollSwitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
            SwitchWeapon(1);
        else if (scroll < 0f)
            SwitchWeapon(-1);
    }

    void SwitchWeapon(int dir)
    {
        int nextIndex = (currentIndex + dir + weapons.Length) % weapons.Length;
        EquipWeapon(nextIndex);
    }

    void EquipWeapon(int index)
    {
        if (weapons[index] == null) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].gameObject.SetActive(i == index);
        }

        currentIndex = index;
    }
}
