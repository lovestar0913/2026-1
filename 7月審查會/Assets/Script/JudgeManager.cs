using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    [Header("判定時間(秒)")]
    public float perfectWindow = 0.05f;
    public float greatWindow = 0.10f;
    public float goodWindow = 0.15f;

    void Update()
    {
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

    void Judge(Lane lane)
    {
        Note[] notes = FindObjectsOfType<Note>();

        Note targetNote = null;
        float smallestError = Mathf.Infinity;

        foreach (Note note in notes)
        {
            if (note.judged)
                continue;

            if (note.lane != lane)
                continue;

            float error =
                Mathf.Abs(GameManager.Instance.MusicTime - note.hitTime);

            if (error < smallestError)
            {
                smallestError = error;
                targetNote = note;
            }
        }

        if (targetNote == null)
            return;

        if (smallestError <= perfectWindow)
        {
            ScoreManager.Instance.Perfect();
        }
        else if (smallestError <= greatWindow)
        {
            ScoreManager.Instance.Great();
        }
        else if (smallestError <= goodWindow)
        {
            ScoreManager.Instance.Good();
        }
        else
        {
            return;
        }

        targetNote.judged = true;
        Destroy(targetNote.gameObject);
    }
}