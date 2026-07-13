using UnityEngine;

public class HoldManager : MonoBehaviour
{
    public static HoldManager Instance;

    [Header("Hold")]
    public GameObject holdPrefab;

    [Header("Chart")]
    public ChartData chartData;

    [Header("Spawn")]
    public float approachTime = 2f;

    [Header("Center")]
    public Transform rotateCenter;

    [Header("Circle")]
    public CircleTrack spawnCircle;
    public CircleTrack redCircle;
    public CircleTrack blueCircle;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SpawnHold();
    }

    void SpawnHold()
    {
        if (GameManager.Instance == null)
            return;

        if (chartData == null)
            return;

        float currentTime = GameManager.Instance.MusicTime;

        foreach (HoldData data in chartData.holds)
        {
            if (data.spawned)
                continue;

            if (currentTime >= data.hitTime - approachTime)
            {
                CreateHold(data);
                data.spawned = true;
            }
        }
    }

    void CreateHold(HoldData data)
    {
        if (rotateCenter == null)
        {
            Debug.LogError("RotateCenter 未指定");
            return;
        }

        if (spawnCircle == null)
        {
            Debug.LogError("SpawnCircle 未指定");
            return;
        }

        CircleTrack judgeCircle =
            data.color == HoldColor.Red
            ? redCircle
            : blueCircle;

        if (judgeCircle == null)
        {
            Debug.LogError("Judge Circle 未指定");
            return;
        }

        //------------------------------------------------
        // 起點、終點
        //------------------------------------------------

        Vector3 spawnPos =
            spawnCircle.GetPoint(data.startAngle);

        Vector3 judgePos =
            judgeCircle.GetPoint(data.endAngle);

        //------------------------------------------------
        // 初始朝向
        //------------------------------------------------

        Vector3 dir =
            (judgePos - spawnPos).normalized;

        Quaternion rotation =
            Quaternion.LookRotation(
                Vector3.forward,
                dir);

        //------------------------------------------------
        // 建立 Hold
        //------------------------------------------------

        GameObject holdObj =
            Instantiate(
                holdPrefab,
                spawnPos,
                rotation);

        //------------------------------------------------
        // HoldNote
        //------------------------------------------------

        HoldNote holdNote =
            holdObj.GetComponent<HoldNote>();

        if (holdNote != null)
        {
            holdNote.data = data;
            GameManager.Instance.activeHolds.Add(holdNote);
        }

        //------------------------------------------------
        // HoldRenderer
        //------------------------------------------------

        HoldRenderer renderer =
            holdObj.GetComponent<HoldRenderer>();

        if (renderer != null)
        {
            float holdLength =
                Mathf.Max(
                    0.5f,
                    (data.endTime - data.hitTime) * 2f);

            renderer.Generate(holdLength);

            renderer.SetColor(
                data.color == HoldColor.Red
                ? Color.red
                : Color.cyan);
        }

        //------------------------------------------------
        // HoldMovement
        //------------------------------------------------

        HoldMovement movement =
            holdObj.GetComponent<HoldMovement>();

        if (movement != null)
        {
            movement.approachTime = approachTime;

            movement.Initialize(
            data.hitTime,
            data.startAngle,
            data.endAngle,
            spawnCircle,
            judgeCircle,
            rotateCenter);
        }
    }
}