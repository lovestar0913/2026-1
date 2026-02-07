using UnityEngine;
using System.Collections;

public class BossChase : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 2f;

    [Header("延遲時間 (秒)")]
    public float minDelay = 0.5f;
    public float maxDelay = 1f;

    private Rigidbody2D rb;
    private Transform player;

    // 記住原始大小
    private Vector3 originalScale;
    private bool canMove = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(StartMoveAfterDelay());
    }

    IEnumerator StartMoveAfterDelay()
    {
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove || player == null) return;

        // 往玩家方向
        Vector2 direction = (player.position - transform.position).normalized;

        // 移動
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // 左右轉向
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(
               -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }
}
