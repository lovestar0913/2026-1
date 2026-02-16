using UnityEngine;

public class MachineGun : WeaponBase
{
    private Transform bossTransform;

    private void Update()
    {
        if (owner == null || firePoint == null) return;

        // 單次點擊射擊
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        // 自動瞄準
        AimGun();
    }

    private void AimGun()
    {
        GameObject bossObj = GameObject.FindWithTag("Boss");
        bossTransform = bossObj != null ? bossObj.transform : null;

        Vector2 aimDir = bossTransform != null ?
                         (bossTransform.position - firePoint.position).normalized :
                         owner.GetAimDirection().normalized;

        if (aimDir == Vector2.zero) return;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected override void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
            rb.gravityScale = 0f;
        }
    }
}
