using UnityEngine;

[RequireComponent(typeof(Note))]
public class NoteMovement : MonoBehaviour
{
    private Note note;

    [HideInInspector]
    public Vector3 startPos;

    [HideInInspector]
    public Vector3 targetPos;

    [HideInInspector]
    public float approachTime = 2f;

    [Header("Scale")]
    public float startScale = 0.4f;
    public float endScale = 1f;

    private float spawnTime;

    private bool initialized;

    private void Awake()
    {
        note = GetComponent<Note>();
    }

    private void Start()
    {
        if (note == null)
            return;

        spawnTime = note.hitTime - approachTime;

        transform.position = startPos;
        transform.localScale = Vector3.one * startScale;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (SongManager.Instance == null)
            return;

        float currentTime =
            SongManager.Instance.MusicTime;

        // 進度
        float progress =
            (currentTime - spawnTime) /
            approachTime;

        // 位置
        transform.position =
            Vector3.LerpUnclamped(
                startPos,
                targetPos,
                progress);

        // 大小
        float scale =
            Mathf.Lerp(
                startScale,
                endScale,
                Mathf.Clamp01(progress));

        transform.localScale =
            Vector3.one * scale;
    }
}