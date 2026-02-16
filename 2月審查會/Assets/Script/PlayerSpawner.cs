using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("玩家設定")]
    public GameObject playerPrefab;   // 玩家預製體
    public Transform spawnPoint;      // 出生點

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("PlayerPrefab 或 SpawnPoint 未設定！");
            return;
        }

        // 生成玩家
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // 初始化血量
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.maxHP = PlayerManager.Instance.maxHP;
            health.TakeDamage(0); // 初始化 currentHP
        }

        // 裝備玩家已選的武器
        PlayerController controller = player.GetComponent<PlayerController>();
        foreach (WeaponBase weaponPrefab in PlayerManager.Instance.equippedWeapons)
        {
            if (weaponPrefab != null)
            {
                WeaponBase weaponInstance = Instantiate(weaponPrefab, player.transform);
                weaponInstance.OnEquip(controller);
            }
        }
    }
}
