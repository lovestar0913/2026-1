using UnityEngine;

public class Gun : WeaponBase
{
    [Header("子彈設定")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    protected override void Fire()
    {
        Debug.Log("Fire!");

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("子彈或發射點沒設定");
            return;
        }

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
