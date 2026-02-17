using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("移動")]
    public float moveSpeed = 2f;

    [Header("延遲時間 (秒)")]
    public float minDelay = 0.5f;
    public float maxDelay = 1f;

    // ===============================
    // 攻擊功能暫時註解掉
    // ===============================
    /*
    [Header("攻擊冷卻")]
    public float attackCooldown = 2f;

    [Header("攻擊提示 Prefab")]
    public GameObject xSlashHintPrefab;
    public GameObject smokeHintPrefab;
    public GameObject stunWaveHintPrefab;
    public GameObject xBeamHintPrefab; // 雷射提示

    [Header("攻擊實體 Prefab")]
    public GameObject xSlashPrefab;
    public GameObject smokePrefab;
    public GameObject stunWavePrefab;
    public GameObject xBeamPrefab; // 雷射攻擊
    */

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
        // 攻擊協程暫時註解掉
        // if (enableAttack)
        //     StartCoroutine(AttackLoop());
    }

    // ===============================
    // 找玩家並延遲移動
    // ===============================
    IEnumerator WaitForPlayer()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            yield return null;
        }

        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove || player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // 左右翻轉
        if (direction.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    /*
    // ===============================
    // 攻擊循環
    // ===============================
    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);
            if (player == null) continue;

            int attackType = Random.Range(1, 5); // 1~4攻擊
            switch (attackType)
            {
                case 1:
                    yield return StartCoroutine(XSlashAttack());
                    break;
                case 2:
                    yield return StartCoroutine(SmokeAttack());
                    break;
                case 3:
                    yield return StartCoroutine(StunSlashAttack());
                    break;
                case 4:
                    yield return StartCoroutine(XBeamAttack());
                    break;
            }
        }
    }

    private IEnumerator XSlashAttack() { ... }
    private IEnumerator SmokeAttack() { ... }
    private IEnumerator StunSlashAttack() { ... }
    private IEnumerator XBeamAttack() { ... }
    private IEnumerator TemporarilySetDodgeCooldown(PlayerController pc, float tempCooldown, float duration) { ... }
    */
}
