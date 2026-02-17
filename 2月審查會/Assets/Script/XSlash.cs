using UnityEngine;

public class XSlash : MonoBehaviour
{
    private Transform player;
    private int damage = 5;

    public void Initialize(Transform target, int dmg)
    {
        player = target;
        damage = dmg;
    }

    void Update()
    {
        if (player == null) return;

        // 簡單範圍偵測，距離小於 1.5 扣血
        if (Vector2.Distance(player.position, transform.position) < 1.5f)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(damage);
                Destroy(gameObject); // 攻擊一次後消失
            }
        }
    }
}
