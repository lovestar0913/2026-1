using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float fireCooldown = 0.2f;
    protected float lastFireTime;

    // ★★★ 新增：是否裝備中 ★★★
    protected bool isEquipped = false;

    public virtual void TryFire()
    {
        if (!isEquipped) return;

        if (Time.time < lastFireTime + fireCooldown)
            return;

        lastFireTime = Time.time;

        Vector2 dir = GetFireDirection();

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        bullet.GetComponent<Bullet>().Init(dir);
    }

    protected virtual Vector2 GetFireDirection()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");

        if (boss != null)
        {
            Vector2 dir = boss.transform.position - firePoint.position;
            return dir.normalized;
        }

        return GetPlayerFacingDirection();
    }

    protected Vector2 GetPlayerFacingDirection()
    {
        Transform visualRoot = transform.root.Find("VisualRoot");

        if (visualRoot == null)
            return Vector2.right;

        return visualRoot.localScale.x < 0
            ? Vector2.right
            : Vector2.left;
    }

    // =====================
    // 裝備 / 卸下
    // =====================
    public virtual void OnEquip()
    {
        isEquipped = true;
        gameObject.SetActive(true);
    }

    public virtual void OnUnequip()
    {
        isEquipped = false;
        gameObject.SetActive(false);
    }
}
