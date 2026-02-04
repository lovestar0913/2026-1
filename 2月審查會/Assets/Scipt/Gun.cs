using UnityEngine;

public class Gun : WeaponBase
{
    public override void Fire()
    {
        if (Time.time < lastFireTime + fireRate)
            return;

        lastFireTime = Time.time;

        Debug.Log("Gun Fire!");
        // 之後放子彈生成
    }
}
