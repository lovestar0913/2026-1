using UnityEngine;

public class HoldRenderer : MonoBehaviour
{
    [Header("Parts")]
    public Transform head;
    public Transform body;
    public Transform tail;

    [Header("Body")]
    public float bodyWidth = 1f;

    private SpriteRenderer bodyRenderer;

    private float totalLength;

    private void Awake()
    {
        bodyRenderer = body.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 建立 Hold
    /// </summary>
    public void Generate(float length)
    {
        totalLength = Mathf.Max(length, 0.01f);

        head.localPosition = Vector3.zero;

        tail.localPosition =
            new Vector3(0, totalLength, 0);

        UpdateBody(totalLength);
    }

    /// <summary>
    /// Hold縮短
    /// </summary>
    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        float remain =
            Mathf.Lerp(totalLength, 0, progress);

        remain = Mathf.Max(remain, 0.01f);

        tail.localPosition =
            new Vector3(0, remain, 0);

        UpdateBody(remain);
    }

    void UpdateBody(float length)
    {
        body.localPosition =
            new Vector3(0, length * 0.5f, 0);

        body.localRotation =
            Quaternion.identity;

        bodyRenderer.size =
            new Vector2(bodyWidth, length);
    }

    public void SetColor(Color color)
    {
        head.GetComponent<SpriteRenderer>().color = color;
        bodyRenderer.color = color;
        tail.GetComponent<SpriteRenderer>().color = color;
    }
}