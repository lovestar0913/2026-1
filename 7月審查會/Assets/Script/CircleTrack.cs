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

    [Header("半徑(0=使用HexagonData)")]
    public float radius = 0f;

    [Header("細緻度")]
    [Range(20, 360)]
    public int segments = 180;

    [Header("線寬(0=使用HexagonData)")]
    public float width = 0f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;

        // 完整圓需要封閉
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

    public void DrawTrack()
    {
        if (line == null)
            return;

        float drawRadius = radius;

        if (drawRadius <= 0f && HexagonData.Instance != null)
            drawRadius = HexagonData.Instance.trackRadius;

        float drawWidth = width;

        if (drawWidth <= 0f && HexagonData.Instance != null)
            drawWidth = HexagonData.Instance.trackWidth;

        line.startWidth = drawWidth;
        line.endWidth = drawWidth;

        if (circleType == CircleType.Full)
        {
            line.loop = true;
            line.positionCount = segments;

            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;

                Vector3 pos = new Vector3(
                    Mathf.Cos(angle) * drawRadius,
                    Mathf.Sin(angle) * drawRadius,
                    -0.01f);

                line.SetPosition(i, pos);
            }
        }
        else
        {
            line.loop = false;
            line.positionCount = segments;

            float startAngle = -90f;
            float endAngle = 90f;

            for (int i = 0; i < segments; i++)
            {
                float t = (float)i / (segments - 1);

                float angle =
                    Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;

                Vector3 pos = new Vector3(
                    Mathf.Cos(angle) * drawRadius,
                    Mathf.Sin(angle) * drawRadius,
                    -0.01f);

                line.SetPosition(i, pos);
            }
        }
    }

    public void Show()
    {
        line.enabled = true;
    }

    public void Hide()
    {
        line.enabled = false;
    }
}