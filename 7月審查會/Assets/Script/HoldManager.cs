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

            // hitTime 取代以前的 startTime
            if (currentTime >= data.hitTime - approachTime)
            {
                CreateHold(data);
                data.spawned = true;
            }
        }
    }

    void CreateHold(HoldData data)
    {
        GameObject obj = Instantiate(holdPrefab);

        HoldNote hold = obj.GetComponent<HoldNote>();

        hold.data = data;

        Vector3 target = Vector3.zero;

        switch (data.startLane)
        {
            case Lane.Q:
                target = qJudge.position;
                break;

            case Lane.W:
                target = wJudge.position;
                break;

            case Lane.E:
                target = eJudge.position;
                break;

            case Lane.D:
                target = dJudge.position;
                break;

            case Lane.S:
                target = sJudge.position;
                break;

            case Lane.A:
                target = aJudge.position;
                break;
        }

        // 目前先直接放到判定點
        obj.transform.position = target;

        // 下一步 HoldMovement 會改成：
        //
        // Vector3 start = target + target.normalized * 3f;
        // move.Initialize(data.hitTime, start, target);
        //
        // 到時候 Hold 就會從六角形外飛進來。
    }
}