using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1;
    public float speed = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        if (!Application.isPlaying) return;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
            rb.gravityScale = 0f;
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!Application.isPlaying) return;

        if (rb == null)
            transform.position += transform.right * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!Application.isPlaying) return;

        if (other.CompareTag("Boss"))
        {
            Destroy(gameObject);
            // 可在這邊呼叫 BossHealth.TakeDamage(damage)
        }
    }
}
