using UnityEngine;

public class HexagonController : MonoBehaviour
{
    [Header("Track")]
    public CircleTrack redTrack;
    public CircleTrack blueTrack;


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
        Vector3 mouse =
            cam.ScreenToWorldPoint(
                Input.mousePosition
            );

        mouse.z = 0;



        Vector2 dir =
            mouse - transform.position;



        currentAngle =
            Mathf.Atan2(
                dir.y,
                dir.x
            ) * Mathf.Rad2Deg;



        Quaternion redRotation =
            Quaternion.Euler(
                0,
                0,
                currentAngle
            );



        Quaternion blueRotation =
            Quaternion.Euler(
                0,
                0,
                currentAngle + 180f
            );



        redTrack.transform.localRotation =
            redRotation;


        blueTrack.transform.localRotation =
            blueRotation;



        // ★ 傳送目前角度給判定系統

        redTrack.currentAngle =
            currentAngle;


        blueTrack.currentAngle =
            currentAngle + 180f;
    }
}