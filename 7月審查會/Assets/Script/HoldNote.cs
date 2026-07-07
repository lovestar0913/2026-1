using UnityEngine;

public class HoldNote : MonoBehaviour
{
    public HoldData data;

    [HideInInspector]
    public HoldRenderer renderer;

    [HideInInspector]
    public HoldMovement movement;

    private void Awake()
    {
        renderer = GetComponent<HoldRenderer>();
        movement = GetComponent<HoldMovement>();
    }

    private void Start()
    {
        if (data == null)
            return;

        // Hold 長度 = Hold 持續時間
        float length = (data.endTime - data.hitTime) * 2f;

        renderer.Generate(length);

        switch (data.color)
        {
            case HoldColor.Red:
                renderer.SetColor(Color.red);
                break;

            case HoldColor.Blue:
                renderer.SetColor(Color.cyan);
                break;
        }
    }
}