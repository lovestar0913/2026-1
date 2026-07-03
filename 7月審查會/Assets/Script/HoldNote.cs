using UnityEngine;

public class HoldNote : MonoBehaviour
{
    public HoldData data;

    [HideInInspector]
    public HoldRenderer renderer;

    private HoldMovement movement;

    private void Awake()
    {
        renderer = GetComponent<HoldRenderer>();
        movement = GetComponent<HoldMovement>();
    }

    private void Start()
    {
        float length =
            (data.endTime - data.startTime) * 2f;

        renderer.Generate(length);

        if (data.color == HoldColor.Red)
            renderer.SetColor(Color.red);
        else
            renderer.SetColor(Color.cyan);
    }
}