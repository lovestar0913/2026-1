using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HexagonLine : MonoBehaviour
{
    public float radius = 4f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.loop = true;
        line.useWorldSpace = false;
        line.positionCount = 6;

        DrawHexagon();
    }

    void DrawHexagon()
    {
        for (int i = 0; i < 6; i++)
        {
            // 從正上方開始（W）
            float angle = Mathf.Deg2Rad * (90 - i * 60);

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );

            line.SetPosition(i, pos);
        }
    }
}