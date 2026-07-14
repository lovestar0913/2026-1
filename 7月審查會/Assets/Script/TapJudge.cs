using UnityEngine;

public class TapJudge : MonoBehaviour
{
    private void Update()
    {
        if (SongManager.Instance == null)
            return;

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

        CheckMiss();
    }

    // Tap 判定
    void Judge(Lane lane)
    {
        Note target = null;
        float smallestError = Mathf.Infinity;

        foreach (Note note in GameManager.Instance.activeNotes)
        {
            if (note == null)
                continue;

            if (note.judged)
                continue;

            if (note.lane != lane)
                continue;

            float error =
                Mathf.Abs(
                    SongManager.Instance.MusicTime -
                    note.hitTime);

            if (error < smallestError)
            {
                smallestError = error;
                target = note;
            }
        }

        if (target == null)
            return;

        if (smallestError <= GameManager.Instance.perfectWindow)
        {
            GameManager.Instance.Perfect();
        }
        else if (smallestError <= GameManager.Instance.greatWindow)
        {
            GameManager.Instance.Great();
        }
        else if (smallestError <= GameManager.Instance.goodWindow)
        {
            GameManager.Instance.Good();
        }
        else
        {
            return;
        }

        target.judged = true;

        GameManager.Instance.activeNotes.Remove(target);

        Destroy(target.gameObject);
    }

    // Miss
    void CheckMiss()
    {
        float now = SongManager.Instance.MusicTime;

        for (int i = GameManager.Instance.activeNotes.Count - 1; i >= 0; i--)
        {
            Note note =
                GameManager.Instance.activeNotes[i];

            if (note == null)
            {
                GameManager.Instance.activeNotes.RemoveAt(i);
                continue;
            }

            if (note.judged)
                continue;

            if (now >
                note.hitTime +
                GameManager.Instance.goodWindow)
            {
                note.judged = true;

                GameManager.Instance.Miss();

                GameManager.Instance.activeNotes.RemoveAt(i);

                Destroy(note.gameObject);
            }
        }
    }
}