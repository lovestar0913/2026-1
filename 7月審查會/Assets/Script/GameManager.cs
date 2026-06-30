using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("Note")]
    public GameObject notePrefab;
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

    [Header("Spawn")]
    public float approachTime = 2f;

    [Header("Judge Window")]
    public float perfectWindow = 0.05f;
    public float greatWindow = 0.10f;
    public float goodWindow = 0.15f;

    [Header("Score")]
    public int combo = 0;
    public int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicSource.Play();
    }

    public float MusicTime
    {
        get { return musicSource.time; }
    }

    private void Update()
    {
        SpawnNotes();

        if (Input.GetKeyDown(KeyCode.Q))
            Judge(Lane.Q);

        if (Input.GetKeyDown(KeyCode.W))
            Judge(Lane.W);

        if (Input.GetKeyDown(KeyCode.E))
            Judge(Lane.E);

        if (Input.GetKeyDown(KeyCode.D))
            Judge(Lane.D);

        if (Input.GetKeyDown(KeyCode.S))
            Judge(Lane.S);

        if (Input.GetKeyDown(KeyCode.A))
            Judge(Lane.A);
    }

    void SpawnNotes()
    {
        float currentTime = MusicTime;

        foreach (NoteData data in chartData.notes)
        {
            if (data.spawned)
                continue;

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

        Note note = obj.GetComponent<Note>();
        NoteMovement movement = obj.GetComponent<NoteMovement>();

        note.lane = data.lane;
        note.hitTime = data.hitTime;

        movement.startPos = centerSpawn.position;

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

    void Judge(Lane lane)
    {
        Note[] notes = Object.FindObjectsByType<Note>(FindObjectsSortMode.None);

        Note target = null;
        float smallestError = Mathf.Infinity;

        foreach (Note note in notes)
        {
            if (note.judged)
                continue;

            if (note.lane != lane)
                continue;

            float error = Mathf.Abs(MusicTime - note.hitTime);

            if (error < smallestError)
            {
                smallestError = error;
                target = note;
            }
        }

        if (target == null)
            return;

        if (smallestError <= perfectWindow)
        {
            Perfect();
        }
        else if (smallestError <= greatWindow)
        {
            Great();
        }
        else if (smallestError <= goodWindow)
        {
            Good();
        }
        else
        {
            return;
        }

        target.judged = true;
        Destroy(target.gameObject);
    }

    void Perfect()
    {
        combo++;
        score += 1000;

        UIManager.Instance.UpdateJudge("PERFECT");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    void Great()
    {
        combo++;
        score += 700;

        UIManager.Instance.UpdateJudge("Great");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    void Good()
    {
        combo++;
        score += 300;

        UIManager.Instance.UpdateJudge("Good");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    public void Miss()
    {
        combo = 0;

        UIManager.Instance.UpdateJudge("Miss");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }
}