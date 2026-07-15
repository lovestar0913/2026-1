using UnityEngine;


public enum Lane
{
    Q,
    W,
    E,
    D,
    S,
    A
}


public enum NoteType
{
    Tap,
    Hold
}


public enum JudgeShape
{
    Circle,
    Triangle,
    Square
}



public class Note : MonoBehaviour
{
    [Header("基本資料")]
    public Lane lane;

    public float hitTime;



    [Header("音符種類")]
    public NoteType noteType = NoteType.Tap;



    [Header("判定環形狀")]
    public JudgeShape judgeShape = JudgeShape.Circle;



    [Header("判定")]
    public bool judged = false;



    //=========================
    // Debug
    //=========================

    private void Awake()
    {
        Debug.Log(
            "Note Awake : "
            + gameObject.name
        );
    }


    private void OnEnable()
    {
        Debug.Log(
            "Note Enable : "
            + gameObject.name
        );
    }


    private void Start()
    {
        Debug.Log(
            "Note Start : "
            + gameObject.name
            +
            " HitTime:"
            + hitTime
        );
    }


    private void OnDisable()
    {
        Debug.LogWarning(
            "Note Disable : "
            + gameObject.name
        );
    }


    private void OnDestroy()
    {
        Debug.LogError(
            "Note Destroy : "
            + gameObject.name
            +
            " Time:"
            + Time.time
        );
    }
}