using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class NoteSprite : MonoBehaviour
{
    public Material triangleMat;
    public Material circleMat;
    public Material squareMat;

    private MeshRenderer meshRenderer;
    private Note note;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        note = GetComponent<Note>();
    }

    public void UpdateSprite()
    {
        switch (note.judgeShape)
        {
            case JudgeShape.Triangle:
                meshRenderer.material = triangleMat;
                break;

            case JudgeShape.Circle:
                meshRenderer.material = circleMat;
                break;

            case JudgeShape.Square:
                meshRenderer.material = squareMat;
                break;
        }
    }
}