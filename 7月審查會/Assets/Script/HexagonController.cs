using UnityEngine;

public class HexagonController : MonoBehaviour
{
    [Header("Track")]
    public Transform redTrack;
    public Transform blueTrack;

    private Camera cam;

    private float currentAngle;

    public float CurrentAngle => currentAngle;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        RotateTrack();
    }

    void RotateTrack()
    {
        Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0;

        Vector2 dir = mouse - transform.position;

        currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion redRotation =
            Quaternion.Euler(0, 0, currentAngle);

        Quaternion blueRotation =
            Quaternion.Euler(0, 0, currentAngle + 180f);

        redTrack.localRotation = redRotation;
        blueTrack.localRotation = blueRotation;
    }
}