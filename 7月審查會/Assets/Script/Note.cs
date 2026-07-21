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



    [Header("譜面資訊")]
    public int noteID;

    public string comment;



    [Header("判定")]
    public bool judged = false;



    //=========================
    // 初始化
    //=========================

    public void Initialize(
        int id,
        Lane lane,
        float time,
        NoteType type,
        JudgeShape shape,
        string text = "")
    {
        noteID = id;

        this.lane = lane;

        hitTime = time;

        noteType = type;

        judgeShape = shape;

        comment = text;

        judged = false;
    }



    //=========================
    // Debug
    //=========================


    private void Awake()
    {
        Debug.Log(
            $"Note Awake : {gameObject.name}"
        );
    }



    private void OnEnable()
    {
        Debug.Log(
            $"Note Enable : {gameObject.name}"
        );
    }



    private void Start()
    {
        Debug.Log(
            $"Note Start : ID:{noteID} " +
            $"Lane:{lane} " +
            $"HitTime:{hitTime} " +
            $"Comment:{comment}"
        );
    }



    private void OnDisable()
    {
        Debug.Log(
            $"Note Disable : {gameObject.name}"
        );
    }



    private void OnDestroy()
    {
        Debug.Log(
            $"Note Destroy : ID:{noteID} " +
            $"Time:{Time.time}"
        );
    }



    //=========================
    // Reset
    //=========================

    public void ResetNote()
    {
        judged = false;

        noteID = 0;

        comment = "";
    }
}