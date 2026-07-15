using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("Judge Window")]
    public float perfectWindow = 0.05f;
    public float greatWindow = 0.10f;
    public float goodWindow = 0.15f;



    [Header("Score")]
    public int combo;
    public int maxCombo;
    public int score;



    [Header("Result")]
    public int perfectCount;
    public int greatCount;
    public int goodCount;
    public int missCount;



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



    //=========================
    // Judge
    //=========================


    public void Perfect()
    {
        perfectCount++;

        AddCombo();

        score += 1000;

        UpdateJudgeUI("PERFECT");
    }



    public void Great()
    {
        greatCount++;

        AddCombo();

        score += 700;

        UpdateJudgeUI("GREAT");
    }



    public void Good()
    {
        goodCount++;

        AddCombo();

        score += 300;

        UpdateJudgeUI("GOOD");
    }



    public void Miss()
    {
        missCount++;

        combo = 0;

        UpdateJudgeUI("MISS");
    }



    //=========================
    // Combo
    //=========================


    private void AddCombo()
    {
        combo++;


        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
    }



    // Hold音符持續得分
    public void HoldTick(int addScore = 200)
    {
        score += addScore;

        UpdateScoreUI();
    }



    //=========================
    // UI
    //=========================


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



    //=========================
    // Game Flow
    //=========================


    //歌曲結束
    public void GameEnd()
    {
        SceneManager.LoadScene("GameOver");
    }



    //重新開始歌曲
    public void ResetGame()
    {
        combo = 0;

        maxCombo = 0;

        score = 0;


        perfectCount = 0;

        greatCount = 0;

        goodCount = 0;

        missCount = 0;



        UpdateScoreUI();
    }



    //回Main時使用
    public void BackToMain()
    {
        ResetGame();

        SceneManager.LoadScene("Main");
    }
}