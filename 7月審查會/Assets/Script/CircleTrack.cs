using UnityEngine;

public enum CircleType
{
    Full,
    Half
}

[RequireComponent(typeof(LineRenderer))]
public class CircleTrack : MonoBehaviour
{
    private LineRenderer line;

    [Header("圓形種類")]
    public CircleType circleType = CircleType.Half;

    [Header("半徑 (0 = 使用 HexagonData)")]
    public float radius = 0f;

    [Header("細緻度")]
    [Range(20, 360)]
    public int segments = 180;

    [Header("線寬 (0 = 使用 HexagonData)")]
    public float width = 0f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.loop = (circleType == CircleType.Full);
    }

    private void Start()
    {
        DrawTrack();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        if (line != null)
        {
            line.loop = (circleType == CircleType.Full);
            DrawTrack();
        }
    }
#endif

    //------------------------------------------------
    // 半徑
    //------------------------------------------------

    public float GetRadius()
    {
        if (radius > 0f)
            return radius;

        if (HexagonData.Instance != null)
            return HexagonData.Instance.trackRadius;

        return 1f;
    }

    //------------------------------------------------
    // 取得世界座標
    //------------------------------------------------

    public Vector3 GetPoint(float angle)
    {
        return GetPoint(angle, transform.position);
    }

    //------------------------------------------------
    // 指定圓心取得世界座標
    //------------------------------------------------

    public Vector3 GetPoint(float angle, Vector3 center)
    {
        float rad = angle * Mathf.Deg2Rad;

        return center +
               new Vector3(
                   Mathf.Cos(rad),
                   Mathf.Sin(rad),
                   0f) * GetRadius();
    }

    //------------------------------------------------
    // 畫圓
    //------------------------------------------------

    public void DrawTrack()
    {
        if (line == null)
            return;

        float drawRadius = GetRadius();

        float drawWidth = width;

        if (drawWidth <= 0f && HexagonData.Instance != null)
            drawWidth = HexagonData.Instance.trackWidth;

        line.startWidth = drawWidth;
        line.endWidth = drawWidth;

        line.loop = (circleType == CircleType.Full);

        line.positionCount = segments;

        float startAngle = 0f;
        float endAngle = 360f;

        if (circleType == CircleType.Half)
        {
            startAngle = -90f;
            endAngle = 90f;
        }

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);

            float angle =
                Mathf.Lerp(startAngle, endAngle, t) *
                Mathf.Deg2Rad;

            Vector3 pos =
                new Vector3(
                    Mathf.Cos(angle),
                    Mathf.Sin(angle),
                    -0.01f) * drawRadius;

            line.SetPosition(i, pos);
        }
    }

    //------------------------------------------------

    public void Show()
    {
        if (line != null)
            line.enabled = true;
    }

    public void Hide()
    {
        if (line != null)
            line.enabled = false;
    }
}