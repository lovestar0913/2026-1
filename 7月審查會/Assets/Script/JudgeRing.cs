using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class JudgeRing : MonoBehaviour
{
    public Lane lane;

    [Header("Shape")]
    public JudgeShape shape = JudgeShape.Circle;

    [Header("Size")]
    public float radius = 0.5f;

    [Header("Circle Segments")]
    public int circleSegments = 50;

    [Header("Line Width")]
    public float lineWidth = 0.08f;

    private LineRenderer line;

    private JudgeShape currentShape;

    void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.loop = true;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        // 一開始隱藏
        line.enabled = false;
    }

    public void SetShape(JudgeShape newShape)
    {
        if (currentShape == newShape && line.enabled)
            return;

        currentShape = newShape;

        switch (newShape)
        {
            case JudgeShape.Circle:
                DrawCircle();
                break;

            case JudgeShape.Triangle:
                DrawTriangle();
                break;

            case JudgeShape.Square:
                DrawSquare();
                break;
        }

        line.enabled = true;
    }

    public void Hide()
    {
        line.enabled = false;
    }

    void DrawCircle()
    {
        line.positionCount = circleSegments;

        for (int i = 0; i < circleSegments; i++)
        {
            float angle = Mathf.PI * 2f * i / circleSegments;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0);

            line.SetPosition(i, pos);
        }
    }

    void DrawTriangle()
    {
        line.positionCount = 3;

        for (int i = 0; i < 3; i++)
        {
            float angle = Mathf.Deg2Rad * (90 - i * 120);

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0);

            line.SetPosition(i, pos);
        }
    }

    void DrawSquare()
    {
        line.positionCount = 4;

        line.SetPosition(0, new Vector3(-radius, radius, 0));
        line.SetPosition(1, new Vector3(radius, radius, 0));
        line.SetPosition(2, new Vector3(radius, -radius, 0));
        line.SetPosition(3, new Vector3(-radius, -radius, 0));
    }
}