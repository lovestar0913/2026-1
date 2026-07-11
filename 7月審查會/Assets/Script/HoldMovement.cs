using UnityEngine;

[RequireComponent(typeof(HoldRenderer))]
public class HoldMovement : MonoBehaviour
{
    public float approachTime = 2f;

    private Vector3 startPosition;

    private Transform target;

    private float spawnTime;

    private bool initialized;

    private HoldRenderer holdRenderer;

    private void Awake()
    {
        holdRenderer = GetComponent<HoldRenderer>();
    }

    public void Initialize(
        float hitTime,
        Vector3 startPos,
        Transform judge)
    {
        startPosition = startPos;
        target = judge;

        spawnTime = hitTime - approachTime;

        transform.position = startPosition;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (GameManager.Instance == null)
            return;

        float currentTime =
            GameManager.Instance.MusicTime;

        float t =
            Mathf.Clamp01(
            (currentTime - spawnTime) /
            approachTime);

        transform.position =
            Vector3.Lerp(
                startPosition,
                target.position,
                t);

        holdRenderer.Refresh();

        HoldNote holdNote = GetComponentInParent<HoldNote>();

        if (holdNote == null)
            return;

        if (holdNote.finished)
            return;

        // 只有玩家正在按住才開始縮短
        if (holdNote.isHolding)
        {
            float progress = Mathf.InverseLerp(
                holdNote.data.hitTime,
                holdNote.data.endTime,
                GameManager.Instance.MusicTime);

            holdRenderer.SetProgress(progress);

            if (progress >= 1f)
            {
                holdNote.finished = true;

                Destroy(holdNote.gameObject);
            }
        }
    }
}