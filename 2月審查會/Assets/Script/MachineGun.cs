using UnityEngine;

public class MachineGun : Gun
{
    public float holdFireInterval = 0.4f;
    public float holdThreshold = 0.2f;

    float pressStartTime;
    bool isHolding;

    void Update()
    {
        if (!isEquipped) return;

        // 按下（單發）
        if (Input.GetMouseButtonDown(0))
        {
            pressStartTime = Time.time;
            isHolding = false;

            TryFire(); // ✅ 只走唯一入口
        }

        // 持續按住（連射）
        if (Input.GetMouseButton(0))
        {
            if (!isHolding && Time.time - pressStartTime >= holdThreshold)
            {
                isHolding = true;
                lastFireTime = Time.time; // 重置冷卻
            }

            if (isHolding && Time.time >= lastFireTime + holdFireInterval)
            {
                TryFire(); // ✅ 冷卻由 WeaponBase 控制
            }
        }

        // 放開
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }
    }
}
