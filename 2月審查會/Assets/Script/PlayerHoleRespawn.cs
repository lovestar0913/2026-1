using UnityEngine;

public class PlayerHoleRespawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHoleHandler holeHandler =
            other.GetComponent<PlayerHoleHandler>();

        if (holeHandler == null) return;

        // 通知玩家「你掉洞了」
        holeHandler.FallIntoHole(transform.position);
    }
}
