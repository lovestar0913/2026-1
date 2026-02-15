using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("子彈設定")]
    public float lifeTime = 3f;
    public int damage = 1;
    public float speed = 10f; // 新增子彈速度

    private Rigidbody2D rb;

    void Start()
    {
        // 如果子彈有 Rigidbody2D，就讓它用速度移動
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed; // 子彈沿著 X 軸方向前進
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 如果沒有 Rigidbody2D，也可以用 transform 移動
        if (rb == null)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
