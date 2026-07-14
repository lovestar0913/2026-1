using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Chart")]
    public ChartData chartData;

    [Header("Prefab")]
    public GameObject notePrefab;
    public GameObject holdPrefab;

    [Header("Spawn")]
    public float approachTime = 2f;

    [Header("Tap Spawn")]
    public Transform centerSpawn;

    [Header("Tap Judge")]
    public Transform qJudge;
    public Transform wJudge;
    public Transform eJudge;
    public Transform dJudge;
    public Transform sJudge;
    public Transform aJudge;

    [Header("Hold")]
    public Transform rotateCenter;

    public CircleTrack spawnCircle;
    public CircleTrack redCircle;
    public CircleTrack blueCircle;

    private void Update()
    {
        if (SongManager.Instance == null)
            return;

        SpawnTap();

        SpawnHold();
    }

    //------------------------------------------------
    // Tap
    //------------------------------------------------

    void SpawnTap()
    {
        float now = SongManager.Instance.MusicTime;

        foreach (NoteData data in chartData.notes)
        {
            if (data.spawned)
                continue;

            if (now >= data.hitTime - approachTime)
            {
                CreateTap(data);
                data.spawned = true;
            }
        }
    }

    void CreateTap(NoteData data)
    {
        GameObject obj =
            Instantiate(
                notePrefab,
                centerSpawn.position,
                Quaternion.identity);

        Note note =
            obj.GetComponent<Note>();

        note.lane = data.lane;
        note.hitTime = data.hitTime;
        note.noteType = data.noteType;
        note.judgeShape = data.judgeShape;

        NoteMovement move =
            obj.GetComponent<NoteMovement>();

        move.startPos = centerSpawn.position;
        move.approachTime = approachTime;

        switch (data.lane)
        {
            case Lane.Q:
                move.targetPos = qJudge.position;
                break;

            case Lane.W:
                move.targetPos = wJudge.position;
                break;

            case Lane.E:
                move.targetPos = eJudge.position;
                break;

            case Lane.D:
                move.targetPos = dJudge.position;
                break;

            case Lane.S:
                move.targetPos = sJudge.position;
                break;

            case Lane.A:
                move.targetPos = aJudge.position;
                break;
        }

        GameManager.Instance.activeNotes.Add(note);

        NoteSprite sprite =
            obj.GetComponent<NoteSprite>();

        if (sprite != null)
            sprite.UpdateSprite();
    }

    //------------------------------------------------
    // Hold
    //------------------------------------------------

    void SpawnHold()
    {
        float now = SongManager.Instance.MusicTime;

        foreach (HoldData data in chartData.holds)
        {
            if (data.spawned)
                continue;

            if (now >= data.hitTime - approachTime)
            {
                CreateHold(data);
                data.spawned = true;
            }
        }
    }

    void CreateHold(HoldData data)
    {
        CircleTrack judgeCircle =
            data.color == HoldColor.Red
            ? redCircle
            : blueCircle;

        Vector3 spawnPos =
            spawnCircle.GetPoint(data.startAngle);

        Vector3 judgePos =
            judgeCircle.GetPoint(data.endAngle);

        Vector3 dir =
            (judgePos - spawnPos).normalized;

        Quaternion rot =
            Quaternion.LookRotation(
                Vector3.forward,
                dir);

        GameObject obj =
            Instantiate(
                holdPrefab,
                spawnPos,
                rot);

        //------------------------------------------------

        HoldNote hold =
            obj.GetComponent<HoldNote>();

        hold.data = data;

        hold.judgeTrack = judgeCircle;
        hold.judgeAngle = data.endAngle;

        GameManager.Instance.activeHolds.Add(hold);

        //------------------------------------------------

        HoldRenderer renderer =
            obj.GetComponent<HoldRenderer>();

        if (renderer != null)
        {
            float length =
                Mathf.Max(
                    0.5f,
                    (data.endTime - data.hitTime) * 2f);

            renderer.Generate(length);

            renderer.SetColor(
                data.color == HoldColor.Red
                ? Color.red
                : Color.cyan);
        }

        //------------------------------------------------

        HoldMovement movement =
            obj.GetComponent<HoldMovement>();

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