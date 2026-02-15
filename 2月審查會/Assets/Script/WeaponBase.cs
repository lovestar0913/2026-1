using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("基礎設定")]
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public float bulletSpeed = 10f;

    protected float nextFireTime;
    protected PlayerController owner;

    public PlayerController Owner => owner;

    public virtual void OnEquip(PlayerController player)
    {
        owner = player;
    }

    public void TryFire()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        Fire();
    }

    protected abstract void Fire();
}
