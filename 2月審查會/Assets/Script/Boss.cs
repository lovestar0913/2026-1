using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 2f;

    [Header("延遲時間 (秒)")]
    public float minDelay = 0.5f;
    public float maxDelay = 1f;

    private Rigidbody2D rb;
    private Transform player;
    private Vector3 originalScale;
    private bool canMove = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Start()
    {
        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer()
    {
        // 等待玩家生成
        while (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            yield return null; // 每幀檢查一次
        }

        // 玩家找到後開始延遲移動
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove || player == null || rb == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // 左右轉向
        if (direction.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
}
