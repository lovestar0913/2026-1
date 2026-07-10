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
        //----------------------------------------------------
        // 判定點
        //----------------------------------------------------

        Transform judge = null;

        switch (data.startLane)
        {
            case Lane.Q:
                judge = qJudge;
                break;

            case Lane.W:
                judge = wJudge;
                break;

            case Lane.E:
                judge = eJudge;
                break;

            case Lane.D:
                judge = dJudge;
                break;

            case Lane.S:
                judge = sJudge;
                break;

            case Lane.A:
                judge = aJudge;
                break;
        }

        if (judge == null)
            return;

        //----------------------------------------------------
        // 外圈生成位置
        //----------------------------------------------------

        Vector3 dir =
            (judge.position - spawnCircle.position).normalized;

        float radius = HexagonData.Instance.spawnRadius;

        Vector3 spawnPos =
            spawnCircle.position + dir * radius;

        //----------------------------------------------------
        // 建立 Hold
        //----------------------------------------------------

        GameObject obj =
            Instantiate(
                holdPrefab,
                spawnPos,
                Quaternion.identity);

        HoldNote hold = obj.GetComponent<HoldNote>();

        hold.data = data;

        //----------------------------------------------------
        // 初始化移動
        //----------------------------------------------------

        HoldMovement move =
            obj.GetComponent<HoldMovement>();

        if (move != null)
        {
            move.Initialize(
            data.hitTime,
            spawnPos,
            judge);
        }
    }
}