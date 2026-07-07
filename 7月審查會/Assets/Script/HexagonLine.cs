using UnityEngine;

public class HexagonLine : MonoBehaviour
{
    [Header("父物件")]
    public Transform frameParent;
    public Transform redParent;
    public Transform blueParent;

    private HexagonData data;

    void Start()
    {
        data = HexagonData.Instance;

        Generate();
    }

    void Generate()
    {
        Clear(frameParent);
        Clear(redParent);
        Clear(blueParent);

        // 白框
        for (int i = 0; i < 6; i++)
        {
            CreateEdge(
                "Frame_" + i,
                data.FramePoints[i],
                data.FramePoints[(i + 1) % 6],
                data.whiteMat,
                frameParent,
                data.frameWidth);
        }
    }

    void CreateEdge(
        string edgeName,
        Vector3 start,
        Vector3 end,
        Material mat,
        Transform parent,
        float width)
    {
        GameObject edge = GameObject.CreatePrimitive(PrimitiveType.Quad);

        edge.name = edgeName;

        edge.transform.SetParent(parent, false);

        Destroy(edge.GetComponent<Collider>());

        Vector3 center = (start + end) * 0.5f;

        edge.transform.localPosition = center;

        Vector3 dir = end - start;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        edge.transform.localRotation = Quaternion.Euler(0, 0, angle);

        edge.transform.localScale = new Vector3(
            dir.magnitude,
            width,
            1);

        edge.GetComponent<MeshRenderer>().material = mat;
    }

    void Clear(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}