using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public float trackRadius;
    public Transform center;

    public Vector3 GetPosition(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return center.position +
               new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * trackRadius;
    }
}