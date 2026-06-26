using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject notePrefab;

    [Header("Spawn Point")]
    public Transform centerSpawn;

    [Header("Judge Points")]
    public Transform qJudge;
    public Transform wJudge;
    public Transform eJudge;
    public Transform dJudge;
    public Transform sJudge;
    public Transform aJudge;

    [Header("Chart")]
    public ChartData chartData;

    [Header("提前幾秒生成")]
    public float approachTime = 2f;

    void Update()
    {
        float currentTime = GameManager.Instance.MusicTime;

        foreach (NoteData data in chartData.notes)
        {
            // 已生成就跳過
            if (data.spawned)
                continue;

            // 到了生成時間
            if (currentTime >= data.hitTime - approachTime)
            {
                Spawn(data);
                data.spawned = true;
            }
        }
    }

    void Spawn(NoteData data)
    {
        GameObject obj = Instantiate(
            notePrefab,
            centerSpawn.position,
            Quaternion.identity);

        // 取得元件
        Note note = obj.GetComponent<Note>();
        NoteMovement movement = obj.GetComponent<NoteMovement>();

        // 設定 Note 資料
        note.lane = data.lane;
        note.hitTime = data.hitTime;

        // 起點
        movement.startPos = centerSpawn.position;

        // 終點
        switch (data.lane)
        {
            case Lane.Q:
                movement.targetPos = qJudge.position;
                break;

            case Lane.W:
                movement.targetPos = wJudge.position;
                break;

            case Lane.E:
                movement.targetPos = eJudge.position;
                break;

            case Lane.D:
                movement.targetPos = dJudge.position;
                break;

            case Lane.S:
                movement.targetPos = sJudge.position;
                break;

            case Lane.A:
                movement.targetPos = aJudge.position;
                break;
        }
    }
}