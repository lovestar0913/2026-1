using UnityEngine;

public class PlayerHoleRespawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHoleHandler holeHandler =
            other.GetComponent<PlayerHoleHandler>();

        if (holeHandler == null) return;

        holeHandler.FallIntoHole(transform.position);
    }
}
