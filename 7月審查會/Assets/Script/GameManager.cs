using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("Judge Window")]
    public float perfectWindow = 0.05f;
    public float greatWindow = 0.10f;
    public float goodWindow = 0.15f;


    [Header("Score")]
    public int combo;
    public int score;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Perfect()
    {
        combo++;
        score += 1000;

        UpdateJudgeUI("PERFECT");
    }

    public void Great()
    {
        combo++;
        score += 700;

        UpdateJudgeUI("GREAT");
    }

    public void Good()
    {
        combo++;
        score += 300;

        UpdateJudgeUI("GOOD");
    }

    public void Miss()
    {
        combo = 0;

        UpdateJudgeUI("MISS");
    }

    public void HoldTick(int addScore = 200)
    {
        combo++;
        score += addScore;

        UpdateScoreUI();
    }

    private void UpdateJudgeUI(string judge)
    {
        if (UIManager.Instance == null)
            return;

        UIManager.Instance.UpdateJudge(judge);
        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    private void UpdateScoreUI()
    {
        if (UIManager.Instance == null)
            return;

        UIManager.Instance.UpdateCombo(combo);
        UIManager.Instance.UpdateScore(score);
    }

    public void ResetGame()
    {
        combo = 0;
        score = 0;

        UpdateScoreUI();
    }
}