using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab;
    public float respawnTime = 2f;

    private Collider2D col;
    private SpriteRenderer sr;

    private bool isOnCooldown = false;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOnCooldown) return;
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        if (weaponPrefab == null)
        {
            Debug.LogWarning("weaponPrefab 未設定！");
            return;
        }

        player.AddWeapon(weaponPrefab);

        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        isOnCooldown = true;

        // 關閉碰撞
        if (col != null)
            col.enabled = false;

        // 隱藏圖片
        if (sr != null)
            sr.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // 重新開啟
        if (col != null)
            col.enabled = true;

        if (sr != null)
            sr.enabled = true;

        isOnCooldown = false;
    }
}
