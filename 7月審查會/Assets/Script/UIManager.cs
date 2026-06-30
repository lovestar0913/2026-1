using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI")]
    public TMP_Text judgeText;
    public TMP_Text comboText;
    public TMP_Text scoreText;

    Vector3 originalScale;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalScale = judgeText.transform.localScale;

        judgeText.text = "";
    }

    public void UpdateJudge(string judge)
    {
        StopAllCoroutines();
        StartCoroutine(ShowJudge(judge));
    }

    IEnumerator ShowJudge(string judge)
    {
        judgeText.text = judge;

        switch (judge)
        {
            case "PERFECT":
                judgeText.color = Color.yellow;
                break;

            case "GREAT":
                judgeText.color = Color.green;
                break;

            case "GOOD":
                judgeText.color = Color.cyan;
                break;

            case "MISS":
                judgeText.color = Color.red;
                break;
        }

        judgeText.transform.localScale = originalScale * 1.4f;

        float timer = 0f;

        while (timer < 0.15f)
        {
            timer += Time.deltaTime;

            judgeText.transform.localScale =
                Vector3.Lerp(
                    originalScale * 1.4f,
                    originalScale,
                    timer / 0.15f);

            yield return null;
        }

        yield return new WaitForSeconds(0.35f);

        judgeText.text = "";
    }

    public void UpdateCombo(int combo)
    {
        comboText.text = combo + " Combo";
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("0000000");
    }
}