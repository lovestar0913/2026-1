using UnityEngine;

[RequireComponent(typeof(HoldRenderer))]
[RequireComponent(typeof(HoldNote))]
public class HoldMovement : MonoBehaviour
{
    [Header("Move")]
    public float approachTime = 2f;

    private Transform rotateCenter;

    private CircleTrack spawnCircle;
    private CircleTrack judgeCircle;

    private float spawnTime;

    private float startAngle;
    private float endAngle;

    private bool initialized;

    private HoldRenderer holdRenderer;
    private HoldNote holdNote;

    private void Awake()
    {
        holdRenderer = GetComponent<HoldRenderer>();
        holdNote = GetComponent<HoldNote>();
    }

    public void Initialize(
        float hitTime,
        float startAngle,
        float endAngle,
        CircleTrack spawnCircle,
        CircleTrack judgeCircle,
        Transform center)
    {
        spawnTime = hitTime - approachTime;

        this.startAngle = startAngle;
        this.endAngle = endAngle;

        this.spawnCircle = spawnCircle;
        this.judgeCircle = judgeCircle;

        rotateCenter = center;

        //------------------------------------------------
        // 初始位置
        //------------------------------------------------

        transform.position =
            spawnCircle.GetPoint(startAngle);

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (GameManager.Instance == null)
            return;

        float currentTime =
            GameManager.Instance.MusicTime;

        //------------------------------------------------
        // 飛行進度
        //------------------------------------------------

        float moveProgress =
            Mathf.Clamp01(
                (currentTime - spawnTime) /
                approachTime);

        //------------------------------------------------
        // 角度插值
        //------------------------------------------------

        float currentAngle =
            Mathf.LerpAngle(
                startAngle,
                endAngle,
                moveProgress);

        //------------------------------------------------
        // 半徑插值
        //------------------------------------------------

        float spawnRadius =
            spawnCircle.GetRadius();

        float judgeRadius =
            judgeCircle.GetRadius();

        float currentRadius =
            Mathf.Lerp(
                spawnRadius,
                judgeRadius,
                moveProgress);

        //------------------------------------------------
        // 座標
        //------------------------------------------------

        float rad =
            currentAngle * Mathf.Deg2Rad;

        Vector3 offset =
            new Vector3(
                Mathf.Cos(rad),
                Mathf.Sin(rad),
                0f);

        transform.position =
            rotateCenter.position +
            offset * currentRadius;

        //------------------------------------------------
        // 永遠朝向圓心（同心旋轉）
        //------------------------------------------------

        Vector3 dirToCenter =
            (rotateCenter.position - transform.position).normalized;

        transform.up = dirToCenter;

        //------------------------------------------------
        // Hold縮短
        //------------------------------------------------

        if (holdNote.isHolding)
        {
            float progress =
                Mathf.InverseLerp(
                    holdNote.data.hitTime,
                    holdNote.data.endTime,
                    currentTime);

            holdRenderer.SetProgress(progress);
        }
    }
}