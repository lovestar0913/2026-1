using UnityEngine;

public class Gun : WeaponBase
{
    [Header("散射設定")]
    public float spreadAngle = 10f;

    // =========================
    // 只改射擊方向（加散射）
    // =========================
    protected override Vector2 GetFireDirection()
    {
        Vector2 baseDir = base.GetFireDirection();

        float randomAngle = Random.Range(-spreadAngle, spreadAngle);
        return Rotate(baseDir, randomAngle);
    }

    Vector2 Rotate(Vector2 v, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }
}
