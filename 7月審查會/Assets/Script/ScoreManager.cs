using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int combo = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Perfect()
    {
        combo++;
        Debug.Log("Perfect  Combo : " + combo);
    }

    public void Great()
    {
        combo++;
        Debug.Log("Great  Combo : " + combo);
    }

    public void Good()
    {
        combo++;
        Debug.Log("Good  Combo : " + combo);
    }

    public void Miss()
    {
        combo = 0;
        Debug.Log("Miss  Combo : 0");
    }
}