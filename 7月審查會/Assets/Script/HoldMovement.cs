using UnityEngine;

public class HoldMovement : MonoBehaviour
{
    [Header("Move")]
    public float approachTime = 2f;

    private Transform target;
    private Transform rotateCenter;

    private float spawnTime;
    private bool initialized;

    //------------------------------------------------
    // Arc Move
    //------------------------------------------------

    private float radius;
    private float startAngle;
    private float endAngle;

    //------------------------------------------------

    public void Initialize(
        float hitTime,
        Vector3 startPos,
        Transform judge,
        Transform center)
    {
        spawnTime = hitTime - approachTime;

        target = judge;
        rotateCenter = center;

        //------------------------------------------------
        // 半徑
        //------------------------------------------------

        radius = Vector3.Distance(
            rotateCenter.position,
            startPos);

        //------------------------------------------------
        // 起始角度
        //------------------------------------------------

        Vector3 startDir =
            (startPos - rotateCenter.position).normalized;

        startAngle =
            Mathf.Atan2(
                startDir.y,
                startDir.x) * Mathf.Rad2Deg;

        //------------------------------------------------
        // 終點角度
        //------------------------------------------------

        Vector3 endDir =
            (judge.position - rotateCenter.position).normalized;

        endAngle =
            Mathf.Atan2(
                endDir.y,
                endDir.x) * Mathf.Rad2Deg;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (GameManager.Instance == null)
            return;

        //------------------------------------------------
        // 飛行進度
        //------------------------------------------------

        float currentTime = GameManager.Instance.MusicTime;

        float moveProgress =
            Mathf.Clamp01(
                (currentTime - spawnTime) /
                approachTime);

        //------------------------------------------------
        // 圓弧移動
        //------------------------------------------------

        float angle =
            Mathf.LerpAngle(
                startAngle,
                endAngle,
                moveProgress);

        Vector3 offset =
            new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f);

        transform.position =
            rotateCenter.position +
            offset * radius;

        //------------------------------------------------
        // 永遠朝向判定圈
        //------------------------------------------------

        Vector3 dir =
            (target.position - transform.position).normalized;

        transform.up = dir;

        //------------------------------------------------
        // 到達終點時固定位置
        //------------------------------------------------

        if (moveProgress >= 1f)
        {
            transform.position = target.position;
        }
    }
}