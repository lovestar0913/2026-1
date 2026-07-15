using UnityEngine;

public class JudgeRingManager : MonoBehaviour
{
    [Header("所有判定環")]
    public JudgeRing[] rings;

    [Header("提前幾秒顯示")]
    public float showTime = 0.5f;

    void Update()
    {
        foreach (JudgeRing ring in rings)
        {
            Note nearest = GetNearestNote(ring.lane);

            if (nearest != null)
            {
                float remain =
                    nearest.hitTime - SongManager.Instance.MusicTime;

                if (remain <= showTime)
                {
                    ring.SetShape(nearest.judgeShape);
                }
                else
                {
                    ring.Hide();
                }
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

        foreach (Note note in NoteSpawner.Instance.activeNotes)
        {
            if (note == null)
                continue;

            if (note.judged)
                continue;

            if (note.lane != lane)
                continue;

            float remain =
                note.hitTime - SongManager.Instance.MusicTime;

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