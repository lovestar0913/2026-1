using UnityEngine;
using System.Collections;

public class PlayerHoleRespawn : MonoBehaviour
{
    [Header("血量")]
    public int maxHP = 10;
    public int currentHP;

    [Header("掉洞設定")]
    public int holeDamage = 1;
    public float respawnDelay = 0.4f;
    public float invincibleTime = 1.2f;

    private Vector3 lastSafePosition;
    private bool isRespawning;
    private Collider2D col;
    private Rigidbody2D rb;

    void Awake()
    {
        currentHP = maxHP;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        lastSafePosition = transform.position;
    }

    void Update()
    {
        // 只要站在安全地面，就更新安全點
        lastSafePosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isRespawning) return;

        if (other.CompareTag("Hole"))
        {
            StartCoroutine(FallIntoHole());
        }
    }

    IEnumerator FallIntoHole()
    {
        isRespawning = true;

        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        currentHP -= holeDamage;
        currentHP = Mathf.Max(currentHP, 0);

        yield return new WaitForSeconds(respawnDelay);

        transform.position = lastSafePosition;

        yield return new WaitForSeconds(invincibleTime);

        col.enabled = true;
        isRespawning = false;
    }
}
