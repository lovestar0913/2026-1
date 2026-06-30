using UnityEngine;

public class HexagonData : MonoBehaviour
{
    public static HexagonData Instance;

    [Header("大小")]
    public float frameRadius = 3.2f;
    public float trackRadius = 2.95f;

    [Header("線寬")]
    public float frameWidth = 0.08f;
    public float trackWidth = 0.18f;

    [Header("材質")]
    public Material whiteMat;
    public Material redMat;
    public Material blueMat;

    public Vector3[] FramePoints { get; private set; }
    public Vector3[] TrackPoints { get; private set; }

    private void Awake()
    {
        Instance = this;

        FramePoints = new Vector3[6];
        TrackPoints = new Vector3[6];

        GeneratePoints();
    }

    void GeneratePoints()
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (90 - i * 60);

            FramePoints[i] = new Vector3(
                Mathf.Cos(angle) * frameRadius,
                Mathf.Sin(angle) * frameRadius,
                0);

            TrackPoints[i] = new Vector3(
                Mathf.Cos(angle) * trackRadius,
                Mathf.Sin(angle) * trackRadius,
                -0.01f);
        }
    }
}