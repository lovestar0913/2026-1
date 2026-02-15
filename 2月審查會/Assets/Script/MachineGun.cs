using UnityEngine;

public class MachineGun : WeaponBase
{
    [Header("開火位置")]
    public Transform firePoint; // 子彈生成位置

    private float fireCooldown = 0f; // 射擊冷卻計時

    private void Awake()
    {
        // 修改父類 fireRate
        fireRate = 3f; // 每秒 3 發
    }

    private void Update()
    {
        if (owner == null) return;

        fireCooldown -= Time.deltaTime;

        // 點擊射擊
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        // 按住射擊
        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate; // 使用父類 fireRate
        }
    }

    protected override void Fire()
    {
        if (bulletPrefab == null || owner == null) return;

        Vector2 dir = owner.GetAimDirection().normalized;

        // 如果 firePoint 有設置，從 firePoint 發射，否則從武器中心
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPos,
            Quaternion.identity
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = dir * bulletSpeed; // ← 正確寫法
            rb.gravityScale = 0f;
        }

        // 子彈旋轉方向
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
