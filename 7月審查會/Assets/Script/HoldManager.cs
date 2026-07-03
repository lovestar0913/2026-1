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

    void Update()
    {
        SpawnHold();
    }

    void SpawnHold()
    {
        float currentTime = GameManager.Instance.MusicTime;

        foreach (HoldData data in chartData.holds)
        {
            if (data.spawned)
                continue;

            if (currentTime >= data.startTime - approachTime)
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

        switch (data.startLane)
        {
            case Lane.Q:
                obj.transform.position = qJudge.position;
                break;

            case Lane.W:
                obj.transform.position = wJudge.position;
                break;

            case Lane.E:
                obj.transform.position = eJudge.position;
                break;

            case Lane.D:
                obj.transform.position = dJudge.position;
                break;

            case Lane.S:
                obj.transform.position = sJudge.position;
                break;

            case Lane.A:
                obj.transform.position = aJudge.position;
                break;
        }
    }
}