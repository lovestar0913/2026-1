using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public Note note;

    [HideInInspector]
    public Vector3 startPos;

    [HideInInspector]
    public Vector3 targetPos;

    [Header("提前幾秒生成")]
    public float approachTime = 2f;

    [Header("音符大小")]
    public float startScale = 0.4f;
    public float endScale = 1.0f;

    void Awake()
    {
        note = GetComponent<Note>();
    }

    void Update()
    {
        if (GameManager.Instance == null || note == null)
            return;

        float currentTime = GameManager.Instance.MusicTime;

        // 音符開始出現時間
        float spawnTime = note.hitTime - approachTime;

        // 計算進度
        float t = (currentTime - spawnTime) / approachTime;

        // 超過判定點後繼續往前飛
        transform.position = Vector3.LerpUnclamped(
            startPos,
            targetPos,
            t
        );

        // 音符由小變大
        float scale = Mathf.Lerp(
            startScale,
            endScale,
            Mathf.Clamp01(t)
        );

        transform.localScale = Vector3.one * scale;

        // Miss
        if (!note.judged && currentTime > note.hitTime + GameManager.Instance.goodWindow)
        {
            note.judged = true;

            GameManager.Instance.Miss();

            // 從目前場上的音符移除
            GameManager.Instance.activeNotes.Remove(note);

            Destroy(gameObject);
        }
    }
}