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

    [Header("Spawn Circle")]
    public Transform spawnCircle;

    [Header("Rotate Center")]
    public Transform rotateCenter;

    [Header("Judge Points")]
    public Transform qJudge;
    public Transform wJudge;
    public Transform eJudge;
    public Transform dJudge;
    public Transform sJudge;
    public Transform aJudge;

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
        Transform judge = GetJudge(data.startLane);

        if (judge == null)
            return;

        if (spawnCircle == null)
        {
            Debug.LogError("Spawn Circle 未指定");
            return;
        }

        if (rotateCenter == null)
        {
            Debug.LogError("Rotate Center 未指定");
            return;
        }

        //------------------------------------------------
        // Spawn Position
        //------------------------------------------------

        Vector3 dir =
            (judge.position - spawnCircle.position).normalized;

        float radius = HexagonData.Instance.spawnRadius;

        Vector3 spawnPos =
            spawnCircle.position + dir * radius;

        //------------------------------------------------
        // 初始旋轉方向
        //------------------------------------------------

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle += 90f;

        Quaternion rotation =
            Quaternion.Euler(0f, 0f, angle);

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

        HoldRenderer holdRenderer =
            holdObj.GetComponent<HoldRenderer>();

        if (holdRenderer != null)
        {
            float holdLength =
                Mathf.Max(
                    0.5f,
                    (data.endTime - data.hitTime) * 2f);

            holdRenderer.Generate(holdLength);

            if (data.color == HoldColor.Red)
                holdRenderer.SetColor(Color.red);
            else
                holdRenderer.SetColor(Color.yellow);
        }

        //------------------------------------------------
        // HoldMovement
        //------------------------------------------------

        HoldMovement holdMovement =
            holdObj.GetComponent<HoldMovement>();

        if (holdMovement != null)
        {
            holdMovement.approachTime = approachTime;

            holdMovement.Initialize(
                data.hitTime,
                spawnPos,
                judge,
                rotateCenter);
        }
    }

    Transform GetJudge(Lane lane)
    {
        switch (lane)
        {
            case Lane.Q:
                return qJudge;

            case Lane.W:
                return wJudge;

            case Lane.E:
                return eJudge;

            case Lane.D:
                return dJudge;

            case Lane.S:
                return sJudge;

            case Lane.A:
                return aJudge;
        }

        return null;
    }
}