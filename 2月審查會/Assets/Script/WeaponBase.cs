using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("基礎設定")]
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public float bulletSpeed = 10f;

    [Header("射擊點")]
    public Transform firePoint;

    [HideInInspector] public PlayerController owner;
    protected float nextFireTime;

    // 設置擁有者
    public virtual void OnEquip(PlayerController player)
    {
        owner = player;
    }

    // 嘗試開火
    public void TryFire()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;
        Fire();
    }

    protected abstract void Fire();

    // 武器瞄準
    public virtual void AimWeapon()
    {
        if (firePoint == null || owner == null) return;

        Vector2 aimDir = owner.GetAimDirection();
        if (aimDir == Vector2.zero) return;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }
}
