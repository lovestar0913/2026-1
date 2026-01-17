using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public float fireRate = 0.2f;
    protected float lastFireTime;

    public abstract void Fire();
}
