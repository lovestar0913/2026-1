using UnityEngine;

public class JudgePointManager : MonoBehaviour
{
    public Transform w;
    public Transform e;
    public Transform d;
    public Transform s;
    public Transform a;
    public Transform q;

    void Start()
    {
        var p = HexagonData.Instance.FramePoints;

        w.localPosition = p[0];
        e.localPosition = p[1];
        d.localPosition = p[2];
        s.localPosition = p[3];
        a.localPosition = p[4];
        q.localPosition = p[5];
    }
}