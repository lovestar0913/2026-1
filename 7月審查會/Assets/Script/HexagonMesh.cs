using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexagonMesh : MonoBehaviour
{
    public float radius = 1f;

    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[7];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i);
            vertices[i + 1] = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );
        }

        int[] triangles =
        {
            0,1,2,
            0,2,3,
            0,3,4,
            0,4,5,
            0,5,6,
            0,6,1
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}