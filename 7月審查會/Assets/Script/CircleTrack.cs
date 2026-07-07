using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleTrack : MonoBehaviour
{
    private LineRenderer line;

    [Header("半圓細緻度")]
    [Range(20, 360)]
    public int segments = 90;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.useWorldSpace = false;
        line.loop = false;
    }

    private void Start()
    {
        DrawTrack();
    }

    public void DrawTrack()
    {
        if (HexagonData.Instance == null)
            return;

        float radius = HexagonData.Instance.trackRadius;

        line.positionCount = segments;
        line.startWidth = HexagonData.Instance.trackWidth;
        line.endWidth = HexagonData.Instance.trackWidth;

        // 固定畫右半圓
        float start = -90f;
        float end = 90f;

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);

            float angle = Mathf.Lerp(start, end, t) * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                -0.01f);

            line.SetPosition(i, pos);
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