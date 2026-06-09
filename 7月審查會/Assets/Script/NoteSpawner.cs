using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;

    [Header("中心出生點")]
    public Transform centerSpawn;

    [Header("六個判定點")]
    public Transform qJudge;
    public Transform wJudge;
    public Transform eJudge;
    public Transform dJudge;
    public Transform sJudge;
    public Transform aJudge;

    private void Start()
    {
        Spawn(Lane.Q, 3f);
        Spawn(Lane.W, 4f);
        Spawn(Lane.E, 5f);
        Spawn(Lane.D, 6f);
        Spawn(Lane.S, 7f);
        Spawn(Lane.A, 8f);
    }

    void Spawn(Lane lane, float hitTime)
    {
        GameObject obj =
            Instantiate(
                notePrefab,
                centerSpawn.position,
                Quaternion.identity);

        Note note = obj.GetComponent<Note>();
        NoteMovement move = obj.GetComponent<NoteMovement>();

        note.lane = lane;
        note.hitTime = hitTime;

        move.startPos = centerSpawn.position;

        switch (lane)
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
    }
}