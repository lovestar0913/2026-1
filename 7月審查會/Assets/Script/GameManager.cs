using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Chart")]
    public ChartData chartData;

    [Header("Judge Window")]
    public float perfectWindow = 0.05f;
    public float greatWindow = 0.10f;
    public float goodWindow = 0.15f;

    [Header("Score")]
    public int combo;
    public int score;

    [HideInInspector]
    public List<Note> activeNotes = new();

    [HideInInspector]
    public List<HoldNote> activeHolds = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Perfect
    public void Perfect()
    {
        combo++;
        score += 1000;

        UIManager.Instance.UpdateJudge("PERFECT");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    // Great
    public void Great()
    {
        combo++;
        score += 700;

        UIManager.Instance.UpdateJudge("GREAT");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    // Good
    public void Good()
    {
        combo++;
        score += 300;

        UIManager.Instance.UpdateJudge("GOOD");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    // Miss
    public void Miss()
    {
        combo = 0;

        UIManager.Instance.UpdateJudge("MISS");
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    // Hold Tick
    public void HoldTick(int addScore = 200)
    {
        combo++;
        score += addScore;

        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }
}