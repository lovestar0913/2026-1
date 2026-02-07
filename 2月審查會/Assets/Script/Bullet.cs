using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("基本設定")]
    public float speed = 15f;
    public int damage = 1;
    public float lifeTime = 2f;

    [Header("特效")]
    public bool rotateWithDirection = true;

    private Vector2 direction;

    // =========================
    // 初始化（由武器呼叫）
    // =========================
    public void Init(Vector2 dir)
    {
        direction = dir.normalized;

        // 讓子彈面向飛行方向
        if (rotateWithDirection)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // =========================
    // 命中判定
    // =========================
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 不打自己
        if (other.CompareTag("Player"))
            return;

        // 打敵人（如果有）
        /*
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        */

        // 命中就消失
        Destroy(gameObject);
    }
}
