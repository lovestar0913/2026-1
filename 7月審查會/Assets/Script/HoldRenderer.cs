using System.Collections.Generic;
using UnityEngine;

public class HoldRenderer : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject segmentPrefab;

    [Header("Parent")]
    public Transform root;

    [Header("Segment")]
    public float segmentLength = 0.08f;

    public float gap = 0.01f;

    private readonly List<GameObject> segments = new();

    public void Generate(float length)
    {
        Clear();

        float step = segmentLength + gap;

        int count = Mathf.CeilToInt(length / step);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(segmentPrefab, root);

            obj.transform.localPosition =
                new Vector3(i * step, 0f, 0f);

            obj.transform.localRotation =
                Quaternion.identity;

            obj.transform.localScale = Vector3.one;

            segments.Add(obj);
        }
    }

    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        int hideCount =
            Mathf.FloorToInt(progress * segments.Count);

        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].SetActive(i >= hideCount);
        }
    }

    public void SetColor(Color color)
    {
        foreach (GameObject obj in segments)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

            if (sr != null)
                sr.color = color;
        }
    }

    void Clear()
    {
        foreach (GameObject obj in segments)
        {
            if (obj != null)
                Destroy(obj);
        }

        segments.Clear();
    }
}