using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    [Header("判定時間")]
    public float perfectTime = 0.05f;
    public float greatTime = 0.1f;
    public float goodTime = 0.2f;
    public float missTime = 0.3f;

    public void Judge(Note note, int playerSector)
    {
        // 已判定過
        if (note.isJudged)
            return;

        // 區域不對
        if (note.targetSector != playerSector)
            return;

        // 音樂時間
        float currentTime = GameManager.Instance.GetMusicTime();

        // 時差
        float diff = Mathf.Abs(currentTime - note.hitTime);

        // 判定
        if (diff <= perfectTime)
        {
            Debug.Log("Perfect");
        }
        else if (diff <= greatTime)
        {
            Debug.Log("Great");
        }
        else if (diff <= goodTime)
        {
            Debug.Log("Good");
        }
        else if (diff <= missTime)
        {
            Debug.Log("Miss");
        }
        else
        {
            return;
        }

        note.isJudged = true;

        Destroy(note.gameObject);
    }
}