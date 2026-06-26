using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public Note note;

    public Vector3 startPos;
    public Vector3 targetPos;

    [Header("提前幾秒出現")]
    public float approachTime = 2f;

    void Awake()
    {
        note = GetComponent<Note>();
    }

    void Update()
    {
        float currentTime = GameManager.Instance.MusicTime;

        float spawnTime = note.hitTime - approachTime;

        float t = (currentTime - spawnTime) / approachTime;

        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(startPos, targetPos, t);

        // 超過判定時間一點點就刪掉（之後會改成 Miss 判定）
        if (!note.judged && currentTime > note.hitTime + 0.15f)
        {
            note.judged = true;
            ScoreManager.Instance.Miss();
            Destroy(gameObject);
        }
    }
}