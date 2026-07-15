using UnityEngine;

public class Backgroundmove : MonoBehaviour
{
    [Header("移動幅度")]
    public float xRange = 0.3f;
    public float yRange = 0.2f;

    [Header("平滑速度")]
    public float smoothSpeed = 5f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // 滑鼠位置轉成 -1 ~ 1
        float x = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float y = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Vector3 target =
            startPosition +
            new Vector3(
                x * xRange,
                y * yRange,
                0f);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            target,
            smoothSpeed * Time.deltaTime);
    }
}