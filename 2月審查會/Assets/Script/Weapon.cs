using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("武器設定")]
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public float bulletSpeed = 15f;

    [Header("子彈生成點")]
    public Transform firePoint;

    [Header("散射設定")]
    public float spreadAngle = 0f;      // 總散射角度範圍
    public int bulletCount = 1;         // 發射幾顆
    public bool useRandomSpread = true; // true=隨機散射 / false=平均散射

    private float nextFireTime;
    private Transform owner;

    public void Initialize(Transform player)
    {
        owner = player;
    }

    void Update()
    {
        if (owner == null) return;

        AimToMouse();

        if (Input.GetButton("Fire1"))
        {
            TryFire();
        }
    }

    void AimToMouse()
    {
        if (Camera.main == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z);

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector2 dir = (Vector2)(mouseWorld - transform.position);
        if (dir.sqrMagnitude < 0.001f) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void TryFire()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        if (bulletPrefab == null || firePoint == null) return;

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector2 baseDir = ((Vector2)mouseWorld - (Vector2)firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < bulletCount; i++)
        {
            float finalAngle = baseAngle;

            if (spreadAngle > 0f)
            {
                if (useRandomSpread)
                {
                    float randomOffset = Random.Range(-spreadAngle, spreadAngle);
                    finalAngle += randomOffset;
                }
                else
                {
                    if (bulletCount > 1)
                    {
                        float step = spreadAngle * 2f / (bulletCount - 1);
                        float offset = -spreadAngle + step * i;
                        finalAngle += offset;
                    }
                }
            }

            Quaternion rot = Quaternion.Euler(0, 0, finalAngle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rot);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 finalDir = new Vector2(
                    Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                    Mathf.Sin(finalAngle * Mathf.Deg2Rad)
                );

                rb.linearVelocity = finalDir * bulletSpeed; // 修正 linearVelocity -> velocity
            }
        }
    }
}
