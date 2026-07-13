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

    // Hold 原始長度
    private float holdLength;

    // Head 與 Body 的距離
    public float headOffset = 0.25f;

    private void Awake()
    {
        bodyRenderer = body.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 初始化 Hold
    /// </summary>
    public void Generate(float length)
    {
        holdLength = length;

        // 一開始沒有縮短
        UpdateVisual(0f);
    }

    /// <summary>
    /// 更新 Hold 縮短
    /// </summary>
    public void SetProgress(float progress)
    {
        UpdateVisual(progress);
    }

    /// <summary>
    /// 更新外觀
    /// </summary>
    private void UpdateVisual(float progress)
    {
        progress = Mathf.Clamp01(progress);

        float currentLength =
            Mathf.Lerp(holdLength, 0f, progress);

        //------------------------------------------------
        // Head 永遠在最前面
        //------------------------------------------------

        head.localPosition =
            Vector3.up * headOffset;

        //------------------------------------------------
        // Tail 往 Head 靠近
        //------------------------------------------------

        tail.localPosition =
            Vector3.down * currentLength;

        //------------------------------------------------
        // Body 在中間
        //------------------------------------------------

        body.localPosition =
            (head.localPosition + tail.localPosition) * 0.5f;

        body.localRotation = Quaternion.identity;

        bodyRenderer.size =
            new Vector2(
                bodyWidth,
                currentLength + headOffset);
    }

    public void SetColor(Color color)
    {
        head.GetComponent<SpriteRenderer>().color = color;
        bodyRenderer.color = color;
        tail.GetComponent<SpriteRenderer>().color = color;
    }
}