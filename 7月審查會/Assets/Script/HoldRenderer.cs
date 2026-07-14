using UnityEngine;

public class HoldRenderer : MonoBehaviour
{
    [Header("Parts")]
    public Transform head;
    public Transform body;
    public Transform tail;

    [Header("Body")]
    public float bodyWidth = 1f;

    public HoldColor currentColor;

    private SpriteRenderer bodyRenderer;

    // Hold 原始長度
    private float holdLength;

    // Head 與 Body 的距離
    public float headOffset = 0f;

    // Shrink
    private bool shrinking = false;

    private float shrinkTimer = 0f;

    private float shrinkDuration = 1f;


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
    /// 開始縮短
    /// </summary>
    public void StartShrink()
    {
        shrinking = true;
        shrinkTimer = 0f;
    }

    private void Update()
    {
        if (!shrinking)
            return;

        shrinkTimer += Time.deltaTime;

        float progress =
            shrinkTimer / shrinkDuration;

        SetProgress(progress);

        if (progress >= 1f)
        {
            shrinking = false;
            SetProgress(1f);
        }
    }

    /// <summary>
    /// 設定縮短時間
    /// </summary>
    public void SetShrinkDuration(float duration)
    {
        shrinkDuration = duration;
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
            Mathf.Lerp(
                holdLength,
                0f,
                progress
            );


        //--------------------------------
        // Hold頭固定
        //--------------------------------

        Vector3 headPos =
            Vector3.up * headOffset;


        head.localPosition =
            headPos;



        //--------------------------------
        // 尾巴位置
        //--------------------------------

        Vector3 tailPos =
            Vector3.down * currentLength
            +
            headPos;


        tail.localPosition =
            tailPos;



        //--------------------------------
        // Body中心
        //--------------------------------

        body.localPosition =
            (headPos + tailPos) * 0.5f;



        body.localRotation =
            Quaternion.identity;



        //--------------------------------
        // Body長度
        //--------------------------------

        float bodyLength =
            Vector3.Distance(
                headPos,
                tailPos
            );


        bodyRenderer.size =
            new Vector2(
                bodyWidth,
                bodyLength
            );
    }
    public void SetColor(Color color)
    {
        if (head != null)
            head.GetComponent<SpriteRenderer>().color = color;

        if (bodyRenderer != null)
            bodyRenderer.color = color;

        if (tail != null)
            tail.GetComponent<SpriteRenderer>().color = color;
    }
}