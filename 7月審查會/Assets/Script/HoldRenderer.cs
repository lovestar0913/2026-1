using System.Collections.Generic;
using UnityEngine;

public class HoldRenderer : MonoBehaviour
{
    [Header("Segment")]
    public GameObject segmentPrefab;

    public Transform root;

    [Header("Render")]
    public float segmentLength = 0.08f;

    public float gap = 0.01f;

    private readonly List<SpriteRenderer> segments = new();

    /// <summary>
    /// 生成 Hold
    /// </summary>
    public void Generate(float holdLength)
    {
        Clear();

        float step = segmentLength + gap;

        int count = Mathf.Max(1, Mathf.CeilToInt(holdLength / step));

        for (int i = 0; i < count; i++)
        {
            GameObject obj =
                Instantiate(segmentPrefab, root);

            obj.transform.localPosition =
                new Vector3(i * step, 0, 0);

            obj.transform.localRotation =
                Quaternion.identity;

            SpriteRenderer sr =
                obj.GetComponent<SpriteRenderer>();

            segments.Add(sr);
        }
    }

    /// <summary>
    /// 設定顏色
    /// </summary>
    public void SetColor(Color color)
    {
        foreach (SpriteRenderer sr in segments)
        {
            if (sr != null)
                sr.color = color;
        }
    }

    /// <summary>
    /// Phigros Hold 縮短
    /// progress:
    /// 0 = 剛開始
    /// 1 = Hold 結束
    /// </summary>
    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        int visible =
            Mathf.CeilToInt(
                segments.Count * (1f - progress));

        for (int i = 0; i < segments.Count; i++)
        {
            if (segments[i] == null)
                continue;

            segments[i].enabled =
                i < visible;
        }
    }

    public void Clear()
    {
        foreach (SpriteRenderer sr in segments)
        {
            if (sr == null)
                continue;

            Destroy(sr.gameObject);
        }

        segments.Clear();
    }
}