using UnityEngine;

public class HoldNote : MonoBehaviour
{
    public HoldData data;

    public bool firstJudgeDone = false;


    // 開始時顏色
    public HoldColor startColor;


    // 目前顏色
    public HoldColor currentColor;


    public int holdButton;


    [HideInInspector]
    public bool isHolding = false;


    [HideInInspector]
    public bool finished = false;


    public float nextTickTime;


    public void StartHold()
    {
        startColor = data.color;

        currentColor = data.color;


        if (startColor == HoldColor.Red)
            holdButton = 0;
        else
            holdButton = 1;


        isHolding = true;
        firstJudgeDone = true;
    }


    public void ChangeColor(HoldColor color)
    {
        currentColor = color;
    }


    public void Miss()
    {
        finished = true;

        if (GameManager.Instance != null)
            GameManager.Instance.Miss();

        Destroy(gameObject);
    }
}