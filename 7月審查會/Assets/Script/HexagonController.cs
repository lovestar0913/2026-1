using UnityEngine;

public class HexagonController : MonoBehaviour
{
    public Transform redTrack;
    public Transform blueTrack;


    private Camera cam;

    private float currentAngle;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        RotateTrack();
    }

    void RotateTrack()
    {
        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

        Vector2 dir = mouse - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        Quaternion rot = Quaternion.Euler(0, 0, angle);

        redTrack.localRotation = rot;
        blueTrack.localRotation = rot;
    }

    public float CurrentAngle => currentAngle;
}