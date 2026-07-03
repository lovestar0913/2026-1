using UnityEngine;

public class JudgeRingManager : MonoBehaviour
{
    public JudgeRing[] rings;

    void Update()
    {
        foreach (JudgeRing ring in rings)
        {
            Note nearest = GetNearestNote(ring.lane);

            if (nearest != null)
            {
                ring.SetShape(nearest.judgeShape);
            }
            else
            {
                ring.Hide();
            }
        }
    }

    Note GetNearestNote(Lane lane)
    {
        Note nearest = null;
        float nearestTime = Mathf.Infinity;

        foreach (Note note in GameManager.Instance.activeNotes)
        {
            if (note == null)
                continue;

            if (note.judged)
                continue;

            if (note.lane != lane)
                continue;

            float remain = note.hitTime - GameManager.Instance.MusicTime;

            if (remain < 0)
                continue;

            if (remain < nearestTime)
            {
                nearestTime = remain;
                nearest = note;
            }
        }

        return nearest;
    }
}