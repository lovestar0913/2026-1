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



        transform.position =
            spawnCircle.GetPoint(startAngle);


        initialized = true;
    }



    private void Update()
    {
        if (!initialized)
            return;


        if (SongManager.Instance == null)
            return;



        float currentTime =
            SongManager.Instance.MusicTime;



        //-------------------------------------
        // 移動進度
        //-------------------------------------

        float moveProgress =
            Mathf.Clamp01(
                (currentTime - spawnTime)
                /
                approachTime);



        //-------------------------------------
        // 角度
        //-------------------------------------

        float currentAngle =
            Mathf.LerpAngle(
                startAngle,
                endAngle,
                moveProgress);



        //-------------------------------------
        // 半徑
        //-------------------------------------

        float spawnRadius =
            spawnCircle.GetRadius();


        float judgeRadius =
            judgeCircle.GetRadius();


        float currentRadius =
            Mathf.Lerp(
                spawnRadius,
                judgeRadius,
                moveProgress);



        //-------------------------------------
        // 計算中心點
        //-------------------------------------

        float rad =
            currentAngle * Mathf.Deg2Rad;


        Vector3 offset =
            new Vector3(
                Mathf.Cos(rad),
                Mathf.Sin(rad),
                0f);



        Vector3 centerPosition =
            rotateCenter.position
            +
            offset * currentRadius;



        //-------------------------------------
        // 朝向圓心
        //-------------------------------------

        Vector3 dirToCenter =
            (rotateCenter.position -
            centerPosition).normalized;


        transform.up =
            dirToCenter;



        //-------------------------------------
        // 修正 Head 對準判定圈
        //-------------------------------------

        float headOffset =
            holdRenderer.headOffset;


        transform.position =
            centerPosition
            -
            transform.up * headOffset;



        //-------------------------------------
        // Hold縮短
        //-------------------------------------

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