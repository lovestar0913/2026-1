using System.Collections.Generic;
using UnityEngine;


public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance;


    private ChartData chartData;


    [Header("Active Notes")]
    public List<Note> activeNotes = new();

    public List<HoldNote> activeHolds = new();



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



    private int spawnIndex = 0;



    public bool AllNotesFinished
    {
        get
        {
            if (chartData == null)
                return false;


            return spawnIndex >= chartData.notes.Count &&
                   activeNotes.Count == 0 &&
                   activeHolds.Count == 0;
        }
    }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    private void Start()
    {
        ResetSpawner();
    }



    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }



    //=========================
    // Reset
    //=========================

    public void ResetSpawner()
    {
        Debug.Log("NoteSpawner Reset");


        // 清除音符
        foreach (Note note in activeNotes)
        {
            if (note != null)
                Destroy(note.gameObject);
        }


        activeNotes.Clear();



        foreach (HoldNote hold in activeHolds)
        {
            if (hold != null)
                Destroy(hold.gameObject);
        }


        activeHolds.Clear();



        // 重置 index
        spawnIndex = 0;



        // 重新讀 Chart
        if (ChartLoader.Instance == null)
        {
            Debug.LogError("沒有 ChartLoader");
            return;
        }


        ChartLoader.Instance.ReloadChart();



        chartData =
            ChartLoader.Instance.GetChart();



        if (chartData == null)
        {
            Debug.LogError("ChartData Null");
            return;
        }



        Debug.Log(
            "Chart Ready Notes:"
            +
            chartData.notes.Count
        );
    }



    private void Update()
    {
        if (SongManager.Instance == null)
            return;


        if (!SongManager.Instance.Started)
            return;


        if (chartData == null)
            return;



        SpawnNotes();
    }





    //=========================
    // Spawn
    //=========================

    void SpawnNotes()
    {
        float now =
            SongManager.Instance.MusicTime;



        while (spawnIndex < chartData.notes.Count)
        {

            NoteData data =
                chartData.notes[spawnIndex];



            if (now < data.hitTime - approachTime)
                break;



            Debug.Log(
                "Spawn Note Index:"
                +
                spawnIndex
                +
                " HitTime:"
                +
                data.hitTime
                +
                " Now:"
                +
                now
            );



            if (data.noteType == NoteType.Tap)
            {
                CreateTap(data);
            }
            else
            {
                CreateHold(data);
            }



            spawnIndex++;
        }
    }





    //=========================
    // Tap
    //=========================

    void CreateTap(NoteData data)
    {

        GameObject obj =
            Instantiate(
                notePrefab,
                centerSpawn.position,
                Quaternion.identity
            );


        Note note =
            obj.GetComponent<Note>();


        if (note == null)
        {
            Debug.LogError(
                "Prefab沒有 Note"
            );

            Destroy(obj);
            return;
        }



        note.lane = data.lane;

        note.hitTime = data.hitTime;

        note.noteType = data.noteType;

        note.judgeShape = data.judgeShape;



        activeNotes.Add(note);



        Debug.Log(
            "Create Note : "
            +
            note.name
            +
            " Hit:"
            +
            note.hitTime
        );



        NoteMovement move =
            obj.GetComponent<NoteMovement>();


        if (move != null)
        {
            move.startPos =
                centerSpawn.position;


            move.targetPos =
                GetJudgePosition(data.lane);


            move.approachTime =
                approachTime;


            move.Initialize();
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
            case Lane.Q: return qJudge.position;
            case Lane.W: return wJudge.position;
            case Lane.E: return eJudge.position;
            case Lane.D: return dJudge.position;
            case Lane.S: return sJudge.position;
            case Lane.A: return aJudge.position;
        }


        return centerSpawn.position;
    }

//=========================
// Hold
//=========================

void CreateHold(NoteData data)
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
                dir
            );



        GameObject obj =
            Instantiate(
                holdPrefab,
                spawnPos,
                rot
            );



        HoldNote hold =
            obj.GetComponent<HoldNote>();



        if (hold == null)
        {
            Debug.LogError(
                "HoldPrefab 缺少 HoldNote"
            );

            Destroy(obj);
            return;
        }



        activeHolds.Add(hold);



        hold.data = data;
        hold.judgeTrack = judgeCircle;
        hold.judgeAngle = data.endAngle;



        HoldRenderer renderer =
            obj.GetComponent<HoldRenderer>();



        if (renderer != null)
        {
            float length =
                Mathf.Max(
                    0.5f,
                    (data.endTime - data.hitTime) * 2f
                );



            renderer.Generate(length);



            renderer.SetColor(
                data.color == HoldColor.Red
                ? Color.red
                : Color.cyan
            );
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
                rotateCenter
            );
        }
    }
}