using UnityEngine;

public class HoldRenderer : MonoBehaviour
{
    [Header("Parts")]
    public Transform pivot;

    public Transform head;

    public Transform body;

    public Transform tail;

    private SpriteRenderer bodyRenderer;

    private float totalLength;

    [Header("Body Width")]
    public float bodyWidth = 1f;

    private void Awake()
    {
        bodyRenderer = body.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 初始化 Hold
    /// </summary>
    public void Generate(float length)
    {
        totalLength = Mathf.Max(0.1f, length);

        UpdateBody(totalLength);
    }

    /// <summary>
    /// 每一幀更新
    /// </summary>
    public void Refresh()
    {
        Vector3 headPos = head.position;
        Vector3 tailPos = tail.position;

        Vector3 dir = tailPos - headPos;

        float length = dir.magnitude;

        if (length <= 0.0001f)
            return;

        //------------------------------------------------
        // Body 長度
        //------------------------------------------------

        bodyRenderer.size =
            new Vector2(length, bodyWidth);

        //------------------------------------------------
        // Body 放在 Head
        //------------------------------------------------

        body.position = headPos;

        //------------------------------------------------
        // Body 朝 Tail
        //------------------------------------------------

        float angle =
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        body.rotation =
            Quaternion.Euler(0, 0, angle);
    }

    void UpdateBody(float length)
    {
        bodyRenderer.size =
            new Vector2(length, bodyWidth);
    }

    /// <summary>
    /// Hold縮短
    /// </summary>
    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        float remainLength =
            Mathf.Lerp(totalLength, 0f, progress);

        remainLength = Mathf.Max(remainLength, 0.01f);

        bodyRenderer.size =
            new Vector2(remainLength, bodyWidth);

        body.localPosition =
            Vector3.zero;

        tail.localPosition =
            new Vector3(remainLength, 0f, 0f);
        if (progress >= 1f)
        {
            progress = 1f;
        }
    }

    public void SetColor(Color color)
    {
        head.GetComponent<SpriteRenderer>().color = color;

        bodyRenderer.color = color;

        tail.GetComponent<SpriteRenderer>().color = color;
    }
}