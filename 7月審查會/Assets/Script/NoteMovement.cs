using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public Note note;

    public Vector3 startPos;
    public Vector3 targetPos;

    [Header("提前幾秒出現")]
    public float approachTime = 2f;

    private void Awake()
    {
        if (note == null)
        {
            note = GetComponent<Note>();
        }
    }

    void Update()
    {
        float musicTime = GameManager.Instance.MusicTime;

        float spawnTime = note.hitTime - approachTime;

        float t = (musicTime - spawnTime) / approachTime;

        t = Mathf.Clamp01(t);

        transform.position =
            Vector3.Lerp(
                startPos,
                targetPos,
                t);
    }
}