using UnityEngine;

public class HoldNote : MonoBehaviour
{
    public HoldData data;

    [HideInInspector]
    public bool isHolding = false;

    [HideInInspector]
    public bool finished = false;

    public void Miss()
    {
        finished = true;

        if (GameManager.Instance != null)
            GameManager.Instance.Miss();

        Destroy(gameObject);
    }
}