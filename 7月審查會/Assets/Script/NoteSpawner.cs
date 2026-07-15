using System.Collections.Generic;
using UnityEngine;


public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance;


    private ChartData chartData;


    [Header("Active Notes")]
    public List<Note> activeNotes = new();

    public List<HoldNote> activeHolds = new();



    [Header("Spawned Note")]
    private HashSet<NoteData> spawnedNotes = new();



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



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    private void Start()
    {
        if (SongManager.Instance == null)
        {
            Debug.LogError("找不到 SongManager");
            return;
        }


        chartData =
            SongManager.Instance.chartData;


        if (chartData == null)
        {
            Debug.LogError("ChartData 為空");
        }
    }



    private void Update()
    {
        if (chartData == null)
            return;


        SpawnNotes();
    }



    // =========================
    // Spawn
    // =========================


    void SpawnNotes()
    {
        float now =
            SongManager.Instance.MusicTime;



        foreach (NoteData data in chartData.notes)
        {
            if (spawnedNotes.Contains(data))
                continue;



            if (now >= data.hitTime - approachTime)
            {
                if (data.noteType == NoteType.Tap)
                {
                    CreateTap(data);
                }
                else if (data.noteType == NoteType.Hold)
                {
                    CreateHold(data);
                }


                spawnedNotes.Add(data);
            }
        }
    }



    // =========================
    // Tap
    // =========================


    void CreateTap(NoteData data)
    {
        GameObject obj =
            Instantiate(
                notePrefab,
                centerSpawn.position,
                Quaternion.identity);



        Note note =
            obj.GetComponent<Note>();


        if (note == null)
        {
            Debug.LogError("NotePrefab 缺少 Note");
            Destroy(obj);
            return;
        }



        // 加入管理
        activeNotes.Add(note);



        note.lane = data.lane;
        note.hitTime = data.hitTime;
        note.noteType = data.noteType;
        note.judgeShape = data.judgeShape;



        NoteMovement move =
            obj.GetComponent<NoteMovement>();


        if (move != null)
        {
            move.startPos =
                centerSpawn.position;


            move.approachTime =
                approachTime;


            move.targetPos =
                GetJudgePosition(data.lane);
        }



        NoteSprite sprite =
            obj.GetComponent<NoteSprite>();


        if (sprite != null)
            sprite.UpdateSprite();
    }




    Vector3 GetJudgePosition(Lane lane)
    {
        switch (lane)
        {
            case Lane.Q:
                return qJudge.position;

            case Lane.W:
                return wJudge.position;

            case Lane.E:
                return eJudge.position;

            case Lane.D:
                return dJudge.position;

            case Lane.S:
                return sJudge.position;

            case Lane.A:
                return aJudge.position;
        }


        return centerSpawn.position;
    }




    // =========================
    // Hold
    // =========================


    void CreateHold(NoteData data)
    {
        CircleTrack judgeCircle =
            data.color == HoldColor.Red
            ? redCircle
            : blueCircle;



        Vector3 spawnPos =
            spawnCircle.GetPoint(
                data.startAngle);



        Vector3 judgePos =
            judgeCircle.GetPoint(
                data.endAngle);



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



        HoldNote hold =
            obj.GetComponent<HoldNote>();


        if (hold == null)
        {
            Debug.LogError("HoldPrefab 缺少 HoldNote");
            Destroy(obj);
            return;
        }



        activeHolds.Add(hold);



        hold.data = data;


        hold.judgeTrack =
            judgeCircle;


        hold.judgeAngle =
            data.endAngle;



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



        HoldMovement movement =
            obj.GetComponent<HoldMovement>();


        if (movement != null)
        {
            movement.approachTime =
                approachTime;


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